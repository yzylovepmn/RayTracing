using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Scene : IHittable
    {
        public Scene()
        {
            _camera = new Camera();
            _hittableList = new HittableList();
        }

        public Camera Camera { get { return _camera; } }
        protected Camera _camera;

        public HittableList HittableList { get { return _hittableList; } }
        protected HittableList _hittableList;

        public void AddHittable(IHittable hittable)
        {
            _hittableList.Add(hittable);
        }

        public void RemoveHittable(IHittable hittable)
        {
            _hittableList.Remove(hittable);
        }

        public void ClearScene()
        {
            _hittableList.Clear();
        }

        public virtual void BuildScene()
        {

        }

        public virtual bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            return _hittableList.HitWithRay(ref ray, out ret, minT, maxT);
        }

        public virtual bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            return _hittableList.GetBoundingBox(out boundingBox);
        }
    }
}