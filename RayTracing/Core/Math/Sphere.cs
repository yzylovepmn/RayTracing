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
            _bounds = AxisAlignedBox3f.Empty;
            _needUpdateBounds = true;
        }

        public Sphere(Float radius, Point3f center)
        {
            _radius = radius;
            _center = center;
            _bounds = AxisAlignedBox3f.Empty;
            _needUpdateBounds = true;
        }

        public Float Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    _needUpdateBounds = true;
                }
            }
        }
        private Float _radius;

        public Point3f Center
        {
            get { return _center; }
            set
            {
                if (_center != value)
                {
                    _center = value;
                    _needUpdateBounds = true;
                }
            }
        }
        private Point3f _center;

        private AxisAlignedBox3f _bounds;
        private bool _needUpdateBounds;

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
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

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            if (_needUpdateBounds)
                _UpdateBounds();
            boundingBox = _bounds;
            return true;
        }

        private void _UpdateBounds()
        {
            var vec = new Vector3f(_radius, _radius, _radius);
            _bounds = new AxisAlignedBox3f(_center - vec, _center + vec);
            _needUpdateBounds = false;
        }
    }
}