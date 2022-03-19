using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class IsotropicMaterial : Material
    {
        public IsotropicMaterial(Colorf albedo) : this(new SolidColorTexture(albedo))
        {
        }

        public IsotropicMaterial(Texture albedo)
        {
            _albedo = albedo;
        }

        public Texture Albedo
        {
            get { return _albedo; }
            set { _albedo = value; }
        }
        private Texture _albedo;

        public override bool Scatter(Ray3f ray, RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered)
        {
            scattered = new Ray3f(hitResult.HitPoint, Utilities.RandomVectorInUnitSphere(), ray.Time);
            attenuation = _albedo.Sample(hitResult.U, hitResult.V, hitResult.HitPoint);
            return true;
        }
    }
}