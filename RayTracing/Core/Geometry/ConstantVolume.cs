using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public struct ConstantVolume : IHittable
    {
        public ConstantVolume(IHittable boundary, float density)
        {
            _boundary = boundary;
            _density = density;
            _negInvDensity = -1 / _density;
        }

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            return _boundary.GetBoundingBox(out boundingBox);
        }

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();

            RayHitResult ret1, ret2;

            if (!_boundary.HitWithRay(ref ray, out ret1, float.NegativeInfinity, float.PositiveInfinity))
                return false;

            if (!_boundary.HitWithRay(ref ray, out ret2, ret1.HitTime + 0.0001f, float.PositiveInfinity))
                return false;

            if (ret1.HitTime < minT)
                ret1.HitTime = minT;
            if (ret2.HitTime > maxT)
                ret2.HitTime = maxT;

            if (ret1.HitTime >= ret2.HitTime)
                return false;

            if (ret1.HitTime < 0)
                ret1.HitTime = 0;

            var rayLength = ray.Direction.Length;
            var distanceInBoundary = (ret2.HitTime - ret1.HitTime) * rayLength;
            var hitDistance = _negInvDensity * (float)Math.Log(Utilities.RandomDouble());
            if (hitDistance > distanceInBoundary)
                return false;

            ret.HitTime = ret1.HitTime + hitDistance / rayLength;
            ret.HitPoint = ray.GetPoint(ret.HitTime);
            ret.Normal = Utilities.RandomVectorOnUnitSphere();
            ret.IsFrontFace = Utilities.RandomFloat() > 0.5f;
            ret.Hittable = this;

            return true;
        }

        public IHittable Boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }
        private IHittable _boundary;

        public float Density
        {
            get { return _density; }
            set
            {
                if (_density != value)
                {
                    _density = value;
                    _negInvDensity = -1 / _density;
                }
            }
        }
        private float _density;
        private float _negInvDensity;
    }
}