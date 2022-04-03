using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class EmissiveMaterial : Material
    {
        public EmissiveMaterial(Colorf emissive) : this(new SolidColorTexture(emissive))
        {
        }

        public EmissiveMaterial(Texture emissive)
        {
            _emissive = emissive;
        }

        public Texture Emissive
        {
            get { return _emissive; }
            set { _emissive = value; }
        }
        private Texture _emissive;

        public override bool Scatter(ref Ray3f ray, ref RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered, out float pdf)
        {
            attenuation = Colorf.Black;
            scattered = new Ray3f();
            pdf = 1;
            return false;
        }

        public override Colorf Emitted(ref RayHitResult ret)
        {
            if (ret.IsFrontFace)
                return _emissive.Sample(ret.U, ret.V, ret.HitPoint);
            return base.Emitted(ref ret);
        }
    }
}