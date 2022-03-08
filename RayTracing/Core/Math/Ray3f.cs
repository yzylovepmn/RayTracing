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
        public Ray3f(Point3f origin, Vector3f direction)
        {
            _origin = origin;
            _direction = direction;
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

        public Point3f GetPoint(Float time)
        {
            return _origin + _direction * time;
        }

        public bool IntersectWith(IHittable hittable, out RayHitResult ret, Float minT = 0, Float maxT = Float.MaxValue)
        {
            return hittable.HitWithRay(this, out ret, minT, maxT);
        }
    }
}