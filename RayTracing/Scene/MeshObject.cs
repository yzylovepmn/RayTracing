using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class MeshObject : IHittable
    {
        public MeshObject()
        {

        }

        public MeshObject(IHittable mesh, Material material)
        {
            _mesh = mesh;
            _material = material;
        }

        public IHittable Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }
        private IHittable _mesh;

        public Material Material
        {
            get { return _material; }
            set { _material = value; }
        }
        private Material _material;

        public bool HitWithRay(Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            if (_mesh == null)
                return false;
            var success = _mesh.HitWithRay(ray, out ret, minT, maxT);
            // override
            ret.Hittable = this;
            return success;
        }
    }
}