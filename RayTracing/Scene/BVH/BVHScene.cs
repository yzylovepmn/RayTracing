using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class BVHScene : Scene
    {
        public BVHScene(int maxDepth = 8)
        {
            _maxDepth = maxDepth;
            _root = new BVHNode(this);
            _hittableList.ListChanged += _hittableListChanged;
        }

        private void _hittableListChanged(object sender, EventArgs e)
        {
            // TODO
        }

        public BVHNode Root { get { return _root; } }
        private BVHNode _root;

        public int MaxDepth { get { return _maxDepth; } }
        private int _maxDepth;

        public override void BuildScene()
        {
            _root.Build(_hittableList.Hittables, _maxDepth);
        }

        public override bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            return _root.HitWithRay(ref ray, out ret, minT, maxT);
        }
    }
}