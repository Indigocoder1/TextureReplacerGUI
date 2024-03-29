﻿namespace TextureReplacerGUI
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.classIDDropdown = new System.Windows.Forms.ComboBox();
            this.configNameBox = new System.Windows.Forms.TextBox();
            this.configNameLabel = new System.Windows.Forms.Label();
            this.materialIndexLabel = new System.Windows.Forms.Label();
            this.materialBox = new System.Windows.Forms.TextBox();
            this.textureFileNameLabel = new System.Windows.Forms.Label();
            this.texFileNameBox = new System.Windows.Forms.TextBox();
            this.classIDLabel = new System.Windows.Forms.Label();
            this.hierarchyPathLabel = new System.Windows.Forms.Label();
            this.hierarchyPathBox = new System.Windows.Forms.TextBox();
            this.textureNameLabel = new System.Windows.Forms.Label();
            this.textureNameBox = new System.Windows.Forms.TextBox();
            this.variationToggle = new System.Windows.Forms.CheckBox();
            this.variationChanceBox = new System.Windows.Forms.TextBox();
            this.variationChanceLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.glControl1 = new OpenTK.GLControl();
            this.loadFolderBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.previewLabel = new System.Windows.Forms.Label();
            this.ExportCfgBtn = new System.Windows.Forms.Button();
            this.SaveCfgBtn = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // classIDDropdown
            // 
            this.classIDDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.classIDDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.classIDDropdown.FormattingEnabled = true;
            this.classIDDropdown.Location = new System.Drawing.Point(24, 172);
            this.classIDDropdown.Name = "classIDDropdown";
            this.classIDDropdown.Size = new System.Drawing.Size(512, 24);
            this.classIDDropdown.TabIndex = 0;
            this.classIDDropdown.SelectedIndexChanged += new System.EventHandler(this.classIDDropdown_SelectedIndexChanged);
            // 
            // configNameBox
            // 
            this.configNameBox.Location = new System.Drawing.Point(24, 40);
            this.configNameBox.Name = "configNameBox";
            this.configNameBox.Size = new System.Drawing.Size(188, 22);
            this.configNameBox.TabIndex = 1;
            // 
            // configNameLabel
            // 
            this.configNameLabel.AutoSize = true;
            this.configNameLabel.Location = new System.Drawing.Point(21, 21);
            this.configNameLabel.Name = "configNameLabel";
            this.configNameLabel.Size = new System.Drawing.Size(85, 16);
            this.configNameLabel.TabIndex = 2;
            this.configNameLabel.Text = "Config Name";
            // 
            // materialIndexLabel
            // 
            this.materialIndexLabel.AutoSize = true;
            this.materialIndexLabel.Location = new System.Drawing.Point(21, 65);
            this.materialIndexLabel.Name = "materialIndexLabel";
            this.materialIndexLabel.Size = new System.Drawing.Size(90, 16);
            this.materialIndexLabel.TabIndex = 4;
            this.materialIndexLabel.Text = "Material Index";
            // 
            // materialBox
            // 
            this.materialBox.Location = new System.Drawing.Point(24, 84);
            this.materialBox.Name = "materialBox";
            this.materialBox.Size = new System.Drawing.Size(121, 22);
            this.materialBox.TabIndex = 3;
            this.materialBox.TextChanged += new System.EventHandler(this.materialBox_TextChanged);
            // 
            // textureFileNameLabel
            // 
            this.textureFileNameLabel.AutoSize = true;
            this.textureFileNameLabel.Location = new System.Drawing.Point(21, 109);
            this.textureFileNameLabel.Name = "textureFileNameLabel";
            this.textureFileNameLabel.Size = new System.Drawing.Size(117, 16);
            this.textureFileNameLabel.TabIndex = 6;
            this.textureFileNameLabel.Text = "Texture File Name";
            // 
            // texFileNameBox
            // 
            this.texFileNameBox.Location = new System.Drawing.Point(24, 128);
            this.texFileNameBox.Name = "texFileNameBox";
            this.texFileNameBox.Size = new System.Drawing.Size(171, 22);
            this.texFileNameBox.TabIndex = 5;
            // 
            // classIDLabel
            // 
            this.classIDLabel.AutoSize = true;
            this.classIDLabel.Location = new System.Drawing.Point(21, 153);
            this.classIDLabel.Name = "classIDLabel";
            this.classIDLabel.Size = new System.Drawing.Size(57, 16);
            this.classIDLabel.TabIndex = 7;
            this.classIDLabel.Text = "Class ID";
            // 
            // hierarchyPathLabel
            // 
            this.hierarchyPathLabel.AutoSize = true;
            this.hierarchyPathLabel.Location = new System.Drawing.Point(21, 199);
            this.hierarchyPathLabel.Name = "hierarchyPathLabel";
            this.hierarchyPathLabel.Size = new System.Drawing.Size(155, 16);
            this.hierarchyPathLabel.TabIndex = 8;
            this.hierarchyPathLabel.Text = "Renderer Hierarchy Path";
            // 
            // hierarchyPathBox
            // 
            this.hierarchyPathBox.Location = new System.Drawing.Point(24, 218);
            this.hierarchyPathBox.Name = "hierarchyPathBox";
            this.hierarchyPathBox.Size = new System.Drawing.Size(280, 22);
            this.hierarchyPathBox.TabIndex = 9;
            // 
            // textureNameLabel
            // 
            this.textureNameLabel.AutoSize = true;
            this.textureNameLabel.Location = new System.Drawing.Point(21, 247);
            this.textureNameLabel.Name = "textureNameLabel";
            this.textureNameLabel.Size = new System.Drawing.Size(92, 16);
            this.textureNameLabel.TabIndex = 10;
            this.textureNameLabel.Text = "Texture Name";
            // 
            // textureNameBox
            // 
            this.textureNameBox.Location = new System.Drawing.Point(24, 266);
            this.textureNameBox.Name = "textureNameBox";
            this.textureNameBox.Size = new System.Drawing.Size(121, 22);
            this.textureNameBox.TabIndex = 11;
            // 
            // variationToggle
            // 
            this.variationToggle.AutoSize = true;
            this.variationToggle.Location = new System.Drawing.Point(24, 303);
            this.variationToggle.Name = "variationToggle";
            this.variationToggle.Size = new System.Drawing.Size(95, 20);
            this.variationToggle.TabIndex = 12;
            this.variationToggle.Text = "Is Variation";
            this.variationToggle.UseVisualStyleBackColor = true;
            this.variationToggle.CheckedChanged += new System.EventHandler(this.variationToggle_CheckedChanged);
            // 
            // variationChanceBox
            // 
            this.variationChanceBox.Location = new System.Drawing.Point(24, 349);
            this.variationChanceBox.Name = "variationChanceBox";
            this.variationChanceBox.Size = new System.Drawing.Size(82, 22);
            this.variationChanceBox.TabIndex = 13;
            this.variationChanceBox.TextChanged += new System.EventHandler(this.variationChanceBox_TextChanged);
            // 
            // variationChanceLabel
            // 
            this.variationChanceLabel.AutoSize = true;
            this.variationChanceLabel.Location = new System.Drawing.Point(21, 330);
            this.variationChanceLabel.Name = "variationChanceLabel";
            this.variationChanceLabel.Size = new System.Drawing.Size(144, 16);
            this.variationChanceLabel.TabIndex = 14;
            this.variationChanceLabel.Text = "Variation Chance (0 - 1)";
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Gray;
            this.glControl1.Location = new System.Drawing.Point(601, 21);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(640, 640);
            this.glControl1.TabIndex = 15;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // loadFolderBtn
            // 
            this.loadFolderBtn.Location = new System.Drawing.Point(12, 548);
            this.loadFolderBtn.Name = "loadFolderBtn";
            this.loadFolderBtn.Size = new System.Drawing.Size(222, 40);
            this.loadFolderBtn.TabIndex = 16;
            this.loadFolderBtn.Text = "Load Assets";
            this.loadFolderBtn.UseVisualStyleBackColor = true;
            this.loadFolderBtn.Click += new System.EventHandler(this.loadFolderBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 620);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(582, 38);
            this.progressBar1.TabIndex = 17;
            // 
            // loadingLabel
            // 
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.loadingLabel.Location = new System.Drawing.Point(7, 591);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(450, 26);
            this.loadingLabel.TabIndex = 18;
            this.loadingLabel.Text = "Loading files... This will take a while (10.55%)";
            this.loadingLabel.Visible = false;
            // 
            // previewLabel
            // 
            this.previewLabel.AutoSize = true;
            this.previewLabel.Location = new System.Drawing.Point(448, 199);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(88, 16);
            this.previewLabel.TabIndex = 19;
            this.previewLabel.Text = "Can\'t Preview";
            this.previewLabel.Visible = false;
            // 
            // ExportCfgBtn
            // 
            this.ExportCfgBtn.Location = new System.Drawing.Point(12, 502);
            this.ExportCfgBtn.Name = "ExportCfgBtn";
            this.ExportCfgBtn.Size = new System.Drawing.Size(222, 40);
            this.ExportCfgBtn.TabIndex = 20;
            this.ExportCfgBtn.Text = "Export Config(s)";
            this.ExportCfgBtn.UseVisualStyleBackColor = true;
            this.ExportCfgBtn.Click += new System.EventHandler(this.ExportCfgBtn_Click);
            // 
            // SaveCfgBtn
            // 
            this.SaveCfgBtn.Location = new System.Drawing.Point(12, 456);
            this.SaveCfgBtn.Name = "SaveCfgBtn";
            this.SaveCfgBtn.Size = new System.Drawing.Size(222, 40);
            this.SaveCfgBtn.TabIndex = 21;
            this.SaveCfgBtn.Text = "Save Config";
            this.SaveCfgBtn.UseVisualStyleBackColor = true;
            this.SaveCfgBtn.Click += new System.EventHandler(this.SaveCfgBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 670);
            this.Controls.Add(this.SaveCfgBtn);
            this.Controls.Add(this.ExportCfgBtn);
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.loadFolderBtn);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.variationChanceLabel);
            this.Controls.Add(this.variationChanceBox);
            this.Controls.Add(this.variationToggle);
            this.Controls.Add(this.textureNameBox);
            this.Controls.Add(this.textureNameLabel);
            this.Controls.Add(this.hierarchyPathBox);
            this.Controls.Add(this.hierarchyPathLabel);
            this.Controls.Add(this.classIDLabel);
            this.Controls.Add(this.textureFileNameLabel);
            this.Controls.Add(this.texFileNameBox);
            this.Controls.Add(this.materialIndexLabel);
            this.Controls.Add(this.materialBox);
            this.Controls.Add(this.configNameLabel);
            this.Controls.Add(this.configNameBox);
            this.Controls.Add(this.classIDDropdown);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox classIDDropdown;
        private System.Windows.Forms.TextBox configNameBox;
        private System.Windows.Forms.Label configNameLabel;
        private System.Windows.Forms.Label materialIndexLabel;
        private System.Windows.Forms.TextBox materialBox;
        private System.Windows.Forms.Label textureFileNameLabel;
        private System.Windows.Forms.TextBox texFileNameBox;
        private System.Windows.Forms.Label classIDLabel;
        private System.Windows.Forms.Label hierarchyPathLabel;
        private System.Windows.Forms.TextBox hierarchyPathBox;
        private System.Windows.Forms.Label textureNameLabel;
        private System.Windows.Forms.TextBox textureNameBox;
        private System.Windows.Forms.CheckBox variationToggle;
        private System.Windows.Forms.TextBox variationChanceBox;
        private System.Windows.Forms.Label variationChanceLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button loadFolderBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.Label previewLabel;
        private System.Windows.Forms.Button ExportCfgBtn;
        private System.Windows.Forms.Button SaveCfgBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

