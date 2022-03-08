using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public abstract class SamplerBase : ISampler
    {
        public SamplerBase(Pipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public Pipeline Pipeline { get { return _pipeline; } }
        protected Pipeline _pipeline;

        public abstract Colorf Sample(int j, int i);
    }
}