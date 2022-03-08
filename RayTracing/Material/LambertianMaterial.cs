using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class LambertianMaterial : Material
    {
        public LambertianMaterial()
        {

        }

        public LambertianMaterial(Colorf albedo)
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
            var scatteredDir = hitResult.Normal + Utilities.RandomVectorOnUnitSphere();
            if (Utilities.IsZero(scatteredDir))
                scatteredDir = hitResult.Normal;
            scattered = new Ray3f(hitResult.HitPoint, scatteredDir);
            attenuation = _albedo;
            return true;
        }
    }
}