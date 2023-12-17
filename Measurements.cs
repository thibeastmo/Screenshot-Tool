using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
namespace Galaxy_Life_Tool {
    public class Measurements {
        private const int PopupWidth = 552;
        private const int PopupHeight = 396;
        private const short BasicExtraPixels = 8;
        public float Zoom { get; set; }

        public Point PopupSize => new Point((int)(PopupWidth * Zoom), (int)(PopupHeight * Zoom));
        public Rectangle Rectangle { get; set; }
        public Rectangle WindowRectangle { get; set; }
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("User32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, out Rectangle rect);
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hwnd, ref Point point);
        public Measurements()
        {
            // Find the handle of the application window
            IntPtr hwnd = FindWindow(null, Constants.ProcessName);
            var windowRect = new Rectangle();
            GetWindowRect(hwnd, ref windowRect);
            // Get the bounds of the client area
            GetClientRect(hwnd, out Rectangle clientRect);
            
            // Convert client coordinates to screen coordinates
            Point clientTopLeft = new Point(clientRect.Left, clientRect.Top);
            Point clientBottomRight = new Point(clientRect.Right, clientRect.Bottom);
            ClientToScreen(hwnd, ref clientTopLeft);
            ClientToScreen(hwnd, ref clientBottomRight);
            
            // Calculate the actual window bounds by considering the title bar and borders
            windowRect.X += clientTopLeft.X - windowRect.Left;
            windowRect.Y += clientTopLeft.Y - windowRect.Top;
            windowRect.Width = clientBottomRight.X - clientTopLeft.X;
            windowRect.Height = clientBottomRight.Y - clientTopLeft.Y;
            
            var dpi = GetDpiForWindow(hwnd);
            Zoom =  dpi / 96.0f;

            // Adjust the window bounds based on DPI scaling
            windowRect.X = (int)Math.Ceiling(windowRect.X * Zoom);
            windowRect.Y = (int)Math.Ceiling(windowRect.Y * Zoom);
            windowRect.Width = (int)Math.Ceiling(windowRect.Width * Zoom);
            windowRect.Height = (int)Math.Ceiling(windowRect.Height * Zoom);
            
            Rectangle = windowRect;
        }
    }
}
