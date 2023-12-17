using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Galaxy_Life_Tool
{

    public class GalaxyWindow {
        private float _zoom;
        public Bitmap Title { get; set; }
        public Bitmap[] Players { get; set; }
        public Bitmap[] Starbases { get; set; }
        public Bitmap[] Levels { get; set; }
        public GalaxyWindow(Bitmap bmp, float zoom)
        {
            _zoom = zoom;
            IsolateTitle(bmp);
            IsolatePlayers(bmp);
            IsolateStarbases(bmp);
            IsolateLevels(bmp);
        }
        private void IsolateLevels(Bitmap bmp)
        {
            var halfSize = CalculateWithZoom(8); //6 (exact) + 2 (to be sure)
            var center1 = CalculateWithZoom(183);
            var center2 = CalculateWithZoom(299);
            Levels = new[]
            {
                Crop(bmp, center1, halfSize),
                Crop(bmp, center2, halfSize)
            };
        }
        private void IsolateStarbases(Bitmap bmp)
        {
            var halfSize = CalculateWithZoom(6); //4 (exact) + 2 (to be sure)
            var center1 = CalculateWithZoom(163);
            var center2 = CalculateWithZoom(279);
            Starbases = new[]
            {
                Crop(bmp, center1, halfSize),
                Crop(bmp, center2, halfSize)
            };
        }
        private void IsolatePlayers(Bitmap bmp)
        {
            var halfSize = CalculateWithZoom(6); //4 (exact) + 2 (to be sure)
            var center1 = CalculateWithZoom(100);
            var center2 = CalculateWithZoom(216);
            Players = new[]
            {
                Crop(bmp, center1, halfSize),
                Crop(bmp, center2, halfSize)
            };
        }
        private void IsolateTitle(Bitmap bmp)
        {
            var center = CalculateWithZoom(22) + (_zoom > 1 ? 3 : 0);
            var halfSize = CalculateWithZoom(12); //11 (exact) + 1 (to be sure)
            Title = Crop(bmp, center, halfSize);
        }
        private int CalculateWithZoom(int original)
        {
            return (int)Math.Floor(original * _zoom);
        }
        private static Bitmap Crop(Bitmap bmp, int center, int halfSize)
        {
            return bmp.Clone(new Rectangle(0, center - halfSize, bmp.Width, halfSize*2), bmp.PixelFormat);
        }
        public void Save(string dtNow)
        {
            var dir = StoredData.data.SaveFolder + "/" + dtNow;
            Directory.CreateDirectory(dir);
            dir += "/";
            CleanSave(Title, "title");
            for (var i = 0; i < Players.Length; i++){
                CleanSave(Players[i], "players"+i);
                CleanSave(Levels[i], "levels"+i);
                CleanSave(Starbases[i], "starbases"+i);
            }

            void CleanSave(Bitmap bmp, string fileNameWithoutExtension)
            {
                using (var fileStream = new FileStream(dir + fileNameWithoutExtension + ".png", FileMode.Create))
                {
                    bmp.Save(fileStream, ImageFormat.Png);
                }
            }
        }
    }
}
