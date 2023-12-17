using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace Galaxy_Life_Tool
{
    public class OutputHandler
    {
        private Form gui;
        private RichTextBox output;
        public OutputHandler(Form form, RichTextBox output)
        {
            gui = form;
            this.output = output;
        }
        private void WriteOutput(string text)
        {
            gui.BeginInvoke(new Action(() =>
            {
                output.Text += text;
            }));
        }
        private void Log(string title, string text)
        {
            if (title != null && title.Length > 0)
            {
                WriteOutput($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} [{title}] {text}\n");
            }
            else
            {
                WriteOutput($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} {text}\n");
            }
        }
        public void LogError(string text)
        {
            Log("ERROR", text);
        }
        public void LogInfo(string text)
        {
            Log("INFO", text);
        }
        public void LogWarning(string text)
        {
            Log("WARNING", text);
        }
        public void LogCritical(string text)
        {
            Log("CRITICAL", text);
        }
        public void LogDebug(string text)
        {
            Log("DEBUG", text);
        }
        public void LogTrace(string text)
        {
            Log("TRACE", text);
        }
        public void Log(string text, LogLevel? logLevel)
        {
            if (logLevel.HasValue)
            {
                switch (logLevel)
                {
                    case LogLevel.Information: LogInfo(text); break;
                    case LogLevel.Error: LogError(text); break;
                    case LogLevel.Warning: LogWarning(text); break;
                    case LogLevel.Critical: LogCritical(text); break;
                    case LogLevel.Debug: LogDebug(text); break;
                    case LogLevel.Trace: LogTrace(text); break;
                    default: Log(null, text); break;
                }
            }
            else
            {
                Log(null, text);
            }
        }
    }
}