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

        public event EventHandler ListChanged;

        public IReadOnlyList<IHittable> Hittables { get { return _hittables; } }
        private List<IHittable> _hittables;

        private AxisAlignedBox3f _bounds;
        private bool _hasBounds = true;
        private bool _needUpdateBounds = true;

        internal void Add(IHittable hittable)
        {
            _hittables.Add(hittable);
            _needUpdateBounds = true;
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void Remove(IHittable hittable)
        {
            _hittables.Remove(hittable);
            _needUpdateBounds = true;
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void Clear()
        {
            _hittables.Clear();
            _needUpdateBounds = true;
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            var closestHitTime = maxT;
            var isHitted = false;

            foreach (var hittable in _hittables)
            {
                RayHitResult temp;
                if (hittable.HitWithRay(ref ray, out temp, minT, closestHitTime))
                {
                    isHitted = true;
                    closestHitTime = temp.HitTime;
                    ret = temp;
                }
            }

            return isHitted;
        }

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            if (_needUpdateBounds)
                _UpdateBounds();
            boundingBox = _bounds;
            return _hasBounds;
        }

        private void _UpdateBounds()
        {
            _hasBounds = true;
            _bounds = AxisAlignedBox3f.Empty;
            foreach (var hittable in _hittables)
            {
                AxisAlignedBox3f tempBox;
                if (!hittable.GetBoundingBox(out tempBox))
                {
                    _hasBounds = false;
                    return;
                }
                _bounds.Union(tempBox);
            }
        }
    }
}