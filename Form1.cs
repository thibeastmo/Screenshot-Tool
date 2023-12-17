using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Galaxy_Life_Tool
{
    //notify user when location == null && name == null
    //houd rekening met uploaden van discord
    //overlay waarbij je aanduid welke solar systems gescreenshot zijn
    public partial class GUI : Form
    {
        #region Form bewegen
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void GUI_MouseDown(object sender, MouseEventArgs e)
        { //Beweeg hele form
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                form2.Location = this.Location;
                form2.Size = this.Size;
            }
        }
        #endregion
        #region Ronde hoeken form

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        public GUI()
        {
            try
            {
                StoredData.Initialize();
                var defaultSavesFolder = FormHandler.GetCorrectPath("./Saves");
                if (Directory.Exists(defaultSavesFolder))
                {
                    Directory.CreateDirectory(defaultSavesFolder);
                }
                InitializeComponent();
                HandleLayout();
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
                form2.Region = this.Region;
                handler = new FormHandler(this, rtbOuput, pnlProgressBars, cbUploadDiscord, cbUsingFlashBrowser, cbUsingSteamApplication, lblAmountSaved);
                SetlblAmountSavedLocation();
                txtSaveFolder.Text = FormHandler.GetCorrectPath(StoredData.data.SaveFolder);
                handler.KeyHookSaved += Handler_KeyHookSaved;
                handler.MouseButtonHookSaved += Handler_MouseButtonHookSaved;
                HandleEnableBtnEnableSavingGalaxyWindows();
            }
            catch (Exception ex){
                var example = FormHandler.GetCorrectPath("./Saves");
                MessageBox.Show("Message:\n" + example + "\n\n" + ex.Message + Environment.NewLine + Environment.NewLine + "Stacktrace:\n" + ex.StackTrace);
            }
        }
        #endregion

        private FormHandler handler;

        private async void btnStartGL_Click(object sender, EventArgs e)
        {
            await handler.StartGalaxyLife();
        }


        private void rtbOuput_TextChanged(object sender, EventArgs e)
        {
            ((RichTextBox)sender).SelectionStart = ((RichTextBox)sender).Text.Length;
            ((RichTextBox)sender)?.ScrollToCaret();
        }

        private void pbShutdown_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            this.Close();
        }

        #region Change save folder
        private void HandleSaveFolderLocationChanging()
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSaveFolder.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void txtSaveFolder_DoubleClick(object sender, EventArgs e)
        {
            PlayButtonSound();
            HandleSaveFolderLocationChanging();
        }

        private void btnChangeSaveFolder_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            HandleSaveFolderLocationChanging();
        }
        #endregion

        private void txtSaveFolder_TextChanged(object sender, EventArgs e)
        {
            StoredData.data.SaveFolder = txtSaveFolder.Text;
        }

        private async void btnOpenSaveFolder_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            await handler.OpenSaveFolder();
        }

        #region Discord bot
        private void cbUploadDiscord_CheckedChanged(object sender, EventArgs e)
        {
            handler.StartDiscordBot();
        }

        private async void btnUploadSavedImages_Click(object sender, EventArgs e)
        {
            SoundHandler.Play(SoundHandler.Sounds.Thicking);
            handler.StartDiscordBot();
            await handler.StartUploadingImages();
        }
        #endregion

        private void btnEnableSavingGalaxyWindows_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            if (handler.IsSubscribedToAHook())
            {
                handler.UnsubscribeHook();
                (sender as Button).Text = "Enable saving galaxy windows";
            }
            else
            {
                handler.SubscribeHook();
                (sender as Button).Text = "Disable saving galaxy windows";
            }
        }

        private bool waitingForKeyOrMouseButtonHook;
        private Panel overlayPanel;
        private void btnKeyMouseRegisterer_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            overlayPanel = new Panel();
            overlayPanel.BackColor = Color.FromArgb(100, Color.Black);
            overlayPanel.Width = this.Width;
            overlayPanel.Height = this.Height;
            var lbl = new Label();
            lbl.Text = "Press any button to save it as the key to save galaxy windows with.";
            Size size = TextRenderer.MeasureText(lbl.Text, lbl.Font);
            lbl.Width = size.Width;
            lbl.Height = size.Height;
            lbl.Location = new Point(overlayPanel.Width/2- lbl.Width/2, overlayPanel.Height/2- lbl.Height/2);
            lbl.BackColor = Color.Gray;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Enabled = false;
            }
            overlayPanel.Controls.Add(lbl);
            this.Controls.Add(overlayPanel);
            waitingForKeyOrMouseButtonHook = true;
            handler.ResetHook();
            handler.SubscribeHook(forceSubscribe: true);
        }
        private void Handler_MouseButtonHookSaved(object sender, EventArgs e)
        {
            HandleNewHookSaved((int)Enum.Parse(typeof(MouseButtons), sender.ToString()));
        }

        private void Handler_KeyHookSaved(object sender, EventArgs e)
        {
            HandleNewHookSaved((char)sender);
        }
        private void HandleNewHookSaved(int hook)
        {
            if (waitingForKeyOrMouseButtonHook)
            {
                this.Controls.Remove(overlayPanel);
                overlayPanel.Dispose();


                for (int i = 0; i < this.Controls.Count; i++)
                {
                    this.Controls[i].Enabled = true;
                }

                StoredData.data.Hook = hook;
                SetBtnKeyMouseRegistererText();
                waitingForKeyOrMouseButtonHook = false;
                HandleEnableBtnEnableSavingGalaxyWindows();
            }
        }
        private void HandleEnableBtnEnableSavingGalaxyWindows()
        {
            btnEnableSavingGalaxyWindows.Enabled = StoredData.data.Hook != -1;
            if (btnEnableSavingGalaxyWindows.Enabled)
            {
                SetBtnKeyMouseRegistererText();
            }
        }

        private void SetBtnKeyMouseRegistererText()
        {
            if (StoredData.data.Hook < ((int)MouseButtons.Left))
            {
                //key
                btnKeyMouseRegisterer.Text = "Key to save a galaxy window:\n" + (char)StoredData.data.Hook;
            }
            else
            {
                //mouse
                btnKeyMouseRegisterer.Text = "Mouse button to save a galaxy window:\n" + MouseButtons.GetName(typeof(MouseButtons), StoredData.data.Hook);
            }
        }

        private async void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            await handler.Close();
            form2.Close();
            StoredData.Save();
            Application.Exit();
        }

        private Form form2;
        private void HandleLayout()
        {
            form2 = new Form();
            form2.Location = this.Location;
            form2.Size = this.Size;
            form2.BackColor = Color.FromArgb(255, 11, 40, 40);
            form2.Opacity = 0.80;
            form2.FormBorderStyle = this.FormBorderStyle;
            this.ShowInTaskbar = false;
            form2.Activated += Form2_Activated;
            form2.MouseDown += GUI_MouseDown;
            this.BackColor = form2.BackColor;
            this.TransparencyKey = form2.BackColor;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                var control = this.Controls[i];
                if (control is Button || (control is PictureBox && control.Name != "logo" ) || control is CheckBox)
                {
                    control.Cursor = Cursors.Hand;
                    if (control is Button)
                    {
                        control.MouseEnter += Control_MouseEnter;
                        control.MouseLeave += Control_MouseLeave;
                        control.MouseDown += Control_MouseDown;
                        control.TabStop = false;
                        control.BackgroundImage = new Bitmap(control.Width, control.Height).CreateGLButton();
                        (control as Button).FlatAppearance.MouseOverBackColor = Color.Transparent;
                        (control as Button).FlatAppearance.MouseDownBackColor = Color.Transparent;
                        (control as Button).FlatAppearance.CheckedBackColor = Color.Transparent;
                        (control as Button).FlatAppearance.BorderSize = 0;
                        (control as Button).FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                        (control as Button).BackColor = Color.FromArgb(0, 255, 255, 255);
                    }
                    else if (control is PictureBox)
                    {
                        control.BackgroundImage = new Bitmap(control.Width, control.Height).CreateGLRoundButton();
                        control.MouseEnter += CircleButton_MouseEnter;
                        control.MouseLeave += CircleButton_MouseLeave;
                        control.MouseDown += CircleButton_MouseDown;
                    }
                    else if (control is CheckBox)
                    {
                        if ((control as CheckBox).Checked)
                        {
                            control.BackgroundImage = new Bitmap(control.Width, control.Height).CreateGLCheckboxChecked();
                        }
                        else
                        {
                            control.BackgroundImage = new Bitmap(control.Width, control.Height).CreateGLCheckbox();
                        }
                        (control as CheckBox).CheckedChanged += GUI_CheckedChanged;
                        control.Text = "   " + control.Text;
                        (control as CheckBox).FlatAppearance.MouseOverBackColor = Color.Transparent;
                        (control as CheckBox).FlatAppearance.MouseDownBackColor = Color.Transparent;
                        (control as CheckBox).FlatAppearance.CheckedBackColor = Color.Transparent;
                        (control as CheckBox).FlatAppearance.BorderSize = 0;
                        (control as CheckBox).FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                        (control as CheckBox).Appearance = Appearance.Button;
                    }
                }
                else if (control is Label)
                {
                    (control as Label).BackgroundImage = new Bitmap(control.Width, control.Height).CreateGLTextbox();
                    (control as Label).BackColor = Color.Transparent;
                    (control as Label).TextChanged += GUI_TextChanged;
                }
            }
            form2.Show();
        }

        private void GUI_TextChanged(object sender, EventArgs e)
        {
            Label control = (sender as Label);
            using (Graphics g = control.CreateGraphics())
            {
                control.Size = g.MeasureString(control.Text, control.Font).ToSize();
                control.Size = new Size(control.Size.Width + 10, control.Size.Height + 10);
            }
            control.BackgroundImage = new Bitmap((sender as Label).Width, control.Height).CreateGLTextbox();
            control.Location = new Point(control.MaximumSize.Width/2- control.Width/2, control.Location.Y);
        }

        private void GUI_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (sender as CheckBox);
            if (cb.Checked)
            {
                cb.BackgroundImage = new Bitmap(cb.Width, cb.Height).CreateGLCheckboxChecked();
            }
            else
            {
                cb.BackgroundImage = new Bitmap(cb.Width, cb.Height).CreateGLCheckbox();
            }
        }

        private void CircleButton_MouseEnter(object sender, EventArgs e)
        {
            var btn = (sender as PictureBox);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLHoverRoundButton();
        }
        private void CircleButton_MouseLeave(object sender, EventArgs e)
        {
            var btn = (sender as PictureBox);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLRoundButton();
        }
        private void CircleButton_MouseDown(object sender, EventArgs e)
        {
            var btn = (sender as PictureBox);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLHoverRoundButton();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void GUI_LocationChanged(object sender, EventArgs e)
        {
            form2.Location = this.Location;
        }

        private void GUI_SizeChanged(object sender, EventArgs e)
        {
            form2.Size = this.Size;
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLButton();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLHoverButton();
        }
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (sender as Button);
            btn.BackgroundImage = new Bitmap(btn.Width, btn.Height).CreateGLHoverButton();
        }

        #region altijd op voorgrond NOG NIET TOEGAPST
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private void voorgrondInit()
        {
            this.BackColor = Color.Lime;
            TransparencyKey = this.BackColor;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
        #endregion

        private void cbUsingFlashBrowser_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                cbUsingSteamApplication.Checked = false;
                StoredData.data.ApplicationType = (int)StoredData.ApplicationType.Flash;
            }
        }

        private void cbUsingSteamApplication_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                cbUsingFlashBrowser.Checked = false;
                StoredData.data.ApplicationType = (int)StoredData.ApplicationType.Steam;
            }
        }

        private void PlayButtonSound()
        {
            SoundHandler.Play(SoundHandler.Sounds.Click);
        }

        private void lblAmountSaved_SizeChanged(object sender, EventArgs e)
        {
            SetlblAmountSavedLocation();
        }
        public void SetlblAmountSavedLocation()
        {
            int width = btnOpenSaveFolder.Location.X + btnOpenSaveFolder.Width - btnChangeSaveFolder.Location.X;
            lblAmountSaved.Location = new Point(btnChangeSaveFolder.Location.X + width / 2 - lblAmountSaved.Width / 2, lblAmountSaved.Location.Y);
        }
    }
}