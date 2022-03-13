using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace RayTracing.Core
{
    public interface IHittable
    {
        /// <summary>
        /// Do not modify ray,It's just to reduce copy overhead
        /// </summary>
        bool HitWithRay(ref Ray3f ray, out RayHitResult ret, Float minT = 0, Float maxT = Float.MaxValue);

        bool GetBoundingBox(out AxisAlignedBox3f boundingBox);
    }
}