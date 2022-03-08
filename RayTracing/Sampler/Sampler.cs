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

            return RayColor(ray, _pipeline.MaxRayDepth);
        }

        protected virtual Colorf RayColor(Ray3f ray, int depth)
        {
            if (depth <= 0)
                return Colorf.Black;

            RayHitResult ret;
            if (_pipeline.Scene.HittableList.HitWithRay(ray, out ret, 1e-3f))
            {
                var meshObject = ret.Hittable as MeshObject;
                Colorf attenuation;
                Ray3f scattered;
                if (meshObject.Material.Scatter(ray, ret, out attenuation, out scattered))
                    return attenuation * RayColor(scattered, depth - 1);
                return Colorf.Black;
            }

            var dir_norm = ray.Direction;
            dir_norm.Normalize();
            var t = 0.5f * (dir_norm.Y + 1);
            return (1 - t) * Colorf.White + t * new Colorf(0.5f, 0.7f, 1);
        }
    }
}