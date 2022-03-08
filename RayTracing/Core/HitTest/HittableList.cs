using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public class HittableList : IHittable
    {
        public HittableList()
        {
            _hittables = new List<IHittable>();
        }

        public HittableList(IEnumerable<IHittable> hittables)
        {
            _hittables = new List<IHittable>(hittables);
        }

        public IReadOnlyList<IHittable> Hittables { get { return _hittables; } }
        private List<IHittable> _hittables;

        public void AddHittable(IHittable hittable)
        {
            _hittables.Add(hittable);
        }

        public void RemoveHittable(IHittable hittable)
        {
            _hittables.Remove(hittable);
        }

        public void ClearHittables()
        {
            _hittables.Clear();
        }

        public bool HitWithRay(Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            var closestHitTime = maxT;
            var isHitted = false;

            foreach (var hittable in _hittables)
            {
                RayHitResult temp;
                if (hittable.HitWithRay(ray, out temp, minT, closestHitTime))
                {
                    isHitted = true;
                    closestHitTime = temp.HitTime;
                    ret = temp;
                }
            }

            return isHitted;
        }
    }
}