using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public abstract class NoiseBase : INoise
    {
        public abstract float Noise(Point3f p);

        public virtual float Turb(Point3f p, int depth = 7)
        {
            var value = 0f;
            var weight = 1f;
            var tp = p;
            for (int i = 0; i < depth; i++)
            {
                value += weight * Noise(tp);
                tp *= 2;
                weight *= 0.5f;
            }
            return Math.Abs(value);
        }
    }
}