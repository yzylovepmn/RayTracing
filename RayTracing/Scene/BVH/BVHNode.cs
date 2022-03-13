using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class BVHNode : IHittable
    {
        public BVHNode(BVHScene scene, BVHNode parent = null)
        {
            _scene = scene;
            _parent = parent;
        }

        public BVHScene Scene { get { return _scene; } }
        private BVHScene _scene;

        public BVHNode Parent { get { return _parent; } }
        private BVHNode _parent;

        public BVHNode Left { get { return _left; } }
        private BVHNode _left;

        public BVHNode Right { get { return _right; } }
        private BVHNode _right;

        private HittableList _hittableList;

        public bool IsRoot { get { return _scene.Root == this; } }

        private AxisAlignedBox3f _bounds;

        public void Build(IReadOnlyList<IHittable> Hittables, int depth)
        {
            var hittableList = new HittableList(Hittables);
            if (depth != 0 && hittableList.Hittables.Count > 1 && hittableList.GetBoundingBox(out _bounds))
            {
                var radio = 0.1f;
                float maxDimension;
                var diagonal = _bounds.Diagonal;
                var maxDimensionIndex = _SelectMaxDimensionIndex(ref diagonal, out maxDimension);
                var leftBound = _bounds.Min[maxDimensionIndex];
                var rightBound = _bounds.Max[maxDimensionIndex];
                var mid = (leftBound + rightBound) * 0.5f;
                var radioLength = radio * maxDimension;

                var leftLimit = mid - radioLength;
                var rightLimit = mid + radioLength;

                var listRemains = new List<IHittable>();
                var leftList = new List<IHittable>();
                var rightList = new List<IHittable>();

                foreach (var hittable in hittableList.Hittables)
                {
                    AxisAlignedBox3f localBounds;
                    hittable.GetBoundingBox(out localBounds);
                    var flag = false;
                    var localLeftBound = localBounds.Min[maxDimensionIndex];
                    var localRightBound = localBounds.Max[maxDimensionIndex];
                    if (localLeftBound >= leftLimit)
                    {
                        flag = true;
                        rightList.Add(hittable);
                    }
                    else if (localRightBound <= rightLimit)
                    {
                        flag = true;
                        leftList.Add(hittable);
                    }
                    if (!flag)
                        listRemains.Add(hittable);
                }

                if (listRemains.Count > 0)
                    _hittableList = new HittableList(listRemains);
                if (leftList.Count > 0)
                {
                    _left = new BVHNode(_scene, this);
                    _left.Build(leftList, depth - 1);
                }
                if (rightList.Count > 0)
                {
                    _right = new BVHNode(_scene, this);
                    _right.Build(rightList, depth - 1);
                }
            }
            else
            {
                _hittableList = hittableList;
                hittableList.GetBoundingBox(out _bounds);
            }
        }

        public bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            if (!_bounds.HitWithRay(ref ray, minT, maxT)) return false;

            RayHitResult temp = new RayHitResult();
            var hitted = _hittableList == null ? false : _hittableList.HitWithRay(ref ray, out temp, minT, maxT);
            if (hitted)
                ret = temp;
            if (_left != null && _left.HitWithRay(ref ray, out temp, minT, hitted ? ret.HitTime : maxT))
            {
                hitted |= true;
                ret = temp;
            }
            if (_right != null && _right.HitWithRay(ref ray, out temp, minT, hitted ? ret.HitTime : maxT))
            {
                hitted |= true;
                ret = temp;
            }
            return hitted;
        }

        public bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            boundingBox = _bounds;
            return true;
        }

        private int _SelectMaxDimensionIndex(ref Vector3f diagonal, out float maxDimension)
        {
            var index = 0;
            maxDimension = diagonal.X;
            if (diagonal.Y > maxDimension)
            {
                maxDimension = diagonal.Y;
                index = 1;
            }
            if (diagonal.Z > maxDimension)
            {
                maxDimension = diagonal.Z;
                index = 2;
            }
            return index;
        }
    }
}