using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    /// <summary>
    /// Support Transform
    /// </summary>
    public class TransformedSceneObject : SceneObject
    {
        public TransformedSceneObject(IHittable mesh, Material material) : base(mesh, material)
        {
            _needUpdateMatrixTransform = true;
            _position = Point3f.Origin;
            _scale = Vector3f.Unit;
            _orientation = Quaternionf.Identity;
            _orientationInverse = Quaternionf.Identity;
        }

        public Point3f Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _needUpdateMatrixTransform = true;
                }
            }
        }
        private Point3f _position;

        public Vector3f Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    _needUpdateMatrixTransform = true;
                }
            }
        }
        private Vector3f _scale;

        public Quaternionf Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    _orientationInverse = _orientation.Inverse;
                    _needUpdateMatrixTransform = true;
                }
            }
        }
        private Quaternionf _orientation;
        private Quaternionf _orientationInverse;

        public Matrix3f Matrix
        {
            get
            {
                if (_needUpdateMatrixTransform)
                    _UpdateMatrixTransform();
                return _matrix;
            }
        }
        private Matrix3f _matrix;

        bool _needUpdateMatrixTransform;

        public Vector3f ToLocal(Vector3f worldDir, bool useScale)
        {
            var ret = worldDir * _orientationInverse;
            if (useScale)
                ret /= _scale;
            return ret;
        }

        public Point3f ToLocal(Point3f worldPos)
        {
            return (Point3f)((worldPos - _position) * _orientationInverse / _scale);
        }

        public Quaternionf ToLocal(Quaternionf worldOrientation)
        {
            return worldOrientation * _orientationInverse;
        }

        public Ray3f ToLocal(Ray3f rayInWorld)
        {
            var p = _ToLocal(rayInWorld.Origin, _orientationInverse);
            var d = _ToLocal(rayInWorld.Direction, _orientationInverse, true);
            return new Ray3f(p, d, rayInWorld.Time);
        }

        private Point3f _ToLocal(Point3f worldPos, Quaternionf worldOrientationInverse)
        {
            return (Point3f)((worldPos - _position) * worldOrientationInverse / _scale);
        }

        private Vector3f _ToLocal(Vector3f worldDir, Quaternionf worldOrientationInverse, bool useScale)
        {
            var ret = worldDir * worldOrientationInverse;
            if (useScale)
                ret /= _scale;
            return ret;
        }

        public Vector3f ToWorld(Vector3f localDir, bool useScale)
        {
            return useScale ? localDir * Matrix : localDir * _orientation;
        }

        public Point3f ToWorld(Point3f localPos)
        {
            return localPos * Matrix;
        }

        public Quaternionf ToWorld(Quaternionf localOrientation)
        {
            return localOrientation * _orientation;
        }

        private void _UpdateMatrixTransform()
        {
            _matrix = MatrixUtil.CreateMatrix(_position, _scale, _orientation);

            _needUpdateMatrixTransform = false;
        }

        public override bool HitWithRay(ref Ray3f ray, out RayHitResult ret, float minT = 0, float maxT = float.MaxValue)
        {
            ret = new RayHitResult();
            if (_mesh == null)
                return false;
            var localRay = ToLocal(ray);
            var success = _mesh.HitWithRay(ref localRay, out ret, minT, maxT);
            ret.HitPoint = ToWorld(ret.HitPoint);
            ret.Normal = ToWorld(ret.Normal, true);
            // override
            if (!(ret.Hittable is SceneObject))
                ret.Hittable = this;
            return success;
        }

        public override bool GetBoundingBox(out AxisAlignedBox3f boundingBox)
        {
            if (base.GetBoundingBox(out boundingBox))
            {
                boundingBox *= Matrix;
                return true;
            }
            return false;
        }
    }
}