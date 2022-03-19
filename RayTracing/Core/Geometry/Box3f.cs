using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public struct Box3f : IHittable
    {
        public Box3f(Point3f min, Point3f max)
        {
            _min = new Vector3f(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y), Math.Min(min.Z, max.Z));
            _max = new Vector3f(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y), Math.Max(min.Z, max.Z));
            _faces = new HittableList();
            _BuildFaces();
        }

        public Vector3f Max { get { return _max; } set { _max = value; } }
        private Vector3f _max;

        public Vector3f Min { get { return _min; } set { _min = value; } }
        private Vector3f _min;

        private HittableList _faces;

        private void _BuildFaces()
        {
            _faces.Add(new AxisAlignedRect(RectPlane.XY, new Vector2f(_min.X, _min.Y), new Vector2f(_max.X, _max.Y), _max.Z));
            _faces.Add(new AxisAlignedRect(RectPlane.XY, new Vector2f(_min.X, _min.Y), new Vector2f(_max.X, _max.Y), _min.Z, true));
            _faces.Add(new AxisAlignedRect(RectPlane.YZ, new Vector2f(_min.Y, _min.Z), new Vector2f(_max.Y, _max.Z), _max.X));
            _faces.Add(new AxisAlignedRect(RectPlane.YZ, new Vector2f(_min.Y, _min.Z), new Vector2f(_max.Y, _max.Z), _min.X, true));
            _faces.Add(new AxisAlignedRect(RectPlane.ZX, new Vector2f(_min.Z, _min.X), new Vector2f(_max.Z, _max.X), _max.Y));
            _faces.Add(new AxisAlignedRect(RectPlane.ZX, new Vector2f(_min.Z, _min.X), new Vector2f(_max.Z, _max.X), _min.Y, true));
        }

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            boundingBox = new AxisAlignedBox3f(_min, _max);
            return true;
        }

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            return _faces.HitWithRay(ref ray, out ret, minT, maxT);
        }
    }
}