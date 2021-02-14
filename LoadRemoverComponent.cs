using LiveSplit.Mgs3LoadRemover;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    internal class LoadRemoverComponent : IComponent
    {
        public string ComponentName => "MGS3 HD Load Remover";
        public float PaddingBottom => 0;
        public float PaddingTop => 0;
        public float PaddingLeft => 0;
        public float PaddingRight => 0;
        public float VerticalHeight => 0;
        public float MinimumWidth => 0;
        public float HorizontalWidth => 0;
        public float MinimumHeight => 0;
        public IDictionary<string, Action> ContextMenuControls { get; protected set; }
        public LoadRemoverComponentSettings Settings { get; private set; }

        private readonly TimerModel timer;
        private readonly BitmapFeatureDetector bitmapFeatureDetector;
        private readonly ColorDetector colorDetector;

        public LoadRemoverComponent(LiveSplitState state)
        {
            Settings = new LoadRemoverComponentSettings();

            bitmapFeatureDetector = new BitmapFeatureDetector();
            colorDetector = new ColorDetector(Settings.ColorTolerance);

            timer = new TimerModel { CurrentState = state };
            timer.CurrentState.OnStart += Timer_OnStart;
        }

        private void CaptureLoads()
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running)
            {
                return;
            }

            //Capture image using the settings defined for the component
            Bitmap capture = Settings.CroppedBitmapPrinter.CaptureImage();

            if (capture.Width != 1280 || capture.Height != 720)
            {
                capture = capture.ResizeImage(1280, 720);
            }

            bool isLoading;
            try
            {
                var captureTop = capture.Crop(new Rectangle(1119, 53, 50, 50));
                var featuresTop = bitmapFeatureDetector.GetFeaturesFromBitmap(captureTop);
                isLoading = colorDetector.IsWhiteImage(featuresTop);

                if (!isLoading) // Camo index isn't white. There are a few load zones with white background.
                {
                    isLoading = colorDetector.IsBlackImage(featuresTop);

                    if (isLoading) // No camo index, means Snake isn't in the torture room
                    {
                        var captureCenter = capture.Crop(new Rectangle(600, 450, 50, 50));
                        var featuresCenter = bitmapFeatureDetector.GetFeaturesFromBitmap(captureCenter);

                        isLoading = colorDetector.IsBlackImage(featuresCenter);

                        if (isLoading) // Center is black, means Snake isn't active
                        {
                            var captureBottom = capture.Crop(new Rectangle(1200, 600, 50, 50));
                            var featuresBottom = bitmapFeatureDetector.GetFeaturesFromBitmap(captureBottom);

                            isLoading = colorDetector.IsBlackImage(featuresBottom); // Bottom right is black, means it's not a save boss
                        }
                    }
                }
            }
            catch (Exception)
            {
                isLoading = false;
            }

            timer.CurrentState.IsGameTimePaused = isLoading;
        }

        private void Timer_OnStart(object sender, EventArgs e) => timer.InitializeGameTime();
        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) => CaptureLoads();
        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }
        public XmlNode GetSettings(XmlDocument document) => Settings.GetSettings(document);
        public Control GetSettingsControl(LayoutMode mode) => Settings;
        public void SetSettings(XmlNode settings) => Settings.SetSettings(settings);
        public void Dispose() => timer.CurrentState.OnStart -= Timer_OnStart;
    }
}