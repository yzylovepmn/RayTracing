using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class LambertianMaterial : Material
    {
        public LambertianMaterial(Colorf albedo) : this(new SolidColorTexture(albedo))
        {
        }

        public LambertianMaterial(Texture albedo)
        {
            _albedo = albedo;
        }

        public Texture Albedo
        {
            get { return _albedo; }
            set { _albedo = value; }
        }
        private Texture _albedo;

        public override bool Scatter(ref Ray3f ray, ref RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered, out float pdf)
        {
            var scatteredDir = Utilities.CosineSampleOnHemisphere(ref hitResult.Normal);
            if (Utilities.IsZero(scatteredDir))
                scatteredDir = hitResult.Normal;
            scatteredDir.Normalize();
            scattered = new Ray3f(hitResult.HitPoint, scatteredDir, ray.Time);
            attenuation = _albedo.Sample(hitResult.U, hitResult.V, hitResult.HitPoint);
            pdf = Utilities.CosineOnHemispherePdf(Vector3f.DotProduct(scatteredDir, hitResult.Normal));
            return true;
        }

        public override float ScatteringPdf(ref Ray3f ray, ref RayHitResult hitResult, ref Ray3f scattered)
        {
            var scatteredDir = scattered.Direction;
            scatteredDir.Normalize();
            var c = Vector3f.DotProduct(hitResult.Normal, scatteredDir);
            return c < 0 ? 0 : Utilities.CosineOnHemispherePdf(c);
        }
    }
}