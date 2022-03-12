using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public abstract class Moveable
    {
        public Moveable()
        {

        }

        public Moveable(float time1, float time2, Point3f position, Point3f target)
        {
            _time1 = time1;
            _time2 = time2;
            _position = position;
            _target = target;
        }

        public float Time1
        {
            get { return _time1; }
            set { _time1 = Math.Min(value, _time2); }
        }
        protected float _time1;

        public float Time2
        {
            get { return _time2; }
            set { _time2 = Math.Max(value, _time1); }
        }
        protected float _time2;

        public Point3f Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Point3f _position;

        public Point3f Target
        {
            get { return _target; }
            set { _target = value; }
        }
        private Point3f _target;

        public Point3f GetPosition(float time)
        {
            return _position + (time - _time1) / (_time2 - _time1) * (_target - _position);
        }
    }
}