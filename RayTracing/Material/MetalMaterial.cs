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

        }

        public MetalMaterial(Colorf albedo)
        {
            _albedo = albedo;
        }

        public Colorf Albedo
        {
            get { return _albedo; }
            set { _albedo = value; }
        }
        private Colorf _albedo;

        public override bool Scatter(Ray3f ray, RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered)
        {
            var dir = ray.Direction;
            dir.Normalize();
            var reflected = Utilities.Reflect(dir, hitResult.Normal);
            scattered = new Ray3f(hitResult.HitPoint, reflected);
            attenuation = _albedo;
            return true;
        }
    }
}