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
                Colorf emitted = meshObject.Material.Emitted(ret.U, ret.V, ret.HitPoint);
                if (meshObject.Material.Scatter(ray, ret, out attenuation, out scattered))
                    return emitted + attenuation * RayColor(scattered, background, depth - 1);
                return emitted;
            }

            return background;
        }
    }
}