using System;
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

        public override Point3f Position
        {
            get { return base.Position; }
            set
            {
                if (base.Position != value)
                {
                    base.Position = value;
                    _needUpdateBounds = true;
                }
            }
        }

        public override Point3f Target
        {
            get { return base.Target; }
            set
            {
                if (base.Target != value)
                {
                    base.Target = value;
                    _needUpdateBounds = true;
                }
            }
        }

        public override Float Time1
        {
            get { return base.Time1; }
            set
            {
                if (base.Time1 != value)
                {
                    base.Time1 = value;
                    _needUpdateBounds = true;
                }
            }
        }

        public override Float Time2
        {
            get { return base.Time2; }
            set
            {
                if (base.Time2 != value)
                {
                    base.Time2 = value;
                    _needUpdateBounds = true;
                }
            }
        }

        private AxisAlignedBox3f _bounds;
        private bool _needUpdateBounds = true;

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
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
                Utilities.ComputeUVOfSphere((Point3f)ret.Normal, out ret.U, out ret.V);

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
            var c1 = GetPosition(_time1);
            var c2 = GetPosition(_time2);
            var vec = new Vector3f(_radius, _radius, _radius);
            var box1 = new AxisAlignedBox3f(c1 - vec, c1 + vec);
            _bounds = new AxisAlignedBox3f(c2 - vec, c2 + vec);
            _bounds.Union(box1);
        }
    }
}