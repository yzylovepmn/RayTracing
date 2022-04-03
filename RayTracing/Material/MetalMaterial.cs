using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class MetalMaterial : Material
    {
        public MetalMaterial()
        {
            _albedo = Colorf.White;
            _fuzz = 0;
        }

        public MetalMaterial(Colorf albedo, float fuzz = 0)
        {
            _albedo = albedo;
            _fuzz = MathUtil.Clamp(fuzz);
        }

        public Colorf Albedo
        {
            get { return _albedo; }
            set { _albedo = value; }
        }
        private Colorf _albedo;

        public float Fuzz
        {
            get { return _fuzz; }
            set { _fuzz = value; }
        }
        private float _fuzz;

        public override bool Scatter(ref Ray3f ray, ref RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered, out float pdf)
        {
            var dir = ray.Direction;
            dir.Normalize();
            var reflected = Utilities.Reflect(dir, hitResult.Normal);
            if (!MathUtil.IsZero(_fuzz))
                reflected += _fuzz * Utilities.UniformSampleInSphere();
            scattered = new Ray3f(hitResult.HitPoint, reflected, ray.Time);
            attenuation = _albedo;
            pdf = 1;
            return Vector3f.DotProduct(reflected, hitResult.Normal) > 0;
        }
    }
}