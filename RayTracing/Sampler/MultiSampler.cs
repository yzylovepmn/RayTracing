using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class MultiSampler : Sampler
    {
        public MultiSampler(Pipeline pipeline) : base(pipeline)
        {
            _samplesPerPixel = 100;
        }

        public int SamplesPerPixel { get { return _samplesPerPixel; } }
        protected int _samplesPerPixel;

        public override Colorf Sample(int j, int i)
        {
            var ret = new Colorf(0f, 0f, 0f, 0f);
            for (int k = 0; k < _samplesPerPixel; k++)
            {
                var u = (float)(i + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Width;
                var v = (float)(j + 0.5f + Utilities.RandomDouble(-0.5, 0.5)) / _pipeline.Image.Height;
                var ray = _pipeline.Scene.Camera.GetRay(u, v);
                ret += RayColor(ray, _pipeline.MaxRayDepth);
            }
            ret *= 1f / _samplesPerPixel;
            return ret;
        }
    }
}