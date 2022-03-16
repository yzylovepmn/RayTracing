using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class DiffuseMaterial : Material
    {
        public DiffuseMaterial(Colorf emissive) : this(new SolidColorTexture(emissive))
        {
        }

        public DiffuseMaterial(Texture emissive)
        {
            _emissive = emissive;
        }

        public Texture Emissive
        {
            get { return _emissive; }
            set { _emissive = value; }
        }
        private Texture _emissive;

        public override bool Scatter(Ray3f ray, RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered)
        {
            attenuation = Colorf.Black;
            scattered = new Ray3f();
            return false;
        }

        public override Colorf Emitted(float u, float v, Point3f p)
        {
            return _emissive.Sample(u, v, p);
        }
    }
}