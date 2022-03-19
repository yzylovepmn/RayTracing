using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class MultiSampler : Sampler
    {
        public MultiSampler(Pipeline pipeline, int samplesPerPixel = 100) : base(pipeline)
        {
            _samplesPerPixel = samplesPerPixel;
        }

        public int SamplesPerPixel { get { return _samplesPerPixel; } }
        protected int _samplesPerPixel;

        public override Colorf Sample(int j, int i)
        {
            var ret = Colorf.TransparentBlack;
            //Parallel.For(0, _samplesPerPixel, (k) =>
            //{
            //    var u = (float)(i + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Width;
            //    var v = (float)(j + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Height;
            //    var ray = _pipeline.Scene.Camera.GetRay(u, v);
            //    ret += RayColor(ray, _pipeline.Background, _pipeline.MaxRayDepth);
            //});
            for (int k = 0; k < _samplesPerPixel; k++)
            {
                var u = (float)(i + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Width;
                var v = (float)(j + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Height;
                var ray = _pipeline.Scene.Camera.GetRay(u, v);
                ret += RayColor(ray, _pipeline.Background, _pipeline.MaxRayDepth);
            }
            ret *= 1f / _samplesPerPixel;
            return ret;
        }
    }
}