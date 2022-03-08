using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public struct Colorf : IComparable<Colorf>, IEquatable<Colorf>
    {
        public float R { get { return _r; } set { _r = MathUtil.Clamp(value, 0.0f, 1f); } }
        public float G { get { return _g; } set { _g = MathUtil.Clamp(value, 0.0f, 1f); } }
        public float B { get { return _b; } set { _b = MathUtil.Clamp(value, 0.0f, 1f); } }
        public float A { get { return _a; } set { _a = MathUtil.Clamp(value, 0.0f, 1f); } }

        internal float _r;
        internal float _g;
        internal float _b;
        internal float _a;

        public Colorf(float greylevel, float a = 1) { _r = _g = _b = greylevel; this._a = a; }

        public Colorf(float r, float g, float b, float a = 1) { this._r = r; this._g = g; this._b = b; this._a = a; }

        public Colorf(byte r, byte g, byte b, byte a = 255) {
            this._r = MathUtil.Clamp((float)r, 0.0f, 255.0f) / 255.0f;
            this._g = MathUtil.Clamp((float)g, 0.0f, 255.0f) / 255.0f;
            this._b = MathUtil.Clamp((float)b, 0.0f, 255.0f) / 255.0f;
            this._a = MathUtil.Clamp((float)a, 0.0f, 255.0f) / 255.0f;
        }
        public Colorf(float[] v2) { _r = v2[0]; _g = v2[1]; _b = v2[2]; _a = v2[3]; }
        public Colorf(Colorf copy) { _r = copy._r; _g = copy._g; _b = copy._b; _a = copy._a; }
        public Colorf(Colorf copy, float newAlpha) { _r = copy._r; _g = copy._g; _b = copy._b; _a = newAlpha; }


        public Colorf Clone(float fAlphaMultiply = 1.0f) {
            return new Colorf(_r, _g, _b, _a * fAlphaMultiply);
        }


        public float this[int key]
        {
            get { if (key == 0) return _r; else if (key == 1) return _g; else if (key == 2) return _b; else return _a; }
            set { if (key == 0) _r = value; else if (key == 1) _g = value; else if (key == 2) _b = value; else _a = value; }
        }

        public float SqrDistance(Colorf v2)
        {
            float a = (_r - v2._r), b = (_g - v2._g), c = (b - v2._b), d = (a - v2._a);
            return a * a + b * b + c * c + d*d;
        }


        public Vector3f ToRGB() {
            return new Vector3f(_r, _g, _b);
        }
        public Colorb ToBytes() {
            return new Colorb(_r, _g, _b, _a);
        }

        public void Set(Colorf o)
        {
            _r = o._r; _g = o._g; _b = o._b; _a = o._a;
        }
        public void Set(float fR, float fG, float fB, float fA)
        {
            _r = fR; _g = fG; _b = fB; _a = fA;
        }
        public Colorf SetAlpha(float a) {
            this._a = a;
            return this;
        }
        public void Add(Colorf o)
        {
            _r += o._r; _g += o._g; _b += o._b; _a += o._a;
        }
        public void Subtract(Colorf o)
        {
            _r -= o._r; _g -= o._g; _b -= o._b; _a -= o._a;
        }
        public Colorf WithAlpha(float newAlpha)
        {
            return new Colorf(_r, _g, _b, newAlpha);
        }


        public static Colorf operator -(Colorf v)
        {
            return new Colorf(-v._r, -v._g, -v._b, -v._a);
        }

        public static Colorf operator *(Colorf c1, Colorf c2)
        {
            return new Colorf(c1._r * c2._r, c1._g * c2._g, c1._b * c2._b, c1._a * c2._a);
        }

        public static Colorf operator *(float f, Colorf v)
        {
            return new Colorf(f * v._r, f * v._g, f * v._b, f * v._a);
        }
        public static Colorf operator *(Colorf v, float f)
        {
            return new Colorf(f * v._r, f * v._g, f * v._b, f * v._a);
        }

        public static Colorf operator +(Colorf v0, Colorf v1)
        {
            return new Colorf(v0._r + v1._r, v0._g + v1._g, v0._b + v1._b, v0._a + v1._a);
        }
        public static Colorf operator +(Colorf v0, float f)
        {
            return new Colorf(v0._r + f, v0._g + f, v0._b + f, v0._a + f);
        }

        public static Colorf operator -(Colorf v0, Colorf v1)
        {
            return new Colorf(v0._r - v1._r, v0._g - v1._g, v0._b - v1._b, v0._a-v1._a);
        }
        public static Colorf operator -(Colorf v0, float f)
        {
            return new Colorf(v0._r - f, v0._g - f, v0._b - f, v0._a = f);
        }


        public static bool operator ==(Colorf a, Colorf b)
        {
            return (a._r == b._r && a._g == b._g && a._b == b._b && a._a == b._a);
        }
        public static bool operator !=(Colorf a, Colorf b)
        {
            return (a._r != b._r || a._g != b._g || a._b != b._b || a._a != b._a);
        }
        public override bool Equals(object obj)
        {
            return this == (Colorf)obj;
        }
        public override int GetHashCode()
        {
            return (_r+_g+_b+_a).GetHashCode();
        }
        public int CompareTo(Colorf other)
        {
            if (_r != other._r)
                return _r < other._r ? -1 : 1;
            else if (_g != other._g)
                return _g < other._g ? -1 : 1;
            else if (_b != other._b)
                return _b < other._b ? -1 : 1;
            else if (_a != other._a)
                return _a < other._a ? -1 : 1;
            return 0;
        }
        public bool Equals(Colorf other)
        {
            return (_r == other._r && _g == other._g && _b == other._b && _a == other._a);
        }


        public static Colorf Lerp(Colorf a, Colorf b, float t) {
            float s = 1 - t;
            return new Colorf(s * a._r + t * b._r, s * a._g + t * b._g, s * a._b + t * b._b, s * a._a + t * b._a);
        }



        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8} {2:F8} {3:F8}", _r, _g, _b, _a);
        }
        public string ToString(string fmt)
        {
            return string.Format("{0} {1} {2} {3}", _r.ToString(fmt), _g.ToString(fmt), _b.ToString(fmt), _a.ToString(fmt));
        }

        static public readonly Colorf TransparentWhite = new Colorf(255, 255, 255, 0);
        static public readonly Colorf TransparentBlack = new Colorf(0, 0, 0, 0);

        static public readonly Colorf White = new Colorf(255, 255, 255, 255);
        static public readonly Colorf Black = new Colorf(0, 0, 0, 255);
        static public readonly Colorf FontBlack = new Colorf(50, 50, 50, 255);
        static public readonly Colorf Blue = new Colorf(0, 0, 255, 255);
        static public readonly Colorf Green = new Colorf(0, 255, 0, 255);
        static public readonly Colorf Red = new Colorf(255, 0, 0, 255);
        static public readonly Colorf Yellow = new Colorf(255, 255, 0, 255);
        static public readonly Colorf Cyan = new Colorf(0, 255, 255, 255);
        static public readonly Colorf Magenta = new Colorf(255, 0, 255, 255);

        static public readonly Colorf VideoWhite = new Colorf(235, 235, 235, 255);
        static public readonly Colorf VideoBlack = new Colorf(16, 16, 16, 255);
        static public readonly Colorf VideoBlue = new Colorf(16, 16, 235, 255);
        static public readonly Colorf VideoGreen = new Colorf(16, 235, 16, 255);
        static public readonly Colorf VideoRed = new Colorf(235, 16, 16, 255);
        static public readonly Colorf VideoYellow = new Colorf(235, 235, 16, 255);
        static public readonly Colorf VideoCyan = new Colorf(16, 235, 235, 255);
        static public readonly Colorf VideoMagenta = new Colorf(235, 16, 235, 255);

        static public readonly Colorf Purple = new Colorf(161, 16, 193, 255);
        static public readonly Colorf DarkRed = new Colorf(128, 16, 16, 255);
        static public readonly Colorf FireBrick = new Colorf(178, 34, 34, 255);
        static public readonly Colorf HotPink = new Colorf(255, 105, 180, 255);
        static public readonly Colorf LightPink = new Colorf(255, 182, 193, 255);

        static public readonly Colorf DarkBlue = new Colorf(16, 16, 139, 255);
        static public readonly Colorf BlueMetal = new Colorf(176, 197, 235, 255);       // I made this one up...
        static public readonly Colorf Navy = new Colorf(16, 16, 128, 255);
        static public readonly Colorf CornflowerBlue = new Colorf(100, 149, 237, 255);
        static public readonly Colorf LightSteelBlue = new Colorf(176, 196, 222, 255);
        static public readonly Colorf DarkSlateBlue = new Colorf(72, 61, 139, 255);

        static public readonly Colorf Teal = new Colorf(16, 128, 128, 255);
        static public readonly Colorf ForestGreen = new Colorf(16, 139, 16, 255);
        static public readonly Colorf LightGreen = new Colorf(144, 238, 144, 255);

        static public readonly Colorf Orange = new Colorf(230, 73, 16, 255);
        static public readonly Colorf Gold = new Colorf(235, 115, 63, 255);
        static public readonly Colorf DarkYellow = new Colorf(235, 200, 95, 255);

        static public readonly Colorf SiennaBrown = new Colorf(160, 82,  45, 255);
        static public readonly Colorf SaddleBrown = new Colorf(139,  69,  19, 255);
        static public readonly Colorf Goldenrod = new Colorf(218, 165,  32, 255);
        static public readonly Colorf Wheat = new Colorf(245, 222, 179, 255);

        static public readonly Colorf LightGrey = new Colorf(211, 211, 211, 255);
        static public readonly Colorf Silver = new Colorf(192, 192, 192, 255);
        static public readonly Colorf LightSlateGrey = new Colorf(119, 136, 153, 255);
        static public readonly Colorf Grey = new Colorf(128, 128, 128, 255);
        static public readonly Colorf DarkGrey = new Colorf(169, 169, 169, 255);
        static public readonly Colorf SlateGrey = new Colorf(112, 128, 144, 255);
        static public readonly Colorf DimGrey = new Colorf(105, 105, 105, 255);
        static public readonly Colorf DarkSlateGrey = new Colorf(47,  79,  79, 255);

        // default colors
        static readonly public Colorf StandardBeige = new Colorf(0.75f, 0.75f, 0.5f);
        static readonly public Colorf SelectionGold = new Colorf(1.0f, 0.6f, 0.05f);
        static readonly public Colorf PivotYellow = new Colorf(1.0f, 1.0f, 0.05f);

        // allow conversion to/from Vector3f
        public static implicit operator Vector3f(Colorf c)
        {
            return new Vector3f(c._r, c._g, c._b);
        }
        public static implicit operator Colorf(Vector3f c)
        {
            return new Colorf(c.X, c.Y, c.Z, 1);
        }
    }
}