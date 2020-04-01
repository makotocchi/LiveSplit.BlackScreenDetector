﻿using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using CrashNSaneLoadDetector;
using System.IO;
using System.Drawing.Imaging;
//using System.Threading;

namespace LiveSplit.UI.Components
{
	class BlackScreenDetectorComponent : IComponent
	{
		public string ComponentName
		{
			get { return "Black Screen Detector"; }
		}
		public GraphicsCache Cache { get; set; }


		public float PaddingBottom { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public int frame_count = 0;
		public bool Refresh { get; set; }

		public IDictionary<string, Action> ContextMenuControls { get; protected set; }

		public BlackScreenDetectorComponentSettings settings { get; set; }

		private bool isLoading = false;
		private bool isTransition = false;
		private int matchingBins = 0;
		private float numSecondsTransitionMax = 10.0f; // A transition can at most be 5 seconds long, otherwise it is not counted

		private TimerModel timer;
		private bool timerStarted = false;
		private bool postLoadTransition = false;
		private bool first_frame_post_load_transition = false;
		private double total_paused_time = 0.0f;
		private string log_file_name = "";
		FileStream log_file_stream = null;
		StreamWriter log_file_writer = null;

		public enum CrashNSTState
		{
			RUNNING,
			LOADING
		}

		private CrashNSTState NSTState = CrashNSTState.RUNNING;
		private int runningFrames = 0;
		private int pausedFrames = 0;
		private int pausedFramesSegment = 0;
		private string GameName = "";
		private string GameCategory = "";
		private int NumberOfSplits = 0;
		private List<string> SplitNames;
		private DateTime lastTime;
		private DateTime transitionStart;

		private DateTime segmentTimeStart;
		private LiveSplitState liveSplitState;
		//private Thread captureThread;
		private bool threadRunning = false;
		private double framesSum = 0.0;
		private int framesSumRounded = 0;
		private int framesSinceLastManualSplit = 0;
		private bool LastSplitSkip = false;

		// Black Screen Detector vars
		private DateTime black_screen_start;
		private bool black_screen_started = false;
		private bool black_screen_active = false;


		//private HighResolutionTimer.HighResolutionTimer highResTimer;
		private List<int> NumberOfLoadsPerSplit;

		public BlackScreenDetectorComponent(LiveSplitState state)
		{

			GameName = state.Run.GameName;
			GameCategory = state.Run.CategoryName;
			NumberOfSplits = state.Run.Count;
			SplitNames = new List<string>();

			foreach (var split in state.Run)
			{
				SplitNames.Add(split.Name);
			}

			liveSplitState = state;
			NumberOfLoadsPerSplit = new List<int>();
			InitNumberOfLoadsFromState();

			settings = new BlackScreenDetectorComponentSettings(state);
			lastTime = DateTime.Now;
			segmentTimeStart = DateTime.Now;
			timer = new TimerModel { CurrentState = state };
			timer.CurrentState.OnStart += timer_OnStart;
			timer.CurrentState.OnReset += timer_OnReset;
			timer.CurrentState.OnSkipSplit += timer_OnSkipSplit;
			timer.CurrentState.OnSplit += timer_OnSplit;
			timer.CurrentState.OnUndoSplit += timer_OnUndoSplit;
			timer.CurrentState.OnPause += timer_OnPause;
			timer.CurrentState.OnResume += timer_OnResume;
			//highResTimer = new HighResolutionTimer.HighResolutionTimer(16.0f);
			//highResTimer.Elapsed += (s, e) => { CaptureLoads(); };
		}

		private void timer_OnResume(object sender, EventArgs e)
		{
			timerStarted = true;
		}

		private void timer_OnPause(object sender, EventArgs e)
		{
			timerStarted = false;
		}

		private void InitNumberOfLoadsFromState()
		{
			NumberOfLoadsPerSplit = new List<int>();
			NumberOfLoadsPerSplit.Clear();

			if (liveSplitState == null)
			{
				return;
			}

			foreach (var split in liveSplitState.Run)
			{
				NumberOfLoadsPerSplit.Add(0);
			}

			//Quicker way to prevent OOB on last split as I'm not sure if the index will go over if the run finishes
			NumberOfLoadsPerSplit.Add(99999);
		}

		private int CumulativeNumberOfLoadsForSplitIndex(int splitIndex)
		{
			int numberOfLoads = 0;

			for (int idx = 0; (idx < NumberOfLoadsPerSplit.Count && idx <= splitIndex); idx++)
			{
				numberOfLoads += NumberOfLoadsPerSplit[idx];
			}
			return numberOfLoads;
		}

		private void CaptureLoads()
		{
			try
			{


				if (timerStarted)
				{
					framesSinceLastManualSplit++;
					//Console.WriteLine("TIME NOW: {0}", DateTime.Now - lastTime);
					//Console.WriteLine("TIME DIFF START: {0}", DateTime.Now - lastTime);
					lastTime = DateTime.Now;

					//Capture image using the settings defined for the component
					Bitmap capture = settings.CaptureImage();
					List<int> max_per_patch;
					List<int> min_per_patch;
					//Feed the image to the feature detection
					int black_level = 0;

					var features = FeatureDetector.featuresFromBitmap(capture, out max_per_patch, out black_level, out min_per_patch);

					int tempMatchingBins = 0;
					bool wasLoading = isLoading;
					bool wasTransition = isTransition;

					//TODO: should we "learn" the black level automatically? we could do this during the first few transitions, by keeping track of the minimum histogram values, and dynamically adjusting the number of black bins?

					float new_avg_transition_max = 0.0f;
					try
					{

						//DateTime current_time = DateTime.Now;
						isLoading = FeatureDetector.compareFeatureVectorTransition(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, max_per_patch, min_per_patch, -1.0f, out new_avg_transition_max, out tempMatchingBins, 0.8f, false, settings.BlackLevel);//FeatureDetector.isGameTransition(capture, 30);
																																																																   //Console.WriteLine("Timing for detection: {0}", (DateTime.Now - current_time).TotalSeconds);
					}
					catch (Exception ex)
					{
						isLoading = false;
						Console.WriteLine("Error: " + ex.ToString());
						throw ex;
					}
					

					matchingBins = tempMatchingBins;

					//timer.CurrentState.IsGameTimePaused = isLoading;

					/*if (settings.RemoveFadeins || settings.RemoveFadeouts)
					{
						try
						{
							isTransition = FeatureDetector.compareFeatureVectorTransition(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, max_per_patch, min_per_patch, -1.0f, out new_avg_transition_max, out tempMatchingBins, 0.8f, false);//FeatureDetector.isGameTransition(capture, 30);
						}
						catch (Exception ex)
						{
							isTransition = false;
							Console.WriteLine("Error: " + ex.ToString());
							throw ex;
						}
						//Console.WriteLine("Transition: {0}", isTransition);
						if (wasLoading && isTransition && settings.RemoveFadeins)
						{
							postLoadTransition = true;
							transitionStart = DateTime.Now;
						}

						if (wasTransition == false && isTransition && settings.RemoveFadeouts)
						{
							//This could be a pre-load transition, start timing it
							transitionStart = DateTime.Now;
						}


						//Console.WriteLine("GAMETIMEPAUSETIME: {0}", timer.CurrentState.GameTimePauseTime);


						if (wasTransition && isLoading)
						{
							// This was a pre-load transition, subtract the gametime
							TimeSpan delta = (DateTime.Now - transitionStart);

							if (settings.RemoveFadeouts)
							{
								if (delta.TotalSeconds > numSecondsTransitionMax)
								{
									Console.WriteLine("{2}: Transition longer than {0} seconds, doesn't count! (Took {1} seconds)", numSecondsTransitionMax, delta.TotalSeconds, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
								}
								else
								{
									timer.CurrentState.SetGameTime(timer.CurrentState.GameTimePauseTime - delta);

									total_paused_time += delta.TotalSeconds;
									Console.WriteLine("PRE-LOAD TRANSITION {2} seconds: {0}, totalPausedTime: {1}", delta.TotalSeconds, total_paused_time, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
								}

							}

						}

						if (settings.RemoveFadeins)
						{
							if (postLoadTransition && isTransition == false)
							{
								TimeSpan delta = (DateTime.Now - transitionStart);

								total_paused_time += delta.TotalSeconds;
								Console.WriteLine("POST-LOAD TRANSITION {2} seconds: {0}, totalPausedTime: {1}", delta.TotalSeconds, total_paused_time, SplitNames[Math.Max(Math.Min(liveSplitState.CurrentSplitIndex, SplitNames.Count - 1), 0)]);
							}


							if (postLoadTransition == true && isTransition)
							{
								// We are transitioning after a load screen, this stops the timer, and actually increases the load time
								timer.CurrentState.IsGameTimePaused = true;


							}
							else
							{
								postLoadTransition = false;
							}

						}

					}
					*/

					if (!isLoading)
					{
						black_screen_started = false;
					}
					else
					{
						// We start a segment. Record the starting time
						if(black_screen_started == false && black_screen_active == false)
						{
							black_screen_started = true;
							black_screen_start = DateTime.Now;

							//Console.WriteLine("Timer GameTime at start: " + timer.CurrentState.GameTimePauseTime);
						}
					}

					var duration_since_black_screen_start = DateTime.Now - black_screen_start;

					if(black_screen_started && duration_since_black_screen_start.TotalSeconds > settings.MinimumBlackLength)
					{
						black_screen_started = false;

						// now we can subtract the duration since the start and set the black screen active (which pauses the timer)

						//Console.WriteLine("Duration since black start: " + duration_since_black_screen_start);

						//Console.WriteLine("Timer GameTime at active: " + timer.CurrentState.GameTimePauseTime);
						

						timer.CurrentState.LoadingTimes += duration_since_black_screen_start;
						
						black_screen_active = true;
					}


					if(black_screen_active == true)
					{
						timer.CurrentState.IsGameTimePaused = true;

						if(!isLoading)
						{
							timer.CurrentState.IsGameTimePaused = false;
							black_screen_active = false;
						}

					}

					//Console.WriteLine("Timer GameTime at frame: " + timer.CurrentState.GameTimePauseTime);


					if (settings.AutoSplitterEnabled && !(settings.AutoSplitterDisableOnSkipUntilSplit && LastSplitSkip))
					{
						//This is just so that if the detection is not correct by a single frame, it still only splits if a few successive frames are loading
						if (timer.CurrentState.IsGameTimePaused && NSTState == CrashNSTState.RUNNING)
						{
							pausedFrames++;
							runningFrames = 0;
						}
						else if (!timer.CurrentState.IsGameTimePaused && NSTState == CrashNSTState.LOADING)
						{
							runningFrames++;
							pausedFrames = 0;
						}

						if (NSTState == CrashNSTState.RUNNING && pausedFrames >= settings.AutoSplitterJitterToleranceFrames)
						{
							runningFrames = 0;
							pausedFrames = 0;
							//We enter pause.
							NSTState = CrashNSTState.LOADING;
							if (framesSinceLastManualSplit >= settings.AutoSplitterManualSplitDelayFrames)
							{
								NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex]++;

								if (CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex) >= settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
								{

									timer.Split();


								}
							}

						}
						else if (NSTState == CrashNSTState.LOADING && runningFrames >= settings.AutoSplitterJitterToleranceFrames)
						{
							runningFrames = 0;
							pausedFrames = 0;
							//We enter runnning.
							NSTState = CrashNSTState.RUNNING;
						}
					}


					//Console.WriteLine("TIME TAKEN FOR DETECTION: {0}", DateTime.Now - lastTime);
				}
			}
			catch (Exception ex)
			{
				isTransition = false;
				isLoading = false;
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		private void timer_OnUndoSplit(object sender, EventArgs e)
		{
			//skippedPauses -= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex + 1].Name);
			runningFrames = 0;
			pausedFrames = 0;

			//If we undo a split that already has met the required number of loads, we probably want the number to reset.
			if (NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] >= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
			{
				NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] = 0;
			}

			//Otherwise - we're fine. If it is a split that was skipped earlier, we still keep track of how we're standing.


		}

		private void timer_OnSplit(object sender, EventArgs e)
		{
			runningFrames = 0;
			pausedFrames = 0;
			framesSinceLastManualSplit = 0;
			//If we split, we add all remaining loads to the last split.
			//This means that the autosplitter now starts at 0 loads on the next split.
			//This is just necessary for manual splits, as automatic splits will always have a difference of 0.
			var loadsRequiredTotal = settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex - 1].Name);
			var loadsCurrentTotal = CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex - 1);
			NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] += loadsRequiredTotal - loadsCurrentTotal;

			LastSplitSkip = false;
		}

		private void timer_OnSkipSplit(object sender, EventArgs e)
		{

			runningFrames = 0;
			pausedFrames = 0;

			//We don't need to do anything here - we just keep track of loads per split now.
			LastSplitSkip = true;

			/*if(settings.AutoSplitterDisableOnSkipUntilSplit)
				  {
					  NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] = 0;
				  }*/
		}

		private void timer_OnReset(object sender, TimerPhase value)
		{
			timerStarted = false;
			runningFrames = 0;
			pausedFrames = 0;
			framesSinceLastManualSplit = 0;
			threadRunning = false;
			LastSplitSkip = false;
			//highResTimer.Stop(joinThread:false);
			InitNumberOfLoadsFromState();

			first_frame_post_load_transition = false;
			total_paused_time = 0.0f;


			if (log_file_writer != null)
			{
				if (log_file_writer.BaseStream != null)
				{
					log_file_writer.Flush();
					log_file_writer.Close();
					log_file_writer.Dispose();
				}
				log_file_writer = null;
			}

		}

		void timer_OnStart(object sender, EventArgs e)
		{
			InitNumberOfLoadsFromState();
			timer.InitializeGameTime();
			runningFrames = 0;
			framesSinceLastManualSplit = 0;
			pausedFrames = 0;
			timerStarted = true;
			threadRunning = true;
			first_frame_post_load_transition = false;
			total_paused_time = 0.0f;

			ReloadLogFile();
			//StartCaptureThread();
			//highResTimer.Start();
		}

		void StartCaptureThread()
		{
			//captureThread = new Thread(() =>
			//{
			//	System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			//	while (threadRunning)
			//	{
			//watch.Restart();
			//		CaptureLoads();
			//TODO: test rounding of framecounts in output, more importantly:
			//TEST FINAL TIME TO SEE IF IT IS ACCURATE WITH THIS,
			//THEN ADD SLEEPS FOR PERFORMANCE
			//THEN ADJUST FOR BETTER PERFORMANCE

			/*Thread.Sleep(Math.Max((int)(captureDelay - watch.Elapsed.TotalMilliseconds - 1), 0));
			while(captureDelay - watch.Elapsed.TotalMilliseconds >= 0)
			{
			  ;
			}*/
			//	}
			//});
			//captureThread.Start();*/
		}

		private void ReloadLogFile()
		{
			if (settings.SaveDetectionLog == false)
				return;


			System.IO.Directory.CreateDirectory(settings.DetectionLogFolderName);

			string fileName = Path.Combine(settings.DetectionLogFolderName, "CTRNitroFueledLoadRemover_Log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_") + settings.removeInvalidXMLCharacters(GameName) + "_" + settings.removeInvalidXMLCharacters(GameCategory) + ".txt");

			if (log_file_writer != null)
			{
				if (log_file_writer.BaseStream != null)
				{
					log_file_writer.Flush();
					log_file_writer.Close();
					log_file_writer.Dispose();
				}
				log_file_writer = null;
			}


			log_file_stream = new FileStream(fileName, FileMode.Create);
			log_file_writer = new StreamWriter(log_file_stream);
			log_file_writer.AutoFlush = true;
			Console.SetOut(log_file_writer);
			Console.SetError(log_file_writer);

		}

		private bool SplitsAreDifferent(LiveSplitState newState)
		{
			//If GameName / Category is different
			if (GameName != newState.Run.GameName || GameCategory != newState.Run.CategoryName)
			{
				GameName = newState.Run.GameName;
				GameCategory = newState.Run.CategoryName;
				return true;
			}

			//If number of splits is different
			if (newState.Run.Count != liveSplitState.Run.Count)
			{
				NumberOfSplits = newState.Run.Count;
				return true;
			}

			//Check if any split name is different.
			for (int splitIdx = 0; splitIdx < newState.Run.Count; splitIdx++)
			{
				if (newState.Run[splitIdx].Name != SplitNames[splitIdx])
				{

					SplitNames = new List<string>();

					foreach (var split in newState.Run)
					{
						SplitNames.Add(split.Name);
					}

					return true;
				}

			}



			return false;
		}
		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
		{
			frame_count++;

			if (SplitsAreDifferent(state))
			{
				settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);

				ReloadLogFile();
			}

			if (settings.RecordImages && (frame_count % 3) == 0)
			{
				settings.StoreCaptureImage(GameName, GameCategory);
			}

			liveSplitState = state;
			/*
				  liveSplitState = state;
				  if (GameName != state.Run.GameName || GameCategory != state.Run.CategoryName)
				  {
					  //Reload settings for different game or category
					  GameName = state.Run.GameName;
					  GameCategory = state.Run.CategoryName;

					  settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);
				  }
				  */



			CaptureLoads();




		}

		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
		{

		}

		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
		{

		}

		public float VerticalHeight
		{
			get { return 0; }
		}

		public float MinimumWidth
		{
			get { return 0; }
		}

		public float HorizontalWidth
		{
			get { return 0; }
		}

		public float MinimumHeight
		{
			get { return 0; }
		}

		public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
		{
			return settings.GetSettings(document);
		}

		public System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
		{
			return settings;
		}

		public void SetSettings(System.Xml.XmlNode settings)
		{
			this.settings.SetSettings(settings);
		}

		public void RenameComparison(string oldName, string newName)
		{
		}

		public void Dispose()
		{
			timer.CurrentState.OnStart -= timer_OnStart;

			if (log_file_writer != null)
			{
				if (log_file_writer.BaseStream != null)
				{
					log_file_writer.Flush();
					log_file_writer.Close();
					log_file_writer.Dispose();
				}
				log_file_writer = null;
			}

		}
	}

}
