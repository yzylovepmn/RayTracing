using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracing.Core
{
    public class ColorHSV
    {
        public float H { get { return _h; } set { _h = value; } }
        public float S { get { return _s; } set { _s = value; } }
        public float V { get { return _v; } set { _v = value; } }
        public float A { get { return _a; } set { _a = value; } }

        private float _h;
        private float _s;
        private float _v;
        private float _a;

        public ColorHSV(float h, float s, float v, float a = 1) { this._h = h; this._s = s; this._v = v; this._a = a; }
        public ColorHSV(Colorf rgb) {
            ConvertFromRGB(rgb);
        }



        public Colorf RGBA {
            get {
                return ConvertToRGB();
            }
            set { ConvertFromRGB(value); }
        }



        public Colorf ConvertToRGB()
        {
            float h = this._h;
            float s = this._s;
            float v = this._v;

            if (h > 360)
                h -= 360;
            if (h < 0)
                h += 360;
            h = MathUtil.Clamp(h, 0.0f, 360.0f);
            s = MathUtil.Clamp(s, 0.0f, 1.0f);
            v = MathUtil.Clamp(v, 0.0f, 1.0f);
            float c = v * s;
            float x = c * (1 - Math.Abs( ((h / 60.0f) % 2) - 1) );
            float m = v - c;
            float rp, gp, bp;
            int a = (int)(h / 60.0f);

            switch (a) {
                case 0:
                    rp = c;
                    gp = x;
                    bp = 0;
                    break;

                case 1:
                    rp = x;
                    gp = c;
                    bp = 0;
                    break;

                case 2:
                    rp = 0;
                    gp = c;
                    bp = x;
                    break;

                case 3:
                    rp = 0;
                    gp = x;
                    bp = c;
                    break;

                case 4:
                    rp = x;
                    gp = 0;
                    bp = c;
                    break;

                default: // case 5:
                    rp = c;
                    gp = 0;
                    bp = x;
                    break;
            }

            return new Colorf(
                MathUtil.Clamp(rp + m,0,1), 
                MathUtil.Clamp(gp + m,0,1), 
                MathUtil.Clamp(bp + m,0,1), this._a);
        }


        public void ConvertFromRGB(Colorf rgb)
        {
            this._a = rgb.A;
            float rp = rgb.R, gp = rgb.G, bp = rgb.B;

            float cmax = rp;
            int cmaxwhich = 0; /* faster comparison afterwards */
            if (gp > cmax) { cmax = gp; cmaxwhich = 1; }
            if (bp > cmax) { cmax = bp; cmaxwhich = 2; }
            float cmin = rp;
            //int cminwhich = 0;
            if (gp < cmin) { cmin = gp; /*cminwhich = 1;*/ }
            if (bp < cmin) { cmin = bp; /*cminwhich = 2;*/ }

            float delta = cmax - cmin;

            /* HUE */
            if (delta == 0) {
                this._h = 0;
            } else {
                switch (cmaxwhich) {
                    case 0: /* cmax == rp */
                        _h = 60.0f * ( ((gp - bp) / delta) % 6.0f );
                        break;

                    case 1: /* cmax == gp */
                        _h = 60.0f * (((bp - rp) / delta) + 2);
                        break;

                    case 2: /* cmax == bp */
                        _h = 60.0f * (((rp - gp) / delta) + 4);
                        break;
                }
                if (_h < 0)
                    _h += 360.0f;
            }

            /* LIGHTNESS/VALUE */
            //l = (cmax + cmin) / 2;
            _v = cmax;

            /* SATURATION */
            /*if (delta == 0) {
              *r_s = 0;
            } else {
              *r_s = delta / (1 - fabs (1 - (2 * (l - 1))));
            }*/
            if (cmax == 0) {
                _s = 0;
            } else {
                _s = delta / cmax;
            }
        }

    }
}
