using System;
using System.Drawing;
using Bitmap = System.Drawing.Bitmap;
using Image = SixLabors.ImageSharp.Image;
using Color = SixLabors.ImageSharp.Color;
using Rectangle = SixLabors.ImageSharp.Rectangle;
using System.Drawing.Imaging;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace Galaxy_Life_Tool
{

    public static class ImageHelper
    {
        public static Image CropEz(this Image image, Rectangle rect)
        {
            image.Mutate(i => i.Resize(image.Width, image.Height).Crop(rect));
            return image;
        }
        public static Image CropByColor(this Image bitmap, Color color)
        {
            return CropByColor(bitmap, new Color[] { color });
        }
        public static Image CropByColorReverse(this Image bitmap, Color color)
        {
            return CropByColorReverse(bitmap, new Color[] { color });
        }
        public static Image CropByColor(this Image bitmap, Color[] colors)
        {
            int left = -1;
            int right = -1;
            int top = -1;
            int bottom = -1;
            int modus = 10;
            Image<Rgba32> image = Image.Load<Rgba32>("my_file.png");
            image.ProcessPixelRows(accessor =>
            {
                for (int j = 0; j < accessor.Height; j++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(j);
                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int i = 0; i < pixelRow.Length; i++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 tempColor = ref pixelRow[i];
                        for (int k = 0; k < colors.Length; k++)
                        {
                            Rgba32 color = colors[k];
                            if (tempColor.R > color.R - modus && tempColor.R < color.R + modus &&
                            tempColor.G > color.G - modus && tempColor.G < color.G + modus &&
                            tempColor.B > color.B - modus && tempColor.B < color.B + modus)
                            {
                                if (right < i)
                                {
                                    right = i;
                                    if (left < 0)
                                    {
                                        left = i;
                                    }
                                }
                                if (left > i)
                                {
                                    left = i;
                                }
                                if (bottom < j)
                                {
                                    bottom = j;
                                    if (top < 0)
                                    {
                                        top = i;
                                    }
                                }
                                if (top > j)
                                {
                                    top = j;
                                }
                            }
                        }
                    }
                }
            });
            if (right - left > 0 && bottom - top > 0)
            {
                var rect = new Rectangle(left /*- (left > 0 ? 1 : 0)*/, top, right - left + 2, bottom - top + 1);
                //var bitmaprect = new Rectangle(0,0,bitmap.Width,bitmap.Height);
                if (rect.Width > bitmap.Width)
                {
                    rect.Width = bitmap.Width;
                }
                if (rect.Height > bitmap.Height)
                {
                    rect.Height = bitmap.Height;
                }
                if (rect.Width + rect.X > bitmap.Width)
                {
                    rect.Width = bitmap.Width - rect.X;
                }
                if (rect.Height + rect.Y > bitmap.Height)
                {
                    rect.Height = bitmap.Height - rect.Y;
                }
                bitmap.CropEz(rect);
            }
            return bitmap;
        }
        public static Image CropByColorReverse(this Image bitmap, Color[] colors)
        {
            int left = -1;
            int right = 0;
            int modus = 10;
            Image<Rgba32> image = Image.Load<Rgba32>("my_file.png");
            image.ProcessPixelRows(accessor =>
            {
                // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                Rgba32 transparent = Color.Transparent;

                for (int j = 0; j < accessor.Height; j++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(j);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int i = 0; i < pixelRow.Length; i++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 tempColor = ref pixelRow[i];

                        for (int k = 0; k < colors.Length; k++)
                        {
                            Rgba32 color = colors[k];
                            if (tempColor.R < color.R - modus || tempColor.R > color.R + modus &&
                            tempColor.G < color.G - modus || tempColor.G > color.G + modus &&
                            tempColor.B < color.B - modus || tempColor.B > color.B + modus)
                            {
                                if (left < 0)
                                {
                                    left = i;
                                }
                                right = i;
                                break;
                            }
                        }
                    }
                }
            });

            int top = -1;
            int bottom = 0;
            image.ProcessPixelRows(accessor =>
            {
                // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                Rgba32 transparent = Color.Transparent;

                for (int j = 0; j < accessor.Height; j++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(j);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int i = 0; i < pixelRow.Length; i++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 tempColor = ref pixelRow[i];

                        for (int k = 0; k < colors.Length; k++)
                        {
                            Rgba32 color = colors[k];
                            if (tempColor.R < color.R - modus || tempColor.R > color.R + modus &&
                            tempColor.G < color.G - modus || tempColor.G > color.G + modus &&
                            tempColor.B < color.B - modus || tempColor.B > color.B + modus)
                            {
                                if (top < 0)
                                {
                                    top = i;
                                }
                                bottom = i;
                                break;
                            }
                        }
                    }
                }
            });

            var rect = new Rectangle(left, top, right - left + 1, bottom - top + 1);
            if (rect.Width == 0 || rect.Height == 0 || top < 0 || bottom < 0)
            {
                return bitmap;
            }

            bitmap.CropEz(rect);
            return bitmap;
        }

        public static Image RemoveOtherColors(this Image bitmap, Color[] colors, Color? changeOtherColorsTo = null)
        {
            int modus = 4;
            Image<Rgba32> image = Image.Load<Rgba32>("my_file.png");
            image.ProcessPixelRows(accessor =>
            {
                // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                Rgba32 transparent = Color.Transparent;

                for (int j = 0; j < accessor.Height; j++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(j);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int i = 0; i < pixelRow.Length; i++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 tempColor = ref pixelRow[i];
                        bool trueOnce = false;
                        for (int k = 0; k < colors.Length && !trueOnce; k++)
                        {
                            Rgba32 color = colors[k];
                            if (tempColor.R > color.R - modus && tempColor.R < color.R + modus &&
                        tempColor.G > color.G - modus && tempColor.G < color.G + modus &&
                        tempColor.B > color.B - modus && tempColor.B < color.B + modus)
                            {
                                trueOnce = true;
                            }
                        }
                        if (trueOnce)
                        {
                            if (changeOtherColorsTo.HasValue)
                            {
                                pixelRow[i] = changeOtherColorsTo.Value;
                            }
                        }
                    }
                }
            });
            return bitmap;
        }


        public static Image SetRGBValues(this Image img, int r = -1, int g = -1, int b = -1)
        {
            if (img is Image<Rgb24>)
            {
                ((Image<Rgb24>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgb24 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgb24(
                                r < 0 ? (byte)tempColor.R : (byte)r,
                                g < 0 ? (byte)tempColor.G : (byte)g,
                                b < 0 ? (byte)tempColor.B : (byte)b);
                        }
                    }
                });
            }
            else
            {
                ((Image<Rgba32>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgba32 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgba32(
                                r < 0 ? (byte)tempColor.R : (byte)r,
                                g < 0 ? (byte)tempColor.G : (byte)g,
                                b < 0 ? (byte)tempColor.B : (byte)b,
                                tempColor.A);
                        }
                    }
                });
            }
            return img;
        }
        public static Image SetTimesValue(this Image img, int value)
        {
            if (img is Image<Rgb24>)
            {
                ((Image<Rgb24>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgb24 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgb24(
                                (byte)(tempColor.R * value),
                                (byte)(tempColor.G * value),
                                (byte)(tempColor.B * value));
                        }
                    }
                });
            }
            else
            {
                ((Image<Rgba32>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgba32 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgba32(
                                (byte)(tempColor.R * value),
                                (byte)(tempColor.G * value),
                                (byte)(tempColor.B * value),
                            tempColor.A);
                        }
                    }
                });
            }
            return img;
        }
        public static Image SetDividedValue(this Image img, int value)
        {
            if (img is Image<Rgb24>)
            {
                ((Image<Rgb24>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgb24 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgb24(
                                (byte)(tempColor.R / value),
                                (byte)(tempColor.G / value),
                                (byte)(tempColor.B / value));
                        }
                    }
                });
            }
            else
            {
                ((Image<Rgba32>)img).ProcessPixelRows(accessor =>
                {
                    // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                    Rgba32 transparent = Color.Transparent;

                    for (int j = 0; j < accessor.Height; j++)
                    {
                        var pixelRow = accessor.GetRowSpan(j);

                        // pixelRow.Length has the same value as accessor.Width,
                        // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                        for (int i = 0; i < pixelRow.Length; i++)
                        {
                            // Get a reference to the pixel at position x
                            ref Rgba32 tempColor = ref pixelRow[i];
                            pixelRow[i] = new Rgba32(
                                (byte)(tempColor.R / value),
                                (byte)(tempColor.G / value),
                                (byte)(tempColor.B / value),
                            tempColor.A);
                        }
                    }
                });
            }
            return img;
        }

        public static Image ScaleImage(this Image bitmap, int width, int height)
        {
            bitmap.Mutate(x => x.Resize(width, height));
            return bitmap;
        }
        public static Image ScaleImageWidth(this Image bitmap, int width)
        {
            double scale = (double)width / (double)bitmap.Width;
            bitmap.Mutate(x => x.Resize(width, (int)((double)bitmap.Height * scale)));
            return bitmap;
        }
        public static Image ScaleImageHeight(this Image bitmap, int height)
        {
            double scale = (double)height / (double)bitmap.Height;
            bitmap.Mutate(x => x.Resize((int)((double)bitmap.Width * scale), height));
            return bitmap;
        }
        public static Image ResizeImage(this Image bitmap, int width, int height, bool center = false)
        {
            using (Image<Rgba32> newImage = new Image<Rgba32>(width, height))
            {
                if (center)
                {
                    int diff = width - bitmap.Width;
                    int half = diff / 2;
                    int left = diff - half;
                    newImage.Mutate(x => x.DrawImage(bitmap, new SixLabors.ImageSharp.Point(left, 0), 1f));
                }
                else
                {
                    newImage.Mutate(x => x.DrawImage(bitmap, new SixLabors.ImageSharp.Point(0, 0), 1f));
                }
                return newImage;
            }
        }


        public static Image SetContrast(this Image bmp, int threshold)
        {
            float contrast = threshold;
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0f + contrast) / 100.0f;
            contrast *= contrast;
            bmp.Mutate(x => x.Contrast(contrast));
            return bmp;
            //Color col;
            //for (int i = 0; i < bmap.Width; i++)
            //{
            //    for (int j = 0; j < bmap.Height; j++)
            //    {
            //        col = bmap.GetPixel(i, j);
            //        double pRed = col.R / 255.0;
            //        pRed -= 0.5;
            //        pRed *= contrast;
            //        pRed += 0.5;
            //        pRed *= 255;
            //        if (pRed < 0) pRed = 0;
            //        if (pRed > 255) pRed = 255;

            //        double pGreen = col.G / 255.0;
            //        pGreen -= 0.5;
            //        pGreen *= contrast;
            //        pGreen += 0.5;
            //        pGreen *= 255;
            //        if (pGreen < 0) pGreen = 0;
            //        if (pGreen > 255) pGreen = 255;

            //        double pBlue = col.B / 255.0;
            //        pBlue -= 0.5;
            //        pBlue *= contrast;
            //        pBlue += 0.5;
            //        pBlue *= 255;
            //        if (pBlue < 0) pBlue = 0;
            //        if (pBlue > 255) pBlue = 255;

            //        bmap.SetPixel(i, j,
            //Color.FromArgb((byte)pRed, (byte)pGreen, (byte)pBlue));
            //    }
            //}
            //return bmap;
        }
        public static Image SetBlackAndWhite(this Image SourceImage)
        {
            SourceImage.Mutate(x => x.BlackWhite());
            return SourceImage;
            //using (Graphics gr = Graphics.FromImage(SourceImage)) // SourceImage is a Image object
            //{
            //    var gray_matrix = new float[][] {
            //    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
            //    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
            //    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
            //    new float[] { 0,      0,      0,      1, 0 },
            //    new float[] { 0,      0,      0,      0, 1 }
            //};

            //    var ia = new System.Drawing.Imaging.ImageAttributes();
            //    ia.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(gray_matrix));
            //    ia.SetThreshold(0.8f); // Change this threshold as needed
            //    var rc = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
            //    gr.DrawImage(SourceImage, rc, 0, 0, SourceImage.Width, SourceImage.Height, GraphicsUnit.Pixel, ia);
            //}
            //return SourceImage;
        }
        public static Image SetBrightness(this Image bmp, float brightness = 1.0f)
        {
            bmp.Mutate(x => x.Brightness(brightness));
            return bmp;
            //    Image originalImage = bmp;
            //    Image adjustedImage = new Image(originalImage.Width, originalImage.Height);
            //    float contrast = 2.0f; // twice the contrast
            //    float gamma = 1.0f; // no change in gamma

            //    float adjustedBrightness = brightness - 1.0f;
            //    // create matrix that will brighten and contrast the image
            //    float[][] ptsArray ={
            //new float[] {contrast, 0, 0, 0, 0}, // scale red
            //new float[] {0, contrast, 0, 0, 0}, // scale green
            //new float[] {0, 0, contrast, 0, 0}, // scale blue
            //new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
            //new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            //    ImageAttributes imageAttributes = new ImageAttributes();
            //    imageAttributes.ClearColorMatrix();
            //    imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Image);
            //    imageAttributes.SetGamma(gamma, ColorAdjustType.Image);
            //    Graphics g = Graphics.FromImage(adjustedImage);
            //    g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
            //        , 0, 0, originalImage.Width, originalImage.Height,
            //        GraphicsUnit.Pixel, imageAttributes);

            //    return adjustedImage;
        }
        //public static Image SetRGValues(this Image bmp, int amount)
        //{
        //    var lockedImage = new Image(bmp);

        //    for (int y = 0; y < lockedImage.Height; y++)
        //    {
        //        for (int x = 0; x < lockedImage.Width; x++)
        //        {
        //            lockedImage.SetPixel(x, y, new );
        //        }
        //    }
        //    return lockedImage;
        //}
        //public static Image ChangeNonTrasparentPixelsToColor(this Image bmp, Color color)
        //{
        //    var lockedImage = new Image(bmp);

        //    for (int y = 0; y < lockedImage.Height; y++)
        //    {
        //        for (int x = 0; x < lockedImage.Width; x++)
        //        {
        //            lockedImage.SetPixel(x, y, color);
        //        }
        //    }
        //    return lockedImage;
        //}
        public static Image SetBackground(this Image bmp, Color color)
        {
            bmp.Mutate(x => x.BackgroundColor(color));
            return bmp;
            //Image newImage = new Image(bmp.Width, bmp.Height);
            //using (Graphics g = Graphics.FromImage(newImage))
            //{
            //    g.Clear(color);
            //    g.DrawImage(bmp, 0, 0);
            //}
            //return newImage;
        }
        public static Image CropByFirstXCoordinateOfContainedImage(this Image bmp, Image containedImage)
        {
            int firstXInBmp = 0;
            int firstYInBmp = 0;
            ((Image<Rgba32>)bmp).ProcessPixelRows(accessor =>
            {
                for (int j = 0; j < accessor.Height && firstYInBmp + containedImage.Height < accessor.Height; j++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(j);
                    for (int i = 0; i < pixelRow.Length && firstXInBmp + containedImage.Width < pixelRow.Length; i++)
                    {
                        ref Rgba32 tempColor = ref pixelRow[i];
                        int xDiff = i - firstXInBmp;
                        int yDiff = j - firstYInBmp;
                        if (xDiff > -1 && yDiff > -1 && xDiff < containedImage.Width && yDiff < containedImage.Height)
                        {
                            var color = ((Image<Rgb24>)containedImage)[xDiff, yDiff];
                            if (tempColor.R == color.R && tempColor.G == color.G && tempColor.B == color.B)
                            {
                                if (xDiff == containedImage.Width && yDiff == containedImage.Height)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                firstXInBmp = i + (i + 1 < accessor.Width ? 1 : -i);
                                firstYInBmp = j + (i + 1 == accessor.Width && j + 1 < accessor.Height ? 1 : -j);
                            }
                        }
                    }
                }
            });
            bmp.CropEz(new Rectangle(0, 0, firstYInBmp, firstXInBmp));
            return bmp;
        }
        public static Image ThickenEdges(this Image bmp)
        {
            bmp.Mutate(x => x.DetectEdges());
            return bmp;
        }
        public static Image SetColorsInverted(this Image bmp)
        {
            bmp.Mutate(x => x.Invert());
            return bmp;
            //var lockedImage = new Image(bmp);
            //for (int y = 0; (y <= (bmp.Height - 1)); y++)
            //{
            //    for (int x = 0; (x <= (bmp.Width - 1)); x++)
            //    {
            //        Color inv = bmp.GetPixel(x, y);
            //        inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
            //        lockedImage.SetPixel(x, y, inv);
            //    }
            //}
            //return lockedImage;
        }

        public static Image BytesToSixLaborsImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.Load(ms);
                return returnImage;
            }
        }
        public static System.Drawing.Image BytesToDrawingImage(this byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = System.Drawing.Image.FromStream(ms);
                return returnImage;
            }
        }
        public static Bitmap BytesToBitmap(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = (Bitmap)Bitmap.FromStream(ms);
                return returnImage;
            }
        }
        public static byte[] BitmapToBytes(this System.Drawing.Bitmap bmp)
        {
            return ((System.Drawing.Image)bmp).ImageDrawingToBytes();
        }
        public static byte[] ImageDrawingToBytes(this System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public static byte[] ImageSixLaborsToBytes(this SixLabors.ImageSharp.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, PngFormat.Instance);
                return ms.ToArray();
            }
        }
        public static Bitmap ToBitmap(this Image img)
        {
            return BytesToBitmap(img.ImageSixLaborsToBytes());
        }
        public static Image ToImageSixLabors(this Bitmap bmp)
        {
            return BytesToSixLaborsImage(bmp.BitmapToBytes());
        }
    }
}
