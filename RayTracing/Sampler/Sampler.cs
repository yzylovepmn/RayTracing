using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Sampler : SamplerBase
    {
        public Sampler(Pipeline pipeline) : base(pipeline)
        {
        }

        public override Colorf Sample(int j, int i)
        {
            var u = (float)(i + 0.5f) / _pipeline.Image.Width;
            var v = (float)(j + 0.5f) / _pipeline.Image.Height;
            var ray = _pipeline.Scene.Camera.GetRay(u, v);

            return RayColor(ray, _pipeline.Background, _pipeline.MaxRayDepth);
        }

        protected virtual Colorf RayColor(Ray3f ray, Colorf background, int depth)
        {
            if (depth <= 0)
                return Colorf.Black;

            RayHitResult ret;
            if (_pipeline.Scene.HitWithRay(ref ray, out ret, 1e-3f))
            {
                var meshObject = ret.Hittable as SceneObject;
                Ray3f scattered;
                Colorf attenuation;
                float pdf;
                Colorf emitted = meshObject.Material.Emitted(ref ret);

                if (meshObject.Material.Scatter(ref ray, ref ret, out attenuation, out scattered, out pdf))
                {
                    //var onLight = new Point3f(Utilities.RandomFloat(123, 423), 554, Utilities.RandomFloat(147, 412));
                    //var toLight = onLight - ret.HitPoint;
                    //var dSquared = toLight.LengthSquared;
                    //toLight.Normalize();
                    //if (Vector3f.DotProduct(toLight, ret.Normal) < 0)
                    //    return emitted;
                    //var area = 79500;
                    //var lightCos = Math.Abs(toLight.Y);
                    //if (lightCos < 1e-6)
                    //    return emitted;
                    //pdf = dSquared / (lightCos * area);
                    //scattered = new Ray3f(ret.HitPoint, toLight, ret.HitTime);
                    return emitted + attenuation * (meshObject.Material.ScatteringPdf(ref ray, ref ret, ref scattered) / pdf) * RayColor(scattered, background, depth - 1);
                }
                return emitted;
            }

            return background;
        }
    }
}