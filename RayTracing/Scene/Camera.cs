using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Camera
    {
        public Camera()
        {
            _aspect = 16 / 9.0f;
            _fovy = 90;
            _height = 2;
            _aperture = 2;
            _nearPlane = 1;
            _position = Point3f.Origin;
            _lookDirection = -Vector3f.ZAxis;
            _upDirection = Vector3f.YAxis;

            Update();
        }

        public float Aspect
        {
            get { return _aspect; }
            set { _aspect = value; }
        }
        private float _aspect;

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private float _height;

        public float Width
        {
            get { return _height * _aspect; }
            set { _height = value / _aspect; }
        }

        public float Fovy
        {
            get { return _fovy; }
            set { _fovy = value; }
        }
        private float _fovy;

        public float NearPlane
        {
            get { return _nearPlane; }
            set { _nearPlane = value; }
        }
        private float _nearPlane;

        public float Aperture
        {
            get { return _aperture; }
            set { _aperture = value; }
        }
        private float _aperture;

        public Point3f Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Point3f _position;

        public Vector3f LookDirection
        {
            get { return _lookDirection; }
            set { _lookDirection = value; }
        }
        private Vector3f _lookDirection;

        public Vector3f UpDirection
        {
            get { return _upDirection; }
            set { _upDirection = value; }
        }
        private Vector3f _upDirection;

        private Vector3f _horizontal;
        private Vector3f _vertical;
        private Vector3f _x;
        private Vector3f _y;
        private Vector3f _z;
        private Point3f _lowerLeftCorner;

        public void Update()
        {
            var theta = MathUtil.DegreesToRadians(_fovy);
            _height = (float)Math.Tan(theta / 2) * _nearPlane * 2;

            _z = -_lookDirection;
            _z.Normalize();
            _x = Vector3f.CrossProduct(_upDirection, _z);
            _x.Normalize();
            _y = Vector3f.CrossProduct(_z, _x);

            _horizontal = _x * Width;
            _vertical = _y * Height;
            _lowerLeftCorner = _position - _horizontal / 2 - _vertical / 2 - _z * _nearPlane;
        }

        public Ray3f GetRay(float u, float v)
        {
            var rd = _aperture * Utilities.RandomVectorInUnitCicle() / 2;
            var offset = _x * rd.X + _y * rd.Y;
            var position = _position + offset;
            return new Ray3f(position, _lowerLeftCorner + u * _horizontal + v * _vertical - position);
        }
    }
}