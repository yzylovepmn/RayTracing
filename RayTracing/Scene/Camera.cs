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
            _height = 2;
            _focalLength = 1;
            _origin = Point3f.Origin;
            _Update();
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

        public float FocalLength
        {
            get { return _focalLength; }
            set { _focalLength = value; }
        }
        private float _focalLength;

        public Point3f Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        private Point3f _origin;

        private Vector3f _horizontal;
        private Vector3f _vertical;
        private Point3f _lowerLeftCorner;

        private void _Update()
        {
            _horizontal = new Vector3f(Width, 0, 0);
            _vertical = new Vector3f(0, Height, 0);
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - new Vector3f(0, 0, _focalLength);
        }

        public Ray3f GetRay(float u, float v)
        {
            return new Ray3f(_origin, _lowerLeftCorner + u * _horizontal + v * _vertical - _origin);
        }
    }
}