using LiveSplit.BlackScreenDetector;
using LiveSplit.Model;
using LoadDetector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace LiveSplit.UI.Components
{
    internal class BlackScreenDetectorComponent : IComponent
    {
        public string ComponentName => "Black Screen Detector";
        public float PaddingBottom => 0;
        public float PaddingTop => 0;
        public float PaddingLeft => 0;
        public float PaddingRight => 0;

        public int FrameCount { get; private set; } = 0;

        public IDictionary<string, Action> ContextMenuControls { get; protected set; }

        public BlackScreenDetectorComponentSettings Settings { get; set; }

        private bool saved = false;
        private bool isLoading = false;
        private bool isTransition = false;

        private readonly TimerModel timer;
        private bool timerStarted = false;

        public enum GameState
        {
            RUNNING,
            LOADING
        }

        private GameState gameState = GameState.RUNNING;
        private int runningFrames = 0;
        private int pausedFrames = 0;
        private string gameName = "";
        private string gameCategory = "";
        private List<string> SplitNames { get; set; }

        private LiveSplitState liveSplitState;
        private int framesSinceLastManualSplit = 0;
        private bool lastSplitSkip = false;

        // Black Screen Detector vars
        private DateTime blackScreenStart;

        private bool blackScreenStarted = false;
        private bool blackScreenActive = false;

        private List<int> LoadsPerSplit;

        public BlackScreenDetectorComponent(LiveSplitState state)
        {
            gameName = state.Run.GameName;
            gameCategory = state.Run.CategoryName;
            SplitNames = state.Run.Select(split => split.Name).ToList();

            liveSplitState = state;
            SetLoadsPerSplit();

            Settings = new BlackScreenDetectorComponentSettings(state);
            timer = new TimerModel { CurrentState = state };
            timer.CurrentState.OnStart += Timer_OnStart;
            timer.CurrentState.OnReset += Timer_OnReset;
            timer.CurrentState.OnSkipSplit += Timer_OnSkipSplit;
            timer.CurrentState.OnSplit += Timer_OnSplit;
            timer.CurrentState.OnUndoSplit += Timer_OnUndoSplit;
            timer.CurrentState.OnPause += Timer_OnPause;
            timer.CurrentState.OnResume += Timer_OnResume;
        }

        private void Timer_OnResume(object sender, EventArgs e) => timerStarted = true;

        private void Timer_OnPause(object sender, EventArgs e) => timerStarted = false;

        private void SetLoadsPerSplit()
        {
            if (liveSplitState == null)
            {
                LoadsPerSplit = new List<int>();
            }
            else
            {
                LoadsPerSplit = Enumerable.Repeat(0, liveSplitState.Run.Count).ToList();
                LoadsPerSplit.Add(99999);
            }
        }

        private int CumulativeNumberOfLoadsForSplitIndex(int splitIndex)
        {
            int numberOfLoads = 0;

            for (int idx = 0; idx < LoadsPerSplit.Count && idx <= splitIndex; idx++)
            {
                numberOfLoads += LoadsPerSplit[idx];
            }

            return numberOfLoads;
        }

        private void CaptureLoads()
        {
            try
            {
                if (!timerStarted)
                {
                    return;
                }

                framesSinceLastManualSplit++;

                //Capture image using the settings defined for the component
                Bitmap capture = Settings.CaptureImage();
                //Feed the image to the feature detection

                if (!saved)
                {
                    saved = true;

                    var capture1 = capture.Crop(new Rectangle(1119, 53, 50, 50));
                    var capture2 = capture.Crop(new Rectangle(600, 450, 50, 50));
                    var capture3 = capture.Crop(new Rectangle(1200, 600, 50, 50));

                    capture.Save("test.png", ImageFormat.Png);
                    capture2.Save("test2.png", ImageFormat.Png);
                    capture3.Save("test3.png", ImageFormat.Png);
                }

                int tempMatchingBins = 0;
                bool wasLoading = isLoading;
                bool wasTransition = isTransition;

                float new_avg_transition_max = 0.0f;
                try
                {
                    var captureTop = capture.Crop(new Rectangle(1119, 53, 50, 50));
                    var featuresTop = FeatureDetector.FeaturesFromBitmap(captureTop, out List<int> maxPerPatchTop, out int blackLevelTop, out List<int> minPerPatchTop);
                    isLoading = FeatureDetector.CompareFeatureVectorTransitionWhite(minPerPatchTop, out new_avg_transition_max, out tempMatchingBins, Settings.BlackLevel);

                    if (!isLoading)
                    {
                        var captureCenter = capture.Crop(new Rectangle(600, 450, 50, 50));
                        var captureBottom = capture.Crop(new Rectangle(1200, 600, 50, 50));

                        var featuresCenter = FeatureDetector.FeaturesFromBitmap(captureCenter, out List<int> maxPerPatchCenter, out int blackLevelCenter, out List<int> minPerPatchCenter);
                        var featuresBottom = FeatureDetector.FeaturesFromBitmap(captureBottom, out List<int> maxPerPatchBottom, out int blackLevelBottom, out List<int> minPerPatchBottom);

                        isLoading = FeatureDetector.CompareFeatureVectorTransitionBlack(maxPerPatchTop, out new_avg_transition_max, out tempMatchingBins, Settings.BlackLevel) &&
                                    FeatureDetector.CompareFeatureVectorTransitionBlack(maxPerPatchCenter, out new_avg_transition_max, out tempMatchingBins, Settings.BlackLevel) &&
                                    FeatureDetector.CompareFeatureVectorTransitionBlack(maxPerPatchBottom, out new_avg_transition_max, out tempMatchingBins, Settings.BlackLevel);
                    }
                }
                catch (Exception ex)
                {
                    isLoading = false;
                    Console.WriteLine("Error: " + ex.ToString());
                    throw;
                }

                if (!isLoading)
                {
                    blackScreenStarted = false;
                }
                else
                {
                    // We start a segment. Record the starting time
                    if (blackScreenStarted == false && blackScreenActive == false)
                    {
                        blackScreenStarted = true;
                        blackScreenStart = DateTime.Now;
                    }
                }

                var duration_since_black_screen_start = DateTime.Now - blackScreenStart;

                if (blackScreenStarted && duration_since_black_screen_start.TotalSeconds > Settings.MinimumBlackLength)
                {
                    blackScreenStarted = false;

                    // now we can subtract the duration since the start and set the black screen active (which pauses the timer)

                    timer.CurrentState.LoadingTimes += duration_since_black_screen_start;

                    blackScreenActive = true;
                }

                if (blackScreenActive == true)
                {
                    timer.CurrentState.IsGameTimePaused = true;

                    if (!isLoading)
                    {
                        timer.CurrentState.IsGameTimePaused = false;
                        blackScreenActive = false;
                    }
                }

                if (Settings.AutoSplitterEnabled && !(Settings.AutoSplitterDisableOnSkipUntilSplit && lastSplitSkip))
                {
                    //This is just so that if the detection is not correct by a single frame, it still only splits if a few successive frames are loading
                    if (timer.CurrentState.IsGameTimePaused && gameState == GameState.RUNNING)
                    {
                        pausedFrames++;
                        runningFrames = 0;
                    }
                    else if (!timer.CurrentState.IsGameTimePaused && gameState == GameState.LOADING)
                    {
                        runningFrames++;
                        pausedFrames = 0;
                    }

                    if (gameState == GameState.RUNNING && pausedFrames >= Settings.AutoSplitterJitterToleranceFrames)
                    {
                        runningFrames = 0;
                        pausedFrames = 0;
                        //We enter pause.
                        gameState = GameState.LOADING;
                        if (framesSinceLastManualSplit >= Settings.AutoSplitterManualSplitDelayFrames)
                        {
                            LoadsPerSplit[liveSplitState.CurrentSplitIndex]++;

                            if (CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex) >= Settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
                            {
                                timer.Split();
                            }
                        }
                    }
                    else if (gameState == GameState.LOADING && runningFrames >= Settings.AutoSplitterJitterToleranceFrames)
                    {
                        runningFrames = 0;
                        pausedFrames = 0;
                        //We enter runnning.
                        gameState = GameState.RUNNING;
                    }
                }
            }
            catch (Exception ex)
            {
                isTransition = false;
                isLoading = false;
                Console.WriteLine("Error: " + ex.ToString());
            }
        }

        private void Timer_OnUndoSplit(object sender, EventArgs e)
        {
            //skippedPauses -= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex + 1].Name);
            runningFrames = 0;
            pausedFrames = 0;

            //If we undo a split that already has met the required number of loads, we probably want the number to reset.
            if (LoadsPerSplit[liveSplitState.CurrentSplitIndex] >= Settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
            {
                LoadsPerSplit[liveSplitState.CurrentSplitIndex] = 0;
            }

            //Otherwise - we're fine. If it is a split that was skipped earlier, we still keep track of how we're standing.
        }

        private void Timer_OnSplit(object sender, EventArgs e)
        {
            runningFrames = 0;
            pausedFrames = 0;
            framesSinceLastManualSplit = 0;
            //If we split, we add all remaining loads to the last split.
            //This means that the autosplitter now starts at 0 loads on the next split.
            //This is just necessary for manual splits, as automatic splits will always have a difference of 0.
            var loadsRequiredTotal = Settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex - 1].Name);
            var loadsCurrentTotal = CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex - 1);
            LoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] += loadsRequiredTotal - loadsCurrentTotal;

            lastSplitSkip = false;
        }

        private void Timer_OnSkipSplit(object sender, EventArgs e)
        {
            runningFrames = 0;
            pausedFrames = 0;

            //We don't need to do anything here - we just keep track of loads per split now.
            lastSplitSkip = true;
        }

        private void Timer_OnReset(object sender, TimerPhase value)
        {
            timerStarted = false;
            runningFrames = 0;
            pausedFrames = 0;
            framesSinceLastManualSplit = 0;
            lastSplitSkip = false;
            SetLoadsPerSplit();
        }

        private void Timer_OnStart(object sender, EventArgs e)
        {
            SetLoadsPerSplit();
            timer.InitializeGameTime();
            runningFrames = 0;
            framesSinceLastManualSplit = 0;
            pausedFrames = 0;
            timerStarted = true;
        }

        private bool SplitsAreDifferent(LiveSplitState newState)
        {
            //If GameName / Category is different
            if (gameName != newState.Run.GameName || gameCategory != newState.Run.CategoryName)
            {
                gameName = newState.Run.GameName;
                gameCategory = newState.Run.CategoryName;
                return true;
            }

            //If number of splits is different
            if (newState.Run.Count != liveSplitState.Run.Count)
            {
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
            FrameCount++;

            if (SplitsAreDifferent(state))
            {
                Settings.ChangeAutoSplitSettingsToGameName(gameName, gameCategory);
            }

            liveSplitState = state;

            CaptureLoads();
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
        }

        public float VerticalHeight => 0;

        public float MinimumWidth => 0;

        public float HorizontalWidth => 0;

        public float MinimumHeight => 0;

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
        {
            return Settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            this.Settings.SetSettings(settings);
        }

        public void Dispose()
        {
            timer.CurrentState.OnStart -= Timer_OnStart;
        }
    }
}