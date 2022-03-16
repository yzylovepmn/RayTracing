using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class CheckerTexture : Texture
    {
        public CheckerTexture(Colorf even, Colorf odd) : this(new SolidColorTexture(even), new SolidColorTexture(odd))
        {
        }

        public CheckerTexture(Texture even, Texture odd)
        {
            _even = even;
            _odd = odd;
        }

        public Texture Even
        {
            get { return _even; }
            set { _even = value; }
        }
        private Texture _even;

        public Texture Odd
        {
            get { return _odd; }
            set { _odd = value; }
        }
        private Texture _odd;

        public override Colorf Sample(float u, float v, Point3f p)
        {
            var value = Math.Sin(u * 50 * Math.PI) * Math.Sin(v * 50 * Math.PI);
            if (value < 0)
                return _odd.Sample(u, v, p);
            else return _even.Sample(u, v, p);
        }
    }
}