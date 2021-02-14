namespace LiveSplit.Mgs3LoadRemover
{
    partial class LoadRemoverComponentSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.capturePreviewLabel = new System.Windows.Forms.Label();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.processListComboBox = new System.Windows.Forms.ComboBox();
            this.croppedPreviewPictureBox = new System.Windows.Forms.PictureBox();
            this.captureLabel = new System.Windows.Forms.Label();
            this.croppedCapturePreviewLabel = new System.Windows.Forms.Label();
            this.bottomRightCropPanel = new System.Windows.Forms.Panel();
            this.bottomRightCropLabel = new System.Windows.Forms.Label();
            this.bottomRightCropYLabel = new System.Windows.Forms.Label();
            this.numBottomRightRectY = new System.Windows.Forms.NumericUpDown();
            this.bottomRightCropXLabel = new System.Windows.Forms.Label();
            this.numBottomRightRectX = new System.Windows.Forms.NumericUpDown();
            this.updatePreviewButton = new System.Windows.Forms.Button();
            this.topLeftCropPanel = new System.Windows.Forms.Panel();
            this.topLeftCropLabel = new System.Windows.Forms.Label();
            this.topLeftCropYLabel = new System.Windows.Forms.Label();
            this.numTopLeftRectY = new System.Windows.Forms.NumericUpDown();
            this.topLeftCropXLabel = new System.Windows.Forms.Label();
            this.numTopLeftRectX = new System.Windows.Forms.NumericUpDown();
            this.cropRectangleLabel = new System.Windows.Forms.Label();
            this.colorToleranceLabel = new System.Windows.Forms.Label();
            this.colorToleranceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).BeginInit();
            this.bottomRightCropPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBottomRightRectY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBottomRightRectX)).BeginInit();
            this.topLeftCropPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopLeftRectY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTopLeftRectX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorToleranceNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // capturePreviewLabel
            // 
            this.capturePreviewLabel.AutoSize = true;
            this.capturePreviewLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capturePreviewLabel.Location = new System.Drawing.Point(23, 88);
            this.capturePreviewLabel.Name = "capturePreviewLabel";
            this.capturePreviewLabel.Size = new System.Drawing.Size(121, 16);
            this.capturePreviewLabel.TabIndex = 25;
            this.capturePreviewLabel.Text = "Capture Preview";
            this.capturePreviewLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPictureBox.Location = new System.Drawing.Point(27, 109);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(426, 240);
            this.previewPictureBox.TabIndex = 24;
            this.previewPictureBox.TabStop = false;
            // 
            // processListComboBox
            // 
            this.processListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processListComboBox.FormattingEnabled = true;
            this.processListComboBox.Items.AddRange(new object[] {
            "Full Display (Primary)"});
            this.processListComboBox.Location = new System.Drawing.Point(82, 12);
            this.processListComboBox.Name = "processListComboBox";
            this.processListComboBox.Size = new System.Drawing.Size(371, 21);
            this.processListComboBox.TabIndex = 22;
            this.processListComboBox.DropDown += new System.EventHandler(this.ProcessListComboBox_DropDown);
            this.processListComboBox.SelectedIndexChanged += new System.EventHandler(this.ProcessListComboBox_SelectedIndexChanged);
            // 
            // croppedPreviewPictureBox
            // 
            this.croppedPreviewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.croppedPreviewPictureBox.Location = new System.Drawing.Point(27, 384);
            this.croppedPreviewPictureBox.Name = "croppedPreviewPictureBox";
            this.croppedPreviewPictureBox.Size = new System.Drawing.Size(256, 144);
            this.croppedPreviewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.croppedPreviewPictureBox.TabIndex = 29;
            this.croppedPreviewPictureBox.TabStop = false;
            // 
            // captureLabel
            // 
            this.captureLabel.AutoSize = true;
            this.captureLabel.Location = new System.Drawing.Point(24, 15);
            this.captureLabel.Name = "captureLabel";
            this.captureLabel.Size = new System.Drawing.Size(47, 13);
            this.captureLabel.TabIndex = 23;
            this.captureLabel.Text = "Capture:";
            // 
            // croppedCapturePreviewLabel
            // 
            this.croppedCapturePreviewLabel.AutoSize = true;
            this.croppedCapturePreviewLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.croppedCapturePreviewLabel.Location = new System.Drawing.Point(23, 364);
            this.croppedCapturePreviewLabel.Name = "croppedCapturePreviewLabel";
            this.croppedCapturePreviewLabel.Size = new System.Drawing.Size(185, 16);
            this.croppedCapturePreviewLabel.TabIndex = 30;
            this.croppedCapturePreviewLabel.Text = "Cropped Capture Preview";
            this.croppedCapturePreviewLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bottomRightCropPanel
            // 
            this.bottomRightCropPanel.BackColor = System.Drawing.SystemColors.Control;
            this.bottomRightCropPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomRightCropPanel.Controls.Add(this.bottomRightCropLabel);
            this.bottomRightCropPanel.Controls.Add(this.bottomRightCropYLabel);
            this.bottomRightCropPanel.Controls.Add(this.numBottomRightRectY);
            this.bottomRightCropPanel.Controls.Add(this.bottomRightCropXLabel);
            this.bottomRightCropPanel.Controls.Add(this.numBottomRightRectX);
            this.bottomRightCropPanel.Location = new System.Drawing.Point(307, 464);
            this.bottomRightCropPanel.Name = "bottomRightCropPanel";
            this.bottomRightCropPanel.Size = new System.Drawing.Size(146, 56);
            this.bottomRightCropPanel.TabIndex = 39;
            // 
            // bottomRightCropLabel
            // 
            this.bottomRightCropLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bottomRightCropLabel.AutoSize = true;
            this.bottomRightCropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomRightCropLabel.Location = new System.Drawing.Point(27, 0);
            this.bottomRightCropLabel.Name = "bottomRightCropLabel";
            this.bottomRightCropLabel.Size = new System.Drawing.Size(96, 16);
            this.bottomRightCropLabel.TabIndex = 45;
            this.bottomRightCropLabel.Text = "Bottom Right";
            this.bottomRightCropLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bottomRightCropYLabel
            // 
            this.bottomRightCropYLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bottomRightCropYLabel.AutoSize = true;
            this.bottomRightCropYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomRightCropYLabel.Location = new System.Drawing.Point(92, 14);
            this.bottomRightCropYLabel.Name = "bottomRightCropYLabel";
            this.bottomRightCropYLabel.Size = new System.Drawing.Size(17, 16);
            this.bottomRightCropYLabel.TabIndex = 43;
            this.bottomRightCropYLabel.Text = "Y";
            // 
            // numBottomRightRectY
            // 
            this.numBottomRightRectY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numBottomRightRectY.Location = new System.Drawing.Point(81, 31);
            this.numBottomRightRectY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numBottomRightRectY.Name = "numBottomRightRectY";
            this.numBottomRightRectY.Size = new System.Drawing.Size(42, 20);
            this.numBottomRightRectY.TabIndex = 44;
            this.numBottomRightRectY.Value = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numBottomRightRectY.ValueChanged += new System.EventHandler(this.NumBottomRightRectY_ValueChanged);
            // 
            // bottomRightCropXLabel
            // 
            this.bottomRightCropXLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bottomRightCropXLabel.AutoSize = true;
            this.bottomRightCropXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomRightCropXLabel.Location = new System.Drawing.Point(38, 14);
            this.bottomRightCropXLabel.Name = "bottomRightCropXLabel";
            this.bottomRightCropXLabel.Size = new System.Drawing.Size(16, 16);
            this.bottomRightCropXLabel.TabIndex = 41;
            this.bottomRightCropXLabel.Text = "X";
            // 
            // numBottomRightRectX
            // 
            this.numBottomRightRectX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numBottomRightRectX.Location = new System.Drawing.Point(27, 31);
            this.numBottomRightRectX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numBottomRightRectX.Name = "numBottomRightRectX";
            this.numBottomRightRectX.Size = new System.Drawing.Size(42, 20);
            this.numBottomRightRectX.TabIndex = 42;
            this.numBottomRightRectX.Value = new decimal(new int[] {
            426,
            0,
            0,
            0});
            this.numBottomRightRectX.ValueChanged += new System.EventHandler(this.NumBottomRightRectX_ValueChanged);
            // 
            // updatePreviewButton
            // 
            this.updatePreviewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatePreviewButton.Location = new System.Drawing.Point(271, 49);
            this.updatePreviewButton.Name = "updatePreviewButton";
            this.updatePreviewButton.Size = new System.Drawing.Size(182, 33);
            this.updatePreviewButton.TabIndex = 37;
            this.updatePreviewButton.Text = "Update Preview";
            this.updatePreviewButton.UseVisualStyleBackColor = true;
            this.updatePreviewButton.Click += new System.EventHandler(this.UpdatePreviewButton_Click);
            // 
            // topLeftCropPanel
            // 
            this.topLeftCropPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topLeftCropPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.topLeftCropPanel.Controls.Add(this.topLeftCropLabel);
            this.topLeftCropPanel.Controls.Add(this.topLeftCropYLabel);
            this.topLeftCropPanel.Controls.Add(this.numTopLeftRectY);
            this.topLeftCropPanel.Controls.Add(this.topLeftCropXLabel);
            this.topLeftCropPanel.Controls.Add(this.numTopLeftRectX);
            this.topLeftCropPanel.Location = new System.Drawing.Point(307, 402);
            this.topLeftCropPanel.Name = "topLeftCropPanel";
            this.topLeftCropPanel.Size = new System.Drawing.Size(146, 56);
            this.topLeftCropPanel.TabIndex = 46;
            // 
            // topLeftCropLabel
            // 
            this.topLeftCropLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.topLeftCropLabel.AutoSize = true;
            this.topLeftCropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLeftCropLabel.Location = new System.Drawing.Point(41, 0);
            this.topLeftCropLabel.Name = "topLeftCropLabel";
            this.topLeftCropLabel.Size = new System.Drawing.Size(65, 16);
            this.topLeftCropLabel.TabIndex = 45;
            this.topLeftCropLabel.Text = "Top Left";
            this.topLeftCropLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // topLeftCropYLabel
            // 
            this.topLeftCropYLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.topLeftCropYLabel.AutoSize = true;
            this.topLeftCropYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLeftCropYLabel.Location = new System.Drawing.Point(92, 14);
            this.topLeftCropYLabel.Name = "topLeftCropYLabel";
            this.topLeftCropYLabel.Size = new System.Drawing.Size(17, 16);
            this.topLeftCropYLabel.TabIndex = 43;
            this.topLeftCropYLabel.Text = "Y";
            // 
            // numTopLeftRectY
            // 
            this.numTopLeftRectY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTopLeftRectY.Location = new System.Drawing.Point(81, 31);
            this.numTopLeftRectY.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numTopLeftRectY.Name = "numTopLeftRectY";
            this.numTopLeftRectY.Size = new System.Drawing.Size(42, 20);
            this.numTopLeftRectY.TabIndex = 44;
            this.numTopLeftRectY.ValueChanged += new System.EventHandler(this.NumTopLeftRectY_ValueChanged);
            // 
            // topLeftCropXLabel
            // 
            this.topLeftCropXLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.topLeftCropXLabel.AutoSize = true;
            this.topLeftCropXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLeftCropXLabel.Location = new System.Drawing.Point(38, 14);
            this.topLeftCropXLabel.Name = "topLeftCropXLabel";
            this.topLeftCropXLabel.Size = new System.Drawing.Size(16, 16);
            this.topLeftCropXLabel.TabIndex = 41;
            this.topLeftCropXLabel.Text = "X";
            // 
            // numTopLeftRectX
            // 
            this.numTopLeftRectX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTopLeftRectX.Location = new System.Drawing.Point(27, 31);
            this.numTopLeftRectX.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numTopLeftRectX.Name = "numTopLeftRectX";
            this.numTopLeftRectX.Size = new System.Drawing.Size(42, 20);
            this.numTopLeftRectX.TabIndex = 42;
            this.numTopLeftRectX.ValueChanged += new System.EventHandler(this.NumTopLeftRectX_ValueChanged);
            // 
            // cropRectangleLabel
            // 
            this.cropRectangleLabel.AutoSize = true;
            this.cropRectangleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cropRectangleLabel.Location = new System.Drawing.Point(307, 383);
            this.cropRectangleLabel.Name = "cropRectangleLabel";
            this.cropRectangleLabel.Size = new System.Drawing.Size(116, 16);
            this.cropRectangleLabel.TabIndex = 47;
            this.cropRectangleLabel.Text = "Crop Rectangle";
            this.cropRectangleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // colorToleranceLabel
            // 
            this.colorToleranceLabel.AutoSize = true;
            this.colorToleranceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorToleranceLabel.Location = new System.Drawing.Point(24, 43);
            this.colorToleranceLabel.Name = "colorToleranceLabel";
            this.colorToleranceLabel.Size = new System.Drawing.Size(108, 16);
            this.colorToleranceLabel.TabIndex = 39;
            this.colorToleranceLabel.Text = "Color Tolerance:";
            // 
            // colorToleranceNumericUpDown
            // 
            this.colorToleranceNumericUpDown.Location = new System.Drawing.Point(27, 62);
            this.colorToleranceNumericUpDown.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.colorToleranceNumericUpDown.Name = "colorToleranceNumericUpDown";
            this.colorToleranceNumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.colorToleranceNumericUpDown.TabIndex = 40;
            this.colorToleranceNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.colorToleranceNumericUpDown.ValueChanged += new System.EventHandler(this.ColorToleranceNumericUpDown_ValueChanged);
            // 
            // LoadRemoverComponentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.colorToleranceNumericUpDown);
            this.Controls.Add(this.colorToleranceLabel);
            this.Controls.Add(this.cropRectangleLabel);
            this.Controls.Add(this.topLeftCropPanel);
            this.Controls.Add(this.updatePreviewButton);
            this.Controls.Add(this.bottomRightCropPanel);
            this.Controls.Add(this.croppedCapturePreviewLabel);
            this.Controls.Add(this.captureLabel);
            this.Controls.Add(this.croppedPreviewPictureBox);
            this.Controls.Add(this.processListComboBox);
            this.Controls.Add(this.previewPictureBox);
            this.Controls.Add(this.capturePreviewLabel);
            this.Location = new System.Drawing.Point(4, 22);
            this.Name = "LoadRemoverComponentSettings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(474, 538);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).EndInit();
            this.bottomRightCropPanel.ResumeLayout(false);
            this.bottomRightCropPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBottomRightRectY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBottomRightRectX)).EndInit();
            this.topLeftCropPanel.ResumeLayout(false);
            this.topLeftCropPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopLeftRectY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTopLeftRectX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorToleranceNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown colorToleranceNumericUpDown;
        private System.Windows.Forms.Label colorToleranceLabel;
        private System.Windows.Forms.Label cropRectangleLabel;
        private System.Windows.Forms.Panel topLeftCropPanel;
        private System.Windows.Forms.Label topLeftCropLabel;
        private System.Windows.Forms.Label topLeftCropYLabel;
        private System.Windows.Forms.NumericUpDown numTopLeftRectY;
        private System.Windows.Forms.Label topLeftCropXLabel;
        private System.Windows.Forms.NumericUpDown numTopLeftRectX;
        private System.Windows.Forms.Button updatePreviewButton;
        private System.Windows.Forms.Panel bottomRightCropPanel;
        private System.Windows.Forms.Label bottomRightCropLabel;
        private System.Windows.Forms.Label bottomRightCropYLabel;
        private System.Windows.Forms.NumericUpDown numBottomRightRectY;
        private System.Windows.Forms.Label bottomRightCropXLabel;
        private System.Windows.Forms.NumericUpDown numBottomRightRectX;
        private System.Windows.Forms.Label croppedCapturePreviewLabel;
        private System.Windows.Forms.Label captureLabel;
        private System.Windows.Forms.PictureBox croppedPreviewPictureBox;
        private System.Windows.Forms.ComboBox processListComboBox;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.Label capturePreviewLabel;
    }
}
