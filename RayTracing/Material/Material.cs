using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public abstract class Material
    {
        public abstract bool Scatter(Ray3f ray, RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered);

        public virtual Colorf Emitted(float u, float v, Point3f p)
        {
            return Colorf.Black;
        }
    }
}