using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace RayTracing.Core
{
    public struct RayHitResult
    {
        public RayHitResult(IHittable hittable, Float hitTime, Point3f hitPoint, Vector3f normal, float u, float v, bool isFrontFace)
        {
            Hittable = hittable;
            HitTime = hitTime;
            HitPoint = hitPoint;
            Normal = normal;
            U = u;
            V = v;
            IsFrontFace = isFrontFace;

            Normal.Normalize();
        }

        public IHittable Hittable;
        public Float HitTime;
        public Point3f HitPoint;
        public Vector3f Normal;
        public float U, V;
        public bool IsFrontFace;
    }
}