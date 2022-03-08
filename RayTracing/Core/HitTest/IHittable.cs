using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace RayTracing.Core
{
    public interface IHittable
    {
        bool HitWithRay(Ray3f ray, out RayHitResult ret, Float minT = 0, Float maxT = Float.MaxValue);
    }
}