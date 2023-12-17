using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Galaxy_Life_Tool
{
    public class WindowCapturer
    {
        public Tuple<Bitmap, Measurements> CaptureProcess()
        {
            try
            {
                //Get window location
                var measurements = SelectWindow();

                //Creating a new Bitmap object
                Bitmap captureBitmap = new Bitmap(measurements.Rectangle.Width, measurements.Rectangle.Height, PixelFormat.Format32bppArgb);

                //Creating a New Graphics Object
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);

                // Adjust the coordinates to be relative to the window
                // measurements.Rectangle.Offset(-measurements.Rectangle.Left, -measurements.Rectangle.Top);
                //Copying Image from The Screen
                captureGraphics.CopyFromScreen(measurements.Rectangle.Location, Point.Empty, measurements.Rectangle.Size);
                return new Tuple<Bitmap, Measurements>(captureBitmap, measurements);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Message:\n" + ex.Message
                    + Environment.NewLine + Environment.NewLine
                    + "StackTrace:\n" + ex.StackTrace);
            }
            return null;
        }
        private Measurements SelectWindow()
        {
            return new Measurements();
        }

        public Bitmap ScreenShot()
        {
            return CaptureProcess().Item1;
        }

        private int LastGameHeight { get; set; } //height of game last time GetGalaxyWindowFromScreenshot method was called
        private StoredData.ApplicationType LastApplicationType { get; set; } //height of game last time GetGalaxyWindowFromScreenshot method was called
        private int LastYCoordinate { get; set; } = -1; //calculated y coordinate last time GetGalaxyWindowFromScreenshot was called
        public Bitmap GetGalaxyWindowFromScreenshot(StoredData.ApplicationType applicationType, Bitmap screenshot, Measurements measurements)
        {
            if (LastGameHeight != screenshot.Height || LastApplicationType != applicationType){
                LastGameHeight = screenshot.Height;
                LastApplicationType = applicationType;
                var yOccurance = screenshot.FoundNotConnectedVerticallyOccurance(screenshot.GetColorVerticallyWithBOrLarger(100), occuranceNumber: 2);
                if (yOccurance > -1){
                    LastYCoordinate = yOccurance;
                }
                else{
                    LastYCoordinate = 0;
                }
            }
            if (LastYCoordinate <= -1) return null;

            var popupSize = measurements.PopupSize;
            int popupWidth = popupSize.X;
            int popupHeight = popupSize.Y;
            
            var galaxyWindowSize = new Rectangle(
            (screenshot.Width - popupWidth) / 2,
            (screenshot.Height - LastYCoordinate - popupHeight) / 2 + LastYCoordinate,
            popupWidth,
            popupHeight
            );
            return screenshot.Clone(galaxyWindowSize, screenshot.PixelFormat);
        }
        public GalaxyWindow IsolateInfoFromBitmap(Bitmap bitmap, float zoom)
        {
            var yStartPopup = bitmap.GetYWhereMiddlePartHasBValue(100);
            var xStartPopup = bitmap.GetXWhereMiddlePartHasBValue(100);
            if (yStartPopup != 0 || xStartPopup != 0){
                if (xStartPopup < 0 || yStartPopup < 0){
                    return null;
                }
                bitmap = bitmap.Clone(new Rectangle(xStartPopup, yStartPopup, bitmap.Width - xStartPopup, bitmap.Height - yStartPopup), bitmap.PixelFormat);
            }
            return new GalaxyWindow(bitmap, zoom);
        }
    }
}
