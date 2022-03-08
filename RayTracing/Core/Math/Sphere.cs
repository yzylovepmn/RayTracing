using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    public struct Sphere : IHittable
    {
        public Sphere(Float radius = 1)
        {
            _radius = radius;
            _center = Point3f.Origin;
        }

        public Sphere(Float radius, Point3f center)
        {
            _radius = radius;
            _center = center;
        }

        public Float Radius { get { return _radius; } set { _radius = value; } }
        private Float _radius;

        public Point3f Center { get { return _center; } set { _center = value; } }
        private Point3f _center;

        public bool HitWithRay(Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            var origin = ray.Origin - _center;
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
                ret.Normal = (ret.HitPoint - _center) / _radius;
                ret.IsFrontFace = Vector3f.DotProduct(ret.Normal, ray.Direction) < 0;
                if (!ret.IsFrontFace)
                    ret.Normal.Negate();

                return true;
            }
        }
    }
}