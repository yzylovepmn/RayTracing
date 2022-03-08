using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point2i

    {
        public Point2i(Ele x, Ele y)
        {
            _x = x;
            _y = y;
        }

        public void Offset(Ele offsetX, Ele offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        public static Point2i operator +(Point2i point, Vector2i vector)
        {
            return new Point2i(point._x + vector._x, point._y + vector._y);
        }

        public static Point2i Add(Point2i point, Vector2i vector)
        {
            return new Point2i(point._x + vector._x, point._y + vector._y);
        }

        public static Point2i operator -(Point2i point, Vector2i vector)
        {
            return new Point2i(point._x - vector._x, point._y - vector._y);
        }

        public static Point2i Subtract(Point2i point, Vector2i vector)
        {
            return new Point2i(point._x - vector._x, point._y - vector._y);
        }

        public static Vector2i operator -(Point2i point1, Point2i point2)
        {
            return new Vector2i(point1._x - point2._x, point1._y - point2._y);
        }

        public static Vector2i Subtract(Point2i point1, Point2i point2)
        {
            return new Vector2i(point1._x - point2._x, point1._y - point2._y);
        }

        public static explicit operator Vector2i(Point2i point)
        {
            return new Vector2i(point._x, point._y);
        }

        public static bool operator ==(Point2i point1, Point2i point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y;
        }

        public static bool operator !=(Point2i point1, Point2i point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point2i point1, Point2i point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point2i))
            {
                return false;
            }

            Point2i value = (Point2i)o;
            return Point2i.Equals(this, value);
        }
        public bool Equals(Point2i value)
        {
            return Point2i.Equals(this, value);
        }
        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        public Ele X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }

        }

        public Ele Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }

        }

        internal Ele _x;
        internal Ele _y;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}");
        }
    }
}