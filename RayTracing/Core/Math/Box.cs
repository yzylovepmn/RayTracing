using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Core
{
    /// <summary>
    /// Define a box with origin (Left, Top, Front)
    /// </summary>
    public interface IBox : IEquatable<IBox>
    {
        uint Left { get; set; }

        uint Right { get; set; }

        uint Top { get; set; }

        uint Bottom { get; set; }

        uint Front { get; set; }

        uint Back { get; set; }

        uint Width { get; }

        uint Height { get; }

        uint Depth { get; }

        Point3i Origin { get; }

        Vector3i Size { get; }

        void SetBox(IBox other);

        void SetBox(Vector3i volume);

        bool Contains(IBox other);

        bool Contains(Point3i p);

        bool Contains(Vector3i v);
    }

    /// <summary>
    /// Define a box with origin (Left, Top, Front)
    /// </summary>
    public struct Box : IBox
    {
        public Box(uint width, uint height, uint depth)
        {
            _left = 0;
            _right = width;
            _top = 0;
            _bottom = height;
            _front = 0;
            _back = depth;
        }

        public Box(Vector3i volume)
        {
            _left = 0;
            _right = (uint)volume._x;
            _top = 0;
            _bottom = (uint)volume._y;
            _front = 0;
            _back = (uint)volume._z;
        }

        public Box(IBox other)
        {
            _left = other.Left;
            _right = other.Right;
            _top = other.Top;
            _bottom = other.Bottom;
            _front = other.Front;
            _back = other.Back;
        }

        public uint Left { get { return _left; } set { _left = value; } }
        private uint _left;

        public uint Right { get { return _right; } set { _right = value; } }
        private uint _right;

        public uint Top { get { return _top; } set { _top = value; } }
        private uint _top;

        public uint Bottom { get { return _bottom; } set { _bottom = value; } }
        private uint _bottom;

        public uint Front { get { return _front; } set { _front = value; } }
        private uint _front;

        public uint Back { get { return _back; } set { _back = value; } }
        private uint _back;


        public uint Width { get { return _right - _left; } }

        public uint Height { get { return _bottom - _top; } }

        public uint Depth { get { return _back - _front; } }

        public Point3i Origin { get { return new Point3i((int)_left, (int)_top, (int)_front); } }

        public Vector3i Size { get { return new Vector3i((int)Width, (int)Height, (int)Depth); } }

        public void SetBox(IBox other)
        {
            _left = other.Left;
            _right = other.Right;
            _top = other.Top;
            _bottom = other.Bottom;
            _front = other.Front;
            _back = other.Back;
        }

        public void SetBox(Vector3i volume)
        {
            _left = 0;
            _right = (uint)volume._x;
            _top = 0;
            _bottom = (uint)volume._y;
            _front = 0;
            _back = (uint)volume._z;
        }

        public bool Contains(IBox other)
        {
            return _left <= other.Left && _right >= other.Right
                && _top <= other.Top && _bottom >= other.Bottom
                && _front <= other.Front && _back >= other.Back;
        }

        public bool Contains(Point3i p)
        {
            return _left <= p._x && _right > p._x
                && _top <= p._y && _bottom > p._y
                && _front <= p._z && _back > p._z;
        }

        public bool Contains(Vector3i v)
        {
            return _left <= v._x && _right > v._x
                && _top <= v._y && _bottom > v._y
                && _front <= v._z && _back > v._z;
        }

        public bool Equals(IBox other)
        {
            return _left == other.Left && _right == other.Right
                && _top == other.Top && _bottom == other.Bottom
                && _front == other.Front && _back == other.Back;
        }
    }
}