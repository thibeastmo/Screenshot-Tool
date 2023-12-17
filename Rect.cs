namespace Galaxy_Life_Tool
{
    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Width
        {
            get
            {
                return Right - Left;
            }
        }
        public int Height
        {
            get
            {
                return Bottom - Top;
            }
        }
    }
}
