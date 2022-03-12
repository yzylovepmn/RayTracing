﻿using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace RayTracing.Core
{
    public class MoveableSphere : Moveable, IHittable
    {
        public MoveableSphere()
        {

        }

        public MoveableSphere(float time1, float time2, Point3f center1, Point3f center2, Float radius) : base(time1, time2, center1, center2)
        {
            _radius = radius;
        }

        public Float Radius { get { return _radius; } set { _radius = value; } }
        private Float _radius;

        public bool HitWithRay(Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            var center = GetPosition(ray.Time);
            ret = new RayHitResult();
            var origin = ray.Origin - center;
            var originSquared = origin.LengthSquared;
            var rSquared = _radius * _radius;

            Float a = ray.Direction.LengthSquared;
            Float half_b = Vector3f.DotProduct(origin, ray.Direction);
            Float c = originSquared - rSquared;

            Float d = half_b * half_b - a * c;
            if (d < 0)
                return false;
            else
            {
                var sqd = (Float)Math.Sqrt(d);
                var t = (-half_b - sqd) / a;
                if (t < minT || t > maxT)
                {
                    t = (-half_b + sqd) / a;
                    if (t < minT || t > maxT)
                        return false;
                }
                ret.Hittable = this;
                ret.HitTime = t;
                ret.HitPoint = ray.GetPoint(t);
                ret.Normal = (ret.HitPoint - center) / _radius;
                ret.IsFrontFace = Vector3f.DotProduct(ret.Normal, ray.Direction) < 0;
                if (!ret.IsFrontFace)
                    ret.Normal.Negate();

                return true;
            }
        }
    }
}