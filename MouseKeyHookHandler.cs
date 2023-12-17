using Gma.System.MouseKeyHook;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Galaxy_Life_Tool
{
    public class MouseKeyHookHandler
    {
        private IKeyboardMouseEvents m_GlobalHook;
        /// <summary>
        /// Key that will trigger something when pressed anywhere on the pc.
        /// </summary>
        private char? Key;
        private bool KeyEventSubscribed;
        /// <summary>
        /// MouseButton that will trigger something when pressed anywhere on the pc.
        /// </summary>
        private MouseButtons? MouseButton;
        private bool MouseButtonEventSubscribed;

        public MouseKeyHookHandler(int hook)
        {
            if (StoredData.data.Hook < ((int)MouseButtons.Left))
            {
                Key = (char)hook;
            }
            else
            {
                MouseButton = (MouseButtons)hook;
            }
        }

        public event EventHandler<HookEventArgs> HookEvent;
        public event EventHandler KeyHookSaved;
        public event EventHandler MouseButtonHookSaved;


        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (Key.HasValue && e.KeyChar == (char)Key.Value)
            {
                HandleHookEvent();
            }
            else if (!Key.HasValue)
            {
                StoredData.data.Hook = e.KeyChar;
                Key = e.KeyChar;
                KeyHookSaved?.Invoke(e.KeyChar, new EventArgs());
            }
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (MouseButton.HasValue && e.Button == MouseButton.Value)
            {
                HandleHookEvent();
            }
            else if (!MouseButton.HasValue)
            {
                StoredData.data.Hook = (int)e.Button;
                MouseButton = e.Button;
                MouseButtonHookSaved?.Invoke(e.Button, new EventArgs());
            }
        }
        private void HandleHookEvent()
        {
            if (IsGalaxyLifeActiveAndOnForeground())
            {
                SoundHandler.Play(SoundHandler.Sounds.Warp_dimmed);
                //SystemSounds.Asterisk.Play();
                HookEvent?.Invoke(this, new HookEventArgs(DateTime.Now));
            }
        }

        public void Subscribe(bool forceSubscribe = false)
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            if (Key.HasValue || forceSubscribe)
            {
                if (!KeyEventSubscribed || forceSubscribe)
                {
                    m_GlobalHook.KeyPress += GlobalHookKeyPress;
                    KeyEventSubscribed = true;
                }
            }
            if (MouseButton.HasValue || forceSubscribe)
            {
                if (!MouseButtonEventSubscribed || forceSubscribe)
                {
                    m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
                    MouseButtonEventSubscribed = true;
                }
            }
        }
        public void Unsubscribe(bool forceUnsubscribe = false)
        {
            bool dispose = false;
            if (KeyEventSubscribed || forceUnsubscribe)
            {
                m_GlobalHook.KeyPress -= GlobalHookKeyPress;
                KeyEventSubscribed = false;
                dispose = true;
            }
            if (MouseButtonEventSubscribed || forceUnsubscribe)
            {
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
                MouseButtonEventSubscribed = false;
                dispose = true;
            }

            //It is recommened to dispose it
            if (dispose)
            {
                m_GlobalHook.Dispose();
            }
        }

        public bool IsSubscribed()
        {
            return KeyEventSubscribed || MouseButtonEventSubscribed;
        }
        public void ResetHook()
        {
            Key = null;
            MouseButton = null;
        }

        #region Processes
        public static bool IsGalaxyLifeActiveAndOnForeground()
        {
            var handle = GetForegroundWindow();
            const int nChars = 256;
            StringBuilder sb = new StringBuilder(nChars);
            var temp = GetWindowText(handle, sb, nChars);
            var x = Marshal.GetLastWin32Error();
            if (Constants.ProcessId == temp || temp == 0)
            {
                return true;
            }
            else
            {
                foreach (var applicationType in Enum.GetValues(typeof(StoredData.ApplicationType)))
                {
                    if ((int)applicationType == temp)
                    {
                        StoredData.data.ApplicationType = (int)applicationType;
                        return true;
                    }
                }
            }
            if (x == 0 && temp == 0) return true;
            return false;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        #endregion
    }
}
