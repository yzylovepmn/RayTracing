using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public abstract class Material
    {
        public virtual bool Scatter(ref Ray3f ray, ref RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered, out float pdf)
        {
            attenuation = new Colorf();
            scattered = new Ray3f();
            pdf = 1;
            return false;
        }

        public virtual float ScatteringPdf(ref Ray3f ray, ref RayHitResult hitResult, ref Ray3f scattered)
        {
            return 1;
        }

        public virtual Colorf Emitted(ref RayHitResult ret)
        {
            return Colorf.Black;
        }
    }
}