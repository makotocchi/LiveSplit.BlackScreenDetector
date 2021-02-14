using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Mgs3LoadRemover
{
    public partial class LoadRemoverComponentSettings : UserControl
    {
        public int ColorTolerance { get; private set; }
        public Rectangle CropSettings { get; private set; }
        public IBitmapPrinter BitmapPrinter { get; private set; }
        public IBitmapPrinter CroppedBitmapPrinter { get; private set; }

        private List<string> captureIDs;
        private int processCaptureIndex = -1; // -1 -> full screen, otherwise index process list
        private Process[] processList;
        private string selectedCaptureID = "";
        private bool loadingSettings = false;

        public LoadRemoverComponentSettings()
        {
            InitializeComponent();

            ColorTolerance = 10;
            CropSettings = new Rectangle(0, 0, previewPictureBox.Width, previewPictureBox.Height);
            BitmapPrinter = new DummyPrinter();
            CroppedBitmapPrinter = new CroppedBitmapPrinter(BitmapPrinter, CropSettings);

            RefreshCaptureWindowList();
        }

        public IBitmapPrinter CreateBitmapPrinter()
        {
            try
            {
                //Full screen capture
                if (processCaptureIndex < 0)
                {
                    Screen selectedScreen = Screen.AllScreens[-processCaptureIndex - 1];

                    return new ScreenPrinter(selectedScreen);
                }

                if (processCaptureIndex >= processList.Length)
                {
                    throw new Exception("Process not found.");
                }

                IntPtr handle = IntPtr.Zero;
                if (processCaptureIndex != -1)
                {
                    handle = processList[processCaptureIndex].MainWindowHandle;
                }

                //Capture from specific process
                processList[processCaptureIndex].Refresh();
                if (handle == IntPtr.Zero)
                {
                    throw new Exception("Handle is null.");
                }

                return new WindowPrinter(handle);
            }
            catch
            {
                return new DummyPrinter();
            }
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var settingsNode = document.CreateElement("Settings");

            settingsNode.AppendChild(XmlHelper.ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            settingsNode.AppendChild(XmlHelper.ToElement(document, "ColorTolerance", ColorTolerance));

            if (captureIDs != null && processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
            {
                var selectedCaptureTitle = captureIDs[processListComboBox.SelectedIndex];

                settingsNode.AppendChild(XmlHelper.ToElement(document, "SelectedCaptureTitle", selectedCaptureTitle));
            }

            var captureRegionNode = document.CreateElement("CaptureRegion");

            captureRegionNode.AppendChild(XmlHelper.ToElement(document, "X", CropSettings.X));
            captureRegionNode.AppendChild(XmlHelper.ToElement(document, "Y", CropSettings.Y));
            captureRegionNode.AppendChild(XmlHelper.ToElement(document, "Width", CropSettings.Width));
            captureRegionNode.AppendChild(XmlHelper.ToElement(document, "Height", CropSettings.Height));

            settingsNode.AppendChild(captureRegionNode);

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            loadingSettings = true;

            try
            {
                var element = (XmlElement)settings;
                if (!element.IsEmpty)
                {
                    if (element["ColorTolerance"] != null)
                    {
                        colorToleranceNumericUpDown.Value = Convert.ToInt32(element["ColorTolerance"].InnerText);
                    }

                    if (element["SelectedCaptureTitle"] != null)
                    {
                        string selectedCaptureTitle = element["SelectedCaptureTitle"].InnerText;
                        selectedCaptureID = selectedCaptureTitle;
                        UpdateIndexToCaptureID();
                        RefreshCaptureWindowList();
                    }

                    if (element["CaptureRegion"] != null)
                    {
                        var elementRegion = element["CaptureRegion"];
                        if (elementRegion["X"] != null && elementRegion["Y"] != null && elementRegion["Width"] != null && elementRegion["Height"] != null)
                        {
                            int x = Convert.ToInt32(elementRegion["X"].InnerText);
                            int y = Convert.ToInt32(elementRegion["Y"].InnerText);
                            int width = Convert.ToInt32(elementRegion["Width"].InnerText);
                            int height = Convert.ToInt32(elementRegion["Height"].InnerText);

                            CropSettings = new Rectangle(x, y, width, height);

                            numTopLeftRectX.Value = CropSettings.X;
                            numTopLeftRectY.Value = CropSettings.Y;
                            numBottomRightRectX.Value = CropSettings.Width;
                            numBottomRightRectY.Value = CropSettings.Height;
                        }
                    }
                }
            }
            finally
            {
                UpdatePreview();
                loadingSettings = false;
            }
        }

        private void RefreshCaptureWindowList()
        {
            try
            {
                List<Process> processesWithName = new List<Process>();

                if (captureIDs != null && processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
                {
                    selectedCaptureID = processListComboBox.SelectedItem.ToString();
                }

                captureIDs = new List<string>();

                processListComboBox.Items.Clear();
                
                foreach (var screen in Screen.AllScreens)
                {
                    processListComboBox.Items.Add($"Screen: {screen.DeviceName}, {screen.Bounds}");
                    captureIDs.Add($"Screen: {screen.DeviceName}");
                }

                foreach (Process process in Process.GetProcesses())
                {
                    if (!string.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        processListComboBox.Items.Add($"{process.ProcessName}: {process.MainWindowTitle}");
                        captureIDs.Add(process.ProcessName);
                        processesWithName.Add(process);
                    }
                }

                UpdateIndexToCaptureID();

                processList = processesWithName.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private void UpdateIndexToCaptureID()
        {
            //Find matching window, set selected index to index in dropdown items
            for (int i = 0; i < processListComboBox.Items.Count; i++)
            {
                string item = processListComboBox.Items[i].ToString();
                if (item.Contains(selectedCaptureID))
                {
                    processListComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void UpdatePreview()
        {
            Bitmap bitmap = BitmapPrinter.CaptureImage();

            previewPictureBox.Image = bitmap.ResizeImage(previewPictureBox.Width, previewPictureBox.Height);

            CropSettings = new Rectangle((int)numTopLeftRectX.Value, (int)numTopLeftRectY.Value, (int)numBottomRightRectX.Value, (int)numBottomRightRectY.Value);
            CroppedBitmapPrinter = new CroppedBitmapPrinter(BitmapPrinter, CropSettings);

            croppedPreviewPictureBox.Image = CroppedBitmapPrinter.CaptureImage().ResizeImage(croppedPreviewPictureBox.Width, croppedPreviewPictureBox.Height);
        }

        private void ProcessListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processListComboBox.SelectedIndex < Screen.AllScreens.Length)
            {
                processCaptureIndex = -processListComboBox.SelectedIndex - 1;
            }
            else
            {
                processCaptureIndex = processListComboBox.SelectedIndex - Screen.AllScreens.Length;
            }

            BitmapPrinter = CreateBitmapPrinter();
            UpdatePreview();
        }

        private void ProcessListComboBox_DropDown(object sender, EventArgs e)
        {
            RefreshCaptureWindowList();
        }

        private void UpdatePreviewButton_Click(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdateCropSettings()
        {
            if (!loadingSettings)
            {
                CropSettings = new Rectangle((int)numTopLeftRectX.Value, (int)numTopLeftRectY.Value, (int)numBottomRightRectX.Value, (int)numBottomRightRectY.Value);
                CroppedBitmapPrinter = new CroppedBitmapPrinter(BitmapPrinter, CropSettings);
            }
        }

        private void NumTopLeftRectX_ValueChanged(object sender, EventArgs e)
        {
            UpdateCropSettings();
        }

        private void NumTopLeftRectY_ValueChanged(object sender, EventArgs e)
        {
            UpdateCropSettings();
        }

        private void NumBottomRightRectX_ValueChanged(object sender, EventArgs e)
        {
            UpdateCropSettings();
        }

        private void NumBottomRightRectY_ValueChanged(object sender, EventArgs e)
        {
            UpdateCropSettings();
        }

        private void ColorToleranceNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ColorTolerance = Convert.ToInt32(colorToleranceNumericUpDown.Value);
        }
    }
}