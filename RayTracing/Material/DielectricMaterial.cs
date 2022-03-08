using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class DielectricMaterial : Material
    {
        public DielectricMaterial(float indexOfRefraction)
        {
            _indexOfRefraction = indexOfRefraction;
        }

        public float IndexOfRefraction
        {
            get { return _indexOfRefraction; }
            set { _indexOfRefraction = value; }
        }
        private float _indexOfRefraction;

        public override bool Scatter(Ray3f ray, RayHitResult hitResult, out Colorf attenuation, out Ray3f scattered)
        {
            attenuation = Colorf.White;
            var refractRadio = hitResult.IsFrontFace ? 1 / _indexOfRefraction : _indexOfRefraction;
            var dir_norm = ray.Direction;
            dir_norm.Normalize();
            var c = Math.Min(Vector3f.DotProduct(-dir_norm, hitResult.Normal), 1);
            var sSquared = 1 - c * c;
            var s = Math.Sqrt(sSquared);
            var cannotRefract = refractRadio * s > 1;
            Vector3f dir;
            if (cannotRefract || Utilities.Reflectance(c, refractRadio) > Utilities.RandomDouble())
                dir = Utilities.Reflect(dir_norm, hitResult.Normal);
            else dir = Utilities.Refract(dir_norm, hitResult.Normal, c, sSquared, refractRadio);

            scattered = new Ray3f(hitResult.HitPoint, dir);
            return true;
        }
    }
}