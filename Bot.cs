using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Life_Tool {
    public class Bot {
        private static DiscordClient _discordClient;
        private readonly OutputHandler _outputHandler;
        private BackgroundWorker _backgroundWorker;
        private DiscordGuild _discordGuild;

        private DiscordChannel _uploadImagesChannel;

        // private const string Token = "";//GLBot
        private const string Token = "";//GalacticSwamp bot
        public const string Prefix = "/";
        public bool Ready;
        public async Task RunAsync(BackgroundWorker backgroundWorker)
        {
            _backgroundWorker = backgroundWorker;
            backgroundWorker.ReportProgress(50);
            _discordClient = new DiscordClient(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });
            backgroundWorker.ReportProgress(80);
            backgroundWorker.ReportProgress(95);
            await _discordClient.ConnectAsync(status: UserStatus.Online);
            backgroundWorker.ReportProgress(100);
            //Events
            _discordClient.GuildDownloadCompleted += DiscordClient_GuildDownloadCompleted;
            await Task.Delay(-1);
        }

        private Task DiscordClient_GuildDownloadCompleted(DiscordClient sender, DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs e)
        {
            _discordGuild = e.Guilds.Single(g => g.Key == Constants.Guilds.GalacticSwamp).Value;
            _uploadImagesChannel = _discordGuild.GetChannel(Constants.Channels.Raw);
            _outputHandler.LogInfo("Discord is now available to upload images towards!");
            Ready = true;
            return Task.CompletedTask;
        }

        public Bot(OutputHandler outputHandler)
        {
            _outputHandler = outputHandler;
        }

        public async Task<bool> UploadImageInChannel(string folderPath, ProgressBar progressBar)
        {
            if (_uploadImagesChannel != null){
                progressBar.Value = 20;
                var dmb = new DiscordMessageBuilder();
                bool atLeastOneFileLocked;
                try{
                    atLeastOneFileLocked = await FileStreamHandling(dmb, new List<FileStream>(), Directory.GetFiles(folderPath).ToList());
                }
                catch (Exception ex){
                    return false;
                }
                return !atLeastOneFileLocked;
            }
            return false;
        }
        private async Task<bool> FileStreamHandling(DiscordMessageBuilder dmb, List<FileStream> fileStreams, IList<string> files)
        {
            if (!IsFileLocked(new FileInfo(files.First()))){
                using (var fs = new FileStream(files.First(), FileMode.OpenOrCreate, FileAccess.ReadWrite)){
                    fileStreams.Add(fs);
                    files.RemoveAt(0);
                    if (files.Any()){
                        return await FileStreamHandling(dmb, fileStreams, files);
                    }
                    foreach (var fileStream in fileStreams){
                        dmb.AddFile(fileStream);
                    }
                    try{
                        await _uploadImagesChannel.SendMessageAsync(dmb);
                    }
                    catch (Exception ex){
                        return true;
                    }
                    foreach (var fileStream in fileStreams){
                        fileStream.Close();
                    }
                }
                return false;
            }
            return true;
        }
        
        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using(FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public async Task Stop()
        {
            await _discordClient.DisconnectAsync();
        }
    }
}
