
namespace Galaxy_Life_Tool
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.pbShutdown = new System.Windows.Forms.PictureBox();
            this.rtbOuput = new System.Windows.Forms.RichTextBox();
            this.btnStartGL = new System.Windows.Forms.Button();
            this.cbUploadDiscord = new System.Windows.Forms.CheckBox();
            this.txtSaveFolder = new System.Windows.Forms.Label();
            this.btnChangeSaveFolder = new System.Windows.Forms.Button();
            this.btnOpenSaveFolder = new System.Windows.Forms.Button();
            this.btnUploadSavedImages = new System.Windows.Forms.Button();
            this.btnEnableSavingGalaxyWindows = new System.Windows.Forms.Button();
            this.pnlProgressBars = new System.Windows.Forms.Panel();
            this.btnKeyMouseRegisterer = new System.Windows.Forms.Button();
            this.logo = new System.Windows.Forms.PictureBox();
            this.cbUsingSteamApplication = new System.Windows.Forms.CheckBox();
            this.cbUsingFlashBrowser = new System.Windows.Forms.CheckBox();
            this.lblAmountSaved = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbShutdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbShutdown
            // 
            this.pbShutdown.BackColor = System.Drawing.Color.Transparent;
            this.pbShutdown.Image = ((System.Drawing.Image)(resources.GetObject("pbShutdown.Image")));
            this.pbShutdown.Location = new System.Drawing.Point(448, 12);
            this.pbShutdown.Name = "pbShutdown";
            this.pbShutdown.Size = new System.Drawing.Size(60, 60);
            this.pbShutdown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbShutdown.TabIndex = 12;
            this.pbShutdown.TabStop = false;
            this.pbShutdown.Click += new System.EventHandler(this.pbShutdown_Click);
            // 
            // rtbOuput
            // 
            this.rtbOuput.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rtbOuput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbOuput.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOuput.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.rtbOuput.Location = new System.Drawing.Point(8, 387);
            this.rtbOuput.Name = "rtbOuput";
            this.rtbOuput.ReadOnly = true;
            this.rtbOuput.Size = new System.Drawing.Size(498, 236);
            this.rtbOuput.TabIndex = 17;
            this.rtbOuput.Text = "";
            this.rtbOuput.TextChanged += new System.EventHandler(this.rtbOuput_TextChanged);
            // 
            // btnStartGL
            // 
            this.btnStartGL.BackColor = System.Drawing.Color.Lime;
            this.btnStartGL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStartGL.FlatAppearance.BorderSize = 0;
            this.btnStartGL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartGL.Font = new System.Drawing.Font("font_esparragon", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartGL.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnStartGL.Location = new System.Drawing.Point(433, 80);
            this.btnStartGL.Name = "btnStartGL";
            this.btnStartGL.Size = new System.Drawing.Size(75, 47);
            this.btnStartGL.TabIndex = 0;
            this.btnStartGL.Text = "Start GL";
            this.btnStartGL.UseVisualStyleBackColor = false;
            this.btnStartGL.Click += new System.EventHandler(this.btnStartGL_Click);
            // 
            // cbUploadDiscord
            // 
            this.cbUploadDiscord.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbUploadDiscord.AutoSize = true;
            this.cbUploadDiscord.BackColor = System.Drawing.Color.Transparent;
            this.cbUploadDiscord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cbUploadDiscord.FlatAppearance.BorderSize = 0;
            this.cbUploadDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUploadDiscord.Font = new System.Drawing.Font("font_esparragon", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUploadDiscord.ForeColor = System.Drawing.Color.Gainsboro;
            this.cbUploadDiscord.Location = new System.Drawing.Point(3, 206);
            this.cbUploadDiscord.Name = "cbUploadDiscord";
            this.cbUploadDiscord.Size = new System.Drawing.Size(238, 26);
            this.cbUploadDiscord.TabIndex = 1;
            this.cbUploadDiscord.Text = "  Upload to discord instantly";
            this.cbUploadDiscord.UseVisualStyleBackColor = false;
            this.cbUploadDiscord.CheckedChanged += new System.EventHandler(this.cbUploadDiscord_CheckedChanged);
            // 
            // txtSaveFolder
            // 
            this.txtSaveFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.txtSaveFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtSaveFolder.Font = new System.Drawing.Font("font_esparragon", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaveFolder.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtSaveFolder.Location = new System.Drawing.Point(9, 351);
            this.txtSaveFolder.MaximumSize = new System.Drawing.Size(499, 24);
            this.txtSaveFolder.Name = "txtSaveFolder";
            this.txtSaveFolder.Size = new System.Drawing.Size(499, 24);
            this.txtSaveFolder.TabIndex = 18;
            this.txtSaveFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSaveFolder.TextChanged += new System.EventHandler(this.txtSaveFolder_TextChanged);
            this.txtSaveFolder.DoubleClick += new System.EventHandler(this.txtSaveFolder_DoubleClick);
            // 
            // btnChangeSaveFolder
            // 
            this.btnChangeSaveFolder.BackColor = System.Drawing.Color.Lime;
            this.btnChangeSaveFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChangeSaveFolder.FlatAppearance.BorderSize = 0;
            this.btnChangeSaveFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeSaveFolder.Font = new System.Drawing.Font("font_esparragon", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSaveFolder.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnChangeSaveFolder.Location = new System.Drawing.Point(247, 224);
            this.btnChangeSaveFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeSaveFolder.Name = "btnChangeSaveFolder";
            this.btnChangeSaveFolder.Size = new System.Drawing.Size(124, 47);
            this.btnChangeSaveFolder.TabIndex = 19;
            this.btnChangeSaveFolder.Text = "Change";
            this.btnChangeSaveFolder.UseVisualStyleBackColor = false;
            this.btnChangeSaveFolder.Click += new System.EventHandler(this.btnChangeSaveFolder_Click);
            // 
            // btnOpenSaveFolder
            // 
            this.btnOpenSaveFolder.BackColor = System.Drawing.Color.Lime;
            this.btnOpenSaveFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOpenSaveFolder.FlatAppearance.BorderSize = 0;
            this.btnOpenSaveFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSaveFolder.Font = new System.Drawing.Font("font_esparragon", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSaveFolder.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnOpenSaveFolder.Location = new System.Drawing.Point(374, 224);
            this.btnOpenSaveFolder.Name = "btnOpenSaveFolder";
            this.btnOpenSaveFolder.Size = new System.Drawing.Size(134, 47);
            this.btnOpenSaveFolder.TabIndex = 20;
            this.btnOpenSaveFolder.Text = "Open folder";
            this.btnOpenSaveFolder.UseVisualStyleBackColor = false;
            this.btnOpenSaveFolder.Click += new System.EventHandler(this.btnOpenSaveFolder_Click);
            // 
            // btnUploadSavedImages
            // 
            this.btnUploadSavedImages.BackColor = System.Drawing.Color.Lime;
            this.btnUploadSavedImages.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUploadSavedImages.FlatAppearance.BorderSize = 0;
            this.btnUploadSavedImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadSavedImages.Font = new System.Drawing.Font("font_esparragon", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadSavedImages.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnUploadSavedImages.Location = new System.Drawing.Point(247, 23);
            this.btnUploadSavedImages.Name = "btnUploadSavedImages";
            this.btnUploadSavedImages.Size = new System.Drawing.Size(155, 49);
            this.btnUploadSavedImages.TabIndex = 21;
            this.btnUploadSavedImages.Text = "Upload saved images";
            this.btnUploadSavedImages.UseVisualStyleBackColor = false;
            this.btnUploadSavedImages.Click += new System.EventHandler(this.btnUploadSavedImages_Click);
            // 
            // btnEnableSavingGalaxyWindows
            // 
            this.btnEnableSavingGalaxyWindows.BackColor = System.Drawing.Color.Lime;
            this.btnEnableSavingGalaxyWindows.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEnableSavingGalaxyWindows.FlatAppearance.BorderSize = 0;
            this.btnEnableSavingGalaxyWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableSavingGalaxyWindows.Font = new System.Drawing.Font("font_esparragon", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnableSavingGalaxyWindows.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnEnableSavingGalaxyWindows.Location = new System.Drawing.Point(247, 80);
            this.btnEnableSavingGalaxyWindows.Name = "btnEnableSavingGalaxyWindows";
            this.btnEnableSavingGalaxyWindows.Size = new System.Drawing.Size(180, 47);
            this.btnEnableSavingGalaxyWindows.TabIndex = 22;
            this.btnEnableSavingGalaxyWindows.Text = "Enable saving galaxy windows";
            this.btnEnableSavingGalaxyWindows.UseVisualStyleBackColor = false;
            this.btnEnableSavingGalaxyWindows.Click += new System.EventHandler(this.btnEnableSavingGalaxyWindows_Click);
            // 
            // pnlProgressBars
            // 
            this.pnlProgressBars.Location = new System.Drawing.Point(8, 238);
            this.pnlProgressBars.Name = "pnlProgressBars";
            this.pnlProgressBars.Size = new System.Drawing.Size(227, 110);
            this.pnlProgressBars.TabIndex = 24;
            // 
            // btnKeyMouseRegisterer
            // 
            this.btnKeyMouseRegisterer.BackColor = System.Drawing.Color.Lime;
            this.btnKeyMouseRegisterer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnKeyMouseRegisterer.FlatAppearance.BorderSize = 0;
            this.btnKeyMouseRegisterer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyMouseRegisterer.Font = new System.Drawing.Font("font_esparragon", 12F);
            this.btnKeyMouseRegisterer.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnKeyMouseRegisterer.Location = new System.Drawing.Point(247, 132);
            this.btnKeyMouseRegisterer.Name = "btnKeyMouseRegisterer";
            this.btnKeyMouseRegisterer.Size = new System.Drawing.Size(261, 86);
            this.btnKeyMouseRegisterer.TabIndex = 25;
            this.btnKeyMouseRegisterer.Text = "Select a button to save galaxy windows with";
            this.btnKeyMouseRegisterer.UseVisualStyleBackColor = false;
            this.btnKeyMouseRegisterer.Click += new System.EventHandler(this.btnKeyMouseRegisterer_Click);
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.InitialImage = null;
            this.logo.Location = new System.Drawing.Point(12, 12);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(100, 100);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logo.TabIndex = 26;
            this.logo.TabStop = false;
            // 
            // cbUsingSteamApplication
            // 
            this.cbUsingSteamApplication.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbUsingSteamApplication.AutoSize = true;
            this.cbUsingSteamApplication.BackColor = System.Drawing.Color.Transparent;
            this.cbUsingSteamApplication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cbUsingSteamApplication.FlatAppearance.BorderSize = 0;
            this.cbUsingSteamApplication.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUsingSteamApplication.Font = new System.Drawing.Font("font_esparragon", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsingSteamApplication.ForeColor = System.Drawing.Color.Gainsboro;
            this.cbUsingSteamApplication.Location = new System.Drawing.Point(12, 118);
            this.cbUsingSteamApplication.Name = "cbUsingSteamApplication";
            this.cbUsingSteamApplication.Size = new System.Drawing.Size(208, 26);
            this.cbUsingSteamApplication.TabIndex = 27;
            this.cbUsingSteamApplication.Text = "  Using steam application";
            this.cbUsingSteamApplication.UseVisualStyleBackColor = false;
            this.cbUsingSteamApplication.CheckedChanged += new System.EventHandler(this.cbUsingSteamApplication_CheckedChanged);
            // 
            // cbUsingFlashBrowser
            // 
            this.cbUsingFlashBrowser.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbUsingFlashBrowser.AutoSize = true;
            this.cbUsingFlashBrowser.BackColor = System.Drawing.Color.Transparent;
            this.cbUsingFlashBrowser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cbUsingFlashBrowser.FlatAppearance.BorderSize = 0;
            this.cbUsingFlashBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUsingFlashBrowser.Font = new System.Drawing.Font("font_esparragon", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsingFlashBrowser.ForeColor = System.Drawing.Color.Gainsboro;
            this.cbUsingFlashBrowser.Location = new System.Drawing.Point(12, 150);
            this.cbUsingFlashBrowser.Name = "cbUsingFlashBrowser";
            this.cbUsingFlashBrowser.Size = new System.Drawing.Size(184, 26);
            this.cbUsingFlashBrowser.TabIndex = 28;
            this.cbUsingFlashBrowser.Text = "  Using flash browser";
            this.cbUsingFlashBrowser.UseVisualStyleBackColor = false;
            this.cbUsingFlashBrowser.CheckedChanged += new System.EventHandler(this.cbUsingFlashBrowser_CheckedChanged);
            // 
            // lblAmountSaved
            // 
            this.lblAmountSaved.AutoSize = true;
            this.lblAmountSaved.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblAmountSaved.Font = new System.Drawing.Font("font_esparragon", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountSaved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.lblAmountSaved.Location = new System.Drawing.Point(315, 299);
            this.lblAmountSaved.Name = "lblAmountSaved";
            this.lblAmountSaved.Size = new System.Drawing.Size(118, 32);
            this.lblAmountSaved.TabIndex = 29;
            this.lblAmountSaved.Text = "label1";
            this.lblAmountSaved.SizeChanged += new System.EventHandler(this.lblAmountSaved_SizeChanged);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(520, 635);
            this.Controls.Add(this.lblAmountSaved);
            this.Controls.Add(this.cbUsingFlashBrowser);
            this.Controls.Add(this.cbUsingSteamApplication);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.btnOpenSaveFolder);
            this.Controls.Add(this.btnKeyMouseRegisterer);
            this.Controls.Add(this.pnlProgressBars);
            this.Controls.Add(this.btnEnableSavingGalaxyWindows);
            this.Controls.Add(this.btnUploadSavedImages);
            this.Controls.Add(this.btnChangeSaveFolder);
            this.Controls.Add(this.txtSaveFolder);
            this.Controls.Add(this.cbUploadDiscord);
            this.Controls.Add(this.btnStartGL);
            this.Controls.Add(this.rtbOuput);
            this.Controls.Add(this.pbShutdown);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Galaxy Life Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.LocationChanged += new System.EventHandler(this.GUI_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.GUI_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GUI_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbShutdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbShutdown;
        private System.Windows.Forms.RichTextBox rtbOuput;
        private System.Windows.Forms.Button btnStartGL;
        private System.Windows.Forms.CheckBox cbUploadDiscord;
        private System.Windows.Forms.Label txtSaveFolder;
        private System.Windows.Forms.Button btnChangeSaveFolder;
        private System.Windows.Forms.Button btnOpenSaveFolder;
        private System.Windows.Forms.Button btnUploadSavedImages;
        private System.Windows.Forms.Button btnEnableSavingGalaxyWindows;
        private System.Windows.Forms.Panel pnlProgressBars;
        private System.Windows.Forms.Button btnKeyMouseRegisterer;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.CheckBox cbUsingSteamApplication;
        private System.Windows.Forms.CheckBox cbUsingFlashBrowser;
        private System.Windows.Forms.Label lblAmountSaved;
    }
}

