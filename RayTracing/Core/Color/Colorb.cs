using System;

namespace RayTracing.Core
{
    public struct Colorb
    {
        public byte R { get { return _r; } set { _r = value; } }
        public byte G { get { return _g; } set { _g = value; } }
        public byte B { get { return _b; } set { _b = value; } }
        public byte A { get { return _a; } set { _a = value; } }

        private byte _r;
        private byte _g;
        private byte _b;
        private byte _a;

        public Colorb(byte greylevel, byte a = 255) { _r = _g = _b = greylevel; this._a = a; }
        public Colorb(byte r, byte g, byte b, byte a = 255) { this._r = r; this._g = g; this._b = b; this._a = a; }
        public Colorb(float r, float g, float b, float a = 1.0f) {
            this._r = (byte)MathUtil.Clamp((int)(r * 255.0f), 0, 255);
            this._g = (byte)MathUtil.Clamp((int)(g * 255.0f), 0, 255);
            this._b = (byte)MathUtil.Clamp((int)(b * 255.0f), 0, 255);
            this._a = (byte)MathUtil.Clamp((int)(a * 255.0f), 0, 255);
        }
        public Colorb(byte[] v2) { _r = v2[0]; _g = v2[1]; _b = v2[2]; _a = v2[3]; }
        public Colorb(Colorb copy) { _r = copy.R; _g = copy.G; _b = copy.B; _a = copy.A; }
        public Colorb(Colorb copy, byte newAlpha) { _r = copy.R; _g = copy.G; _b = copy.B; _a = newAlpha; }


        public byte this[int key]
        {
            get { if (key == 0) return _r; else if (key == 1) return _g; else if (key == 2) return _b; else return _a; }
            set { if (key == 0) _r = value; else if (key == 1) _g = value; else if (key == 2) _b = value; else _a = value; }
        }

        public Colorf ToColorf()
        {
            return new Colorf(_r, _g, _b, _a);
        }

        static public readonly Colorb White = new Colorb(255, 255, 255, 255);
        static public readonly Colorb Black = new Colorb(0, 0, 0, 255);
        static public readonly Colorb Blue = new Colorb(0, 0, 255, 255);
        static public readonly Colorb Green = new Colorb(0, 255, 0, 255);
        static public readonly Colorb Red = new Colorb(255, 0, 0, 255);
        static public readonly Colorb Yellow = new Colorb(255, 255, 0, 255);
        static public readonly Colorb Cyan = new Colorb(0, 255, 255, 255);
        static public readonly Colorb Magenta = new Colorb(255, 0, 255, 255);

        static public readonly Colorb VideoWhite = new Colorb(235, 235, 235, 255);
        static public readonly Colorb VideoBlack = new Colorb(16, 16, 16, 255);
        static public readonly Colorb VideoBlue = new Colorb(16, 16, 235, 255);
        static public readonly Colorb VideoGreen = new Colorb(16, 235, 16, 255);
        static public readonly Colorb VideoRed = new Colorb(235, 16, 16, 255);
        static public readonly Colorb VideoYellow = new Colorb(235, 235, 16, 255);
        static public readonly Colorb VideoCyan = new Colorb(16, 235, 235, 255);
        static public readonly Colorb VideoMagenta = new Colorb(235, 16, 235, 255);

        static public readonly Colorb Purple = new Colorb(161, 16, 193, 255);
        static public readonly Colorb DarkRed = new Colorb(128, 16, 16, 255);
        static public readonly Colorb FireBrick = new Colorb(178, 34, 34, 255);
        static public readonly Colorb HotPink = new Colorb(255, 105, 180, 255);
        static public readonly Colorb LightPink = new Colorb(255, 182, 193, 255);

        static public readonly Colorb DarkBlue = new Colorb(16, 16, 139, 255);
        static public readonly Colorb BlueMetal = new Colorb(176, 197, 235, 255);       // I made this one up...
        static public readonly Colorb Navy = new Colorb(16, 16, 128, 255);
        static public readonly Colorb CornflowerBlue = new Colorb(100, 149, 237, 255);
        static public readonly Colorb LightSteelBlue = new Colorb(176, 196, 222, 255);
        static public readonly Colorb DarkSlateBlue = new Colorb(72, 61, 139, 255);

        static public readonly Colorb Teal = new Colorb(16, 128, 128, 255);
        static public readonly Colorb ForestGreen = new Colorb(16, 139, 16, 255);
        static public readonly Colorb LightGreen = new Colorb(144, 238, 144, 255);

        static public readonly Colorb Orange = new Colorb(230, 73, 16, 255);
        static public readonly Colorb Gold = new Colorb(235, 115, 63, 255);
        static public readonly Colorb DarkYellow = new Colorb(235, 200, 95, 255);

        static public readonly Colorb SiennaBrown = new Colorb(160, 82, 45, 255);
        static public readonly Colorb SaddleBrown = new Colorb(139, 69, 19, 255);
        static public readonly Colorb Goldenrod = new Colorb(218, 165, 32, 255);
        static public readonly Colorb Wheat = new Colorb(245, 222, 179, 255);

        static public readonly Colorb LightGrey = new Colorb(211, 211, 211, 255);
        static public readonly Colorb Silver = new Colorb(192, 192, 192, 255);
        static public readonly Colorb LightSlateGrey = new Colorb(119, 136, 153, 255);
        static public readonly Colorb Grey = new Colorb(128, 128, 128, 255);
        static public readonly Colorb DarkGrey = new Colorb(169, 169, 169, 255);
        static public readonly Colorb SlateGrey = new Colorb(112, 128, 144, 255);
        static public readonly Colorb DimGrey = new Colorb(105, 105, 105, 255);
        static public readonly Colorb DarkSlateGrey = new Colorb(47, 79, 79, 255);
    }
}