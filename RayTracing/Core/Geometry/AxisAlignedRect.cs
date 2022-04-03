using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public enum RectPlane
    {
        XY,
        YZ,
        ZX
    }

    public struct AxisAlignedRect : IHittable
    {
        public AxisAlignedRect(RectPlane plane, Vector2f min, Vector2f max, float value, bool negativeNormal = false)
        {
            _plane = plane;
            _min = min;
            _max = max;
            _value = value;
            _negativeNormal = negativeNormal;
        }

        public RectPlane Plane { get { return _plane; } }
        private RectPlane _plane;

        public Vector2f Min { get { return _min; } }
        private Vector2f _min;

        public Vector2f Max { get { return _max; } }
        private Vector2f _max;

        public float Value { get { return _value; } }
        private float _value;

        public bool NegativeNormal { get { return _negativeNormal; } }
        private bool _negativeNormal;

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            boundingBox = new AxisAlignedBox3f();
            switch (_plane)
            {
                case RectPlane.XY:
                    if (_negativeNormal)
                        boundingBox = new AxisAlignedBox3f(new Vector3f(_min.X, _min.Y, _value), new Vector3f(_max.X, _max.Y, _value + 0.0001f));
                    else boundingBox = new AxisAlignedBox3f(new Vector3f(_min.X, _min.Y, _value - 0.0001f), new Vector3f(_max.X, _max.Y, _value));
                    break;
                case RectPlane.YZ:
                    if (_negativeNormal)
                        boundingBox = new AxisAlignedBox3f(new Vector3f(_value, _min.X, _min.Y), new Vector3f(_value + 0.0001f, _max.X, _max.Y));
                    else boundingBox = new AxisAlignedBox3f(new Vector3f(_value - 0.0001f, _min.X, _min.Y), new Vector3f(_value, _max.X, _max.Y));
                    break;
                case RectPlane.ZX:
                    if (_negativeNormal)
                        boundingBox = new AxisAlignedBox3f(new Vector3f(_min.Y, _value, _min.X), new Vector3f(_max.Y, _value + 0.0001f, _max.X));
                    else boundingBox = new AxisAlignedBox3f(new Vector3f(_min.Y, _value - 0.0001f, _min.X), new Vector3f(_max.Y, _value, _max.X));
                    break;
            }
            return true;
        }

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            float t = 0;
            switch (_plane)
            {
                case RectPlane.XY:
                    t = (_value - ray.Origin.Z) / ray.Direction.Z;
                    break;
                case RectPlane.YZ:
                    t = (_value - ray.Origin.X) / ray.Direction.X;
                    break;
                case RectPlane.ZX:
                    t = (_value - ray.Origin.Y) / ray.Direction.Y;
                    break;
            }
            if (t < minT || t > maxT || float.IsNaN(t)) // float.IsNaN(t) means the ray is parallel to surface
                return false;

            var hitPoint = ray.GetPoint(t);
            float p1 = 0, p2 = 0;
            switch (_plane)
            {
                case RectPlane.XY:
                    p1 = hitPoint.X;
                    p2 = hitPoint.Y;
                    break;
                case RectPlane.YZ:
                    p1 = hitPoint.Y;
                    p2 = hitPoint.Z;
                    break;
                case RectPlane.ZX:
                    p1 = hitPoint.Z;
                    p2 = hitPoint.X;
                    break;
            }
            if (p1 < _min.X || p1 > _max.X || p2 < _min.Y || p2 > _max.Y)
                return false;

            ret.U = (p1 - _min.X) / (_max.X - _min.X);
            ret.V = (p2 - _min.Y) / (_max.Y - _min.Y);
            ret.HitTime = t;
            Vector3f normal = Vector3f.Unit;
            switch (_plane)
            {
                case RectPlane.XY:
                    normal = _negativeNormal ? -Vector3f.ZAxis : Vector3f.ZAxis;
                    break;
                case RectPlane.YZ:
                    normal = _negativeNormal ? -Vector3f.XAxis : Vector3f.XAxis;
                    break;
                case RectPlane.ZX:
                    normal = _negativeNormal ? -Vector3f.YAxis : Vector3f.YAxis;
                    break;
            }
            ret.Normal = normal;
            ret.IsFrontFace = Vector3f.DotProduct(ret.Normal, ray.Direction) < 0;
            if (!ret.IsFrontFace)
                ret.Normal.Negate();
            ret.HitPoint = hitPoint;
            ret.Hittable = this;

            return true;
        }
    }
}