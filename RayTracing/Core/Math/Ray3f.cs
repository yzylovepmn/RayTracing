using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    public struct Ray3f
    {
        public Ray3f(Point3f origin, Vector3f direction, Float time = 0)
        {
            _origin = origin;
            _direction = direction;
            _time = time;
        }

        public Point3f Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        private Point3f _origin;

        public Vector3f Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private Vector3f _direction;

        public Float Time
        {
            get { return _time; }
            set { _time = value; }
        }
        private Float _time;

        public Point3f GetPoint(Float time)
        {
            return _origin + _direction * time;
        }

        public bool IntersectWith(IHittable hittable, out RayHitResult ret, Float minT = 0, Float maxT = Float.MaxValue)
        {
            return hittable.HitWithRay(ref this, out ret, minT, maxT);
        }
    }
}