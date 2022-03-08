using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    public enum PlaneSide
    {
        Positive,
        Negative,
        OnPlane,
        BothSide
    }

    /// <summary>
    /// Ax + By + Cz + D = 0
    /// </summary>
    public struct Plane : IHittable
    {
        public Plane(Float a, Float b, Float c, Float d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _Normalize();
        }

        /// <summary>
        /// Indicate that A = v.X, B = v.Y, C = v.Z, D = v.W
        /// </summary>
        public Plane(Vector4f v)
        {
            _a = v._x;
            _b = v._y;
            _c = v._z;
            _d = v._w;
            _Normalize();
        }

        public Plane(Vector3f normal, Point3f point)
        {
            normal.Normalize();
            _d = -Vector3f.DotProduct((Vector3f)point, normal);
            _a = normal.X;
            _b = normal.Y;
            _c = normal.Z;
        }

        public Plane(Point3f p1, Point3f p2, Point3f p3) : this(MathUtil.CalcNormal(p1, p2, p3), p1)
        {

        }

        public Vector3f Normal { get { return new Vector3f(_a, _b, _c); } }

        public Float A { get { return _a; } }
        private Float _a;

        public Float B { get { return _b; } }
        private Float _b;

        public Float C { get { return _c; } }
        private Float _c;

        public Float D { get { return _d; } }
        private Float _d;

        private void _Normalize()
        {
            var len = Normal.Length;
            if (len > 0)
            {
                var invLen = 1 / len;
                _a *= invLen;
                _b *= invLen;
                _c *= invLen;
                _d *= invLen;
            }
        }

        public Float GetDistance(Point3f point)
        {
            return _a * point.X + _b * point.Y + _c * point.Z + _d;
        }

        public Float GetDistance(Vector3f point)
        {
            return GetDistance((Point3f)point);
        }

        public PlaneSide GetSide(Point3f point)
        {
            var dist = GetDistance(point);
            if (dist > 0)
                return PlaneSide.Positive;
            if (dist < 0)
                return PlaneSide.Negative;
            return PlaneSide.OnPlane;
        }

        public PlaneSide GetSide(Vector3f point)
        {
            return GetSide((Point3f)point);
        }

        public PlaneSide GetSide(AxisAlignedBox3f box)
        {
            if (box.IsEmpty)
                return PlaneSide.OnPlane;
            if (box.IsInfinite)
                return PlaneSide.BothSide;
            return GetSide(box.Center, box.Extents);
        }

        public PlaneSide GetSide(Point3f center, Vector3f halfSize)
        {
            var dist = GetDistance(center);

            var maxDist = Vector3f.AbsDotProduct(Normal, halfSize);

            if (dist > maxDist)
                return PlaneSide.Positive;
            if (dist < -maxDist)
                return PlaneSide.Negative;
            return PlaneSide.BothSide;
        }

        /// <summary>
        /// Project a vector onto the plane.
        /// </summary>
        public Vector3f ProjectVector(Vector3f v)
        {
            var f = Vector3f.DotProduct(v, Normal);
            return v - f * Normal;
        }

        public bool HitWithRay(Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            var norm = Vector3f.DotProduct(ray.Direction, Normal);
            if (MathUtil.IsZero(norm))
                return false;
            else
            {
                var dist = GetDistance(ray.Origin);
                var time = -dist / norm;
                if (time < minT || time > maxT)
                    return false;
                ret.Hittable = this;
                ret.HitTime = time;
                ret.HitPoint = ray.GetPoint(time);
                ret.IsFrontFace = norm < 0;
                ret.Normal = ret.IsFrontFace ? Normal : -Normal;

                return true;
            }
        }
    }
}