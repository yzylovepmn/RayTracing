using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class SolidColorTexture : Texture
    {
        public SolidColorTexture()
        {
            _color = Colorf.Black;
        }

        public SolidColorTexture(Colorf color)
        {
            _color = color;
        }

        public Colorf Color { get { return _color; } }
        private Colorf _color;

        public override Colorf Sample(float u, float v, Point3f p)
        {
            return _color;
        }
    }
}