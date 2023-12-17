using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Life_Tool {
    public class FormHandler {
        private readonly OutputHandler _outputHandler;
        private readonly WindowCapturer _windowCapturer;
        private readonly MouseKeyHookHandler _mouseKeyHookHandler;
        private Bot _bot;
        private readonly GUI _gui;
        private readonly Panel _progressBarsPanel;
        private readonly CheckBox _cbUploadDiscord;
        private readonly CheckBox _cbUsingFlashBrowser;
        private readonly CheckBox _cbUsingSteamApplication;
        private readonly Label _lblAmountSaved;
        private ProgressBar _backgroundWorkerProgressBar;
        private readonly BackgroundWorker _backgroundWorker;

        public event EventHandler KeyHookSaved;
        public event EventHandler MouseButtonHookSaved;
        public FormHandler(Form gui, RichTextBox rtbOuput, Panel pnlProgressBars, CheckBox cbUploadDiscord, CheckBox cbUsingFlashBrowser, CheckBox cbUsingSteamApplication, Label lblAmountSaved)
        {
            SoundHandler.Play(SoundHandler.Sounds.LevelUp);
            _gui = (GUI)gui;
            this._backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            this._progressBarsPanel = pnlProgressBars;
            this._cbUploadDiscord = cbUploadDiscord;
            _cbUsingFlashBrowser = cbUsingFlashBrowser;
            _cbUsingSteamApplication = cbUsingSteamApplication;
            _lblAmountSaved = lblAmountSaved;
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            UpdateProgressBarsPanelHeight();
            _outputHandler = new OutputHandler(gui, rtbOuput);
            SetAmountSavedLabelText();
            _windowCapturer = new WindowCapturer();
            //WindowCapturer.InitializePlanetsLetterColors();
            _mouseKeyHookHandler = new MouseKeyHookHandler(StoredData.data.Hook);
            _mouseKeyHookHandler.HookEvent += MouseKeyHookHandler_HookEvent;
            _mouseKeyHookHandler.KeyHookSaved += MouseKeyHookHandler_KeyHookSaved;
            _mouseKeyHookHandler.MouseButtonHookSaved += MouseKeyHookHandler_MouseButtonHookSaved;
            switch ((StoredData.ApplicationType)StoredData.data.ApplicationType){
                case StoredData.ApplicationType.Steam:
                    cbUsingSteamApplication.Checked = true;
                    break;
                case StoredData.ApplicationType.Flash:
                    cbUsingFlashBrowser.Checked = true;
                    break;
                default:
                    cbUsingSteamApplication.Checked = true;
                    break;
            }
        }
        internal async Task Close()
        {
            if (_bot != null){
                await _bot.Stop();
            }
            UnsubscribeHook();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _backgroundWorkerProgressBar.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100){
                RemoveProgressBar(_backgroundWorkerProgressBar);
            }
        }

        private void MouseKeyHookHandler_MouseButtonHookSaved(object sender, EventArgs e)
        {
            MouseButtonHookSaved?.Invoke(sender, e);
            _outputHandler.LogInfo("Will now save galaxy windows when the " + (Enum.Parse(typeof(MouseButtons), sender.ToString())) + " mouse button is pressed.");
        }

        private void MouseKeyHookHandler_KeyHookSaved(object sender, EventArgs e)
        {
            KeyHookSaved?.Invoke(sender, e);
            _outputHandler.LogInfo("Will now save galaxy windows when the `" + (char)sender + "` is pressed.");
        }

        private async void MouseKeyHookHandler_HookEvent(object sender, HookEventArgs e)
        {
            UpdateCheckBoxesIfNeeded();
            var folderName = SaveGalaxyWindowInfoFromScreen();
            if (_cbUploadDiscord.Checked){
                await HandleUploadToDiscord(StoredData.data.SaveFolder + "/" + folderName);
            }
            SetAmountSavedLabelText();
        }

        private void UpdateCheckBoxesIfNeeded()
        {
            void SafeCheckboxEdit(CheckBox checkBox)
            {
                checkBox.Checked = true;
            }
            CheckBox cb = ((StoredData.ApplicationType)StoredData.data.ApplicationType) switch
            {
                StoredData.ApplicationType.Steam => _cbUsingSteamApplication,
                StoredData.ApplicationType.Flash => _cbUsingFlashBrowser,                
            };
            if (cb != null && !cb.Checked) {
                SafeCheckboxEdit(cb);
            }
        }

        /// <summary>
        /// Creates a progess bar and adds it to the panel.
        /// </summary>
        /// <returns>The created progress bar</returns>
        private ProgressBar CreateAndAddProgressBar()
        {
            var pb = new ProgressBar();
            pb.Width = _progressBarsPanel.Width;
            pb.Location = new Point(0, _progressBarsPanel.Controls.Count * pb.Height);
            if (_progressBarsPanel.InvokeRequired){
                void SafeProgressBarEdit()
                {
                    CreateAndAddProgressBar();
                    UpdateProgressBarsPanelHeight();
                }

                _progressBarsPanel.Invoke((Action)SafeProgressBarEdit);
            }
            else{
                _progressBarsPanel.Controls.Add(pb);
                UpdateProgressBarsPanelHeight();
            }
            return pb;
        }
        private void RemoveProgressBar(ProgressBar progressBar)
        {
            if (_progressBarsPanel.InvokeRequired){
                void SafeProgressBarEdit()
                {
                    _progressBarsPanel.Controls.Remove(progressBar);
                    UpdateProgressBarsPanelHeight();
                }

                _progressBarsPanel.Invoke((Action)SafeProgressBarEdit);
            }
            else{
                _progressBarsPanel.Controls.Remove(progressBar);
                UpdateProgressBarsPanelHeight();
            }
        }

        private void UpdateProgressBarsPanelHeight()
        {
            var height = 0;
            for (var i = 0; i < _progressBarsPanel.Controls.Count; i++){
                height += _progressBarsPanel.Controls[i].Height;
            }
            _progressBarsPanel.Height = height;
        }

        /// <summary>
        /// Saves the info of a galaxy window that is currently being visited ingame (Galaxy Life). It creates a png file and a json file.
        /// </summary>
        /// <returns>The image file full path.</returns>
        public string SaveGalaxyWindowInfoFromScreen()
        {
            var pb = CreateAndAddProgressBar();
            pb.Value = 10;
            pb.Value = 20;
            pb.Value = 30;
            var dtNow = DateTime.Now.ToString("ddmmyyyyhhMMss");
            pb.Value = 40;
            var image = GetGalaxyWindowAsImage();
            var didNotWork = image == null || image.Item1 == null;
            if (!didNotWork){
                pb.Value = 50;
                var galaxyWindow = _windowCapturer.IsolateInfoFromBitmap(image.Item1, image.Item2);
                pb.Value = 60;
                if (galaxyWindow == null){
                    didNotWork = true;
                }
                else{
                    galaxyWindow.Save(dtNow);
                }
                pb.Value = 70;
                pb.Value = 80;
                pb.Value = 90;
            }
            if (didNotWork) {
                SoundHandler.Play(SoundHandler.Sounds.Voice_1);
                _outputHandler.LogInfo("No popup could be isolated.");
            }
            pb.Value = 100;
            RemoveProgressBar(pb);
            return dtNow;
        }
        private Tuple<Bitmap, float> GetGalaxyWindowAsImage()
        {
            var screenshotTuple = _windowCapturer.CaptureProcess();
            return new Tuple<Bitmap, float>(_windowCapturer.GetGalaxyWindowFromScreenshot((StoredData.ApplicationType)StoredData.data.ApplicationType,
            screenshotTuple.Item1,
            screenshotTuple.Item2),
            screenshotTuple.Item2.Zoom);
        }
        public async Task StartGalaxyLife()
        {
            await Task.Run(() => {
                Cursor.Current = Cursors.WaitCursor;
                _outputHandler.LogInfo("Starting Galaxy Life...");
                Process.Start(Constants.GlProcessUrl);
                SoundHandler.Play(SoundHandler.Sounds.Repair);
                Cursor.Current = Cursors.Default;
            });
        }

        internal async Task OpenSaveFolder()
        {
            await Task.Run(() => {
                Cursor.Current = Cursors.WaitCursor;
                _outputHandler.LogInfo("Opening save folder...");
                string path = GetCorrectPath(StoredData.data.SaveFolder);
                Process.Start("explorer.exe", path);
                Cursor.Current = Cursors.Default;
            });
        }

        public void StartDiscordBot()
        {
            if (_bot == null){
                if (!_backgroundWorker.IsBusy){
                    _backgroundWorkerProgressBar = CreateAndAddProgressBar();
                    _backgroundWorker.RunWorkerAsync();
                }
            }
        }

        private bool _stopUploading;
        public async Task StartUploadingImages(bool tryingAgain = false)
        {
            while (_bot is not { Ready: true }){
                await Task.Delay(100);
            }
            List<string> savedFolders;
            const int amountOfThreads = 3;
            do{
                savedFolders = Directory.EnumerateDirectories(StoredData.data.SaveFolder).ToList();
                do{
                    var tasks = new Task[amountOfThreads > savedFolders.Count ? savedFolders.Count : amountOfThreads];
                    for (var i = 0; i < tasks.Length && !_stopUploading; i++){
                        tasks[i] = HandleUploadToDiscord(savedFolders[i]);
                    }
                    await Task.WhenAll(tasks);
                    for (var i = 0; i < tasks.Length; i++){
                        savedFolders.RemoveAt(0);
                    }
                } while (!_stopUploading && savedFolders.Count > 0);
                savedFolders = Directory.EnumerateDirectories(StoredData.data.SaveFolder).ToList();
            } while (!_stopUploading && savedFolders.Count > 0);
            if (_stopUploading){
                _stopUploading = false;
                await StartUploadingImages(tryingAgain: true);
                //if (tryingAgain){
                //    _outputHandler.LogError("A problem occured when uploading some images to discord!");
                //    SoundHandler.Play(SoundHandler.Sounds.Voice_3);
                //}
            }
            else {
                SoundHandler.Play(SoundHandler.Sounds.Hurray);
                _outputHandler.LogInfo("Done uploading images!");
            }
        }

        private async Task HandleUploadToDiscord(string folderPath)
        {
            var progressBar = CreateAndAddProgressBar();
            progressBar.Value = 10;
            if (!await _bot.UploadImageInChannel(folderPath, progressBar)){
                _stopUploading = true;
            }
            progressBar.Value = 80;
            try{
                Directory.Delete(folderPath, true);
            }
            catch{
                _outputHandler.LogCritical("Could not delete the saved folder:\n" + folderPath);
            }
            progressBar.Value = 100;
            RemoveProgressBar(progressBar);
            SetAmountSavedLabelText();
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ((BackgroundWorker)sender).ReportProgress(0);
            _outputHandler.LogInfo("Linking discord with this application...");
            ((BackgroundWorker)sender).ReportProgress(10);
            _bot = new Bot(_outputHandler);
            ((BackgroundWorker)sender).ReportProgress(20);
            _bot.RunAsync((BackgroundWorker)sender).GetAwaiter().GetResult();
        }

        public void ResetHook()
        {
            _mouseKeyHookHandler.ResetHook();
        }
        public void SubscribeHook(bool forceSubscribe = false)
        {
            _mouseKeyHookHandler.Subscribe(forceSubscribe);
            _outputHandler.LogInfo("Saving galaxy windows.");
        }
        public void UnsubscribeHook(bool forceUnsubscribe = false)
        {
            _mouseKeyHookHandler.Unsubscribe(forceUnsubscribe);
            _outputHandler.LogInfo("Not saving galaxy windows anymore.");
        }
        public bool IsSubscribedToAHook()
        {
            return _mouseKeyHookHandler.IsSubscribed();
        }
        public static string GetCorrectPath(string path)
        {
            if (!StoredData.data.SaveFolder.StartsWith(".\\")) return path;
            var localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)?.Remove(0, 6);
            path = path.Remove(0, 1);
            if (localPath != null) return localPath.Replace(".\\", string.Empty) + path;
            return path;
        }
        private void SetAmountSavedLabelText()
        {
            void SafelblAmountSavedEdit()
            {
                _lblAmountSaved.Text = Directory.GetDirectories(StoredData.data.SaveFolder).Length + " Stored";
                _gui.SetlblAmountSavedLocation();
            }
            if (_lblAmountSaved.InvokeRequired)
            {
                _lblAmountSaved.Invoke((Action)SafelblAmountSavedEdit);
            }
            else
            {
                SafelblAmountSavedEdit();
            }
        }
    }
}
