using System;

namespace Galaxy_Life_Tool
{
    public class HookEventArgs : EventArgs
    {
        public int Percentage { get; set; }
        public DateTime StartTime { get; set; }
        public HookEventArgs(DateTime startTime)
        {
            StartTime = startTime;
            Percentage = 0;
        }
    }
}
