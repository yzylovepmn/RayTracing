using System;
using System.Windows;
using System.Xml.Linq;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point2f
    {
        public Point2f(Float x, Float y)
        {
            _x = x;
            _y = y;
        }

        public void Offset(Float offsetX, Float offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        public static Point2f operator +(Point2f point, Vector2f vector)
        {
            return new Point2f(point._x + vector._x, point._y + vector._y);
        }

        public static Point2f Add(Point2f point, Vector2f vector)
        {
            return new Point2f(point._x + vector._x, point._y + vector._y);
        }

        public static Point2f operator -(Point2f point, Vector2f vector)
        {
            return new Point2f(point._x - vector._x, point._y - vector._y);
        }

        public static Point2f Subtract(Point2f point, Vector2f vector)
        {
            return new Point2f(point._x - vector._x, point._y - vector._y);
        }

        public static Vector2f operator -(Point2f point1, Point2f point2)
        {
            return new Vector2f(point1._x - point2._x, point1._y - point2._y);
        }

        public static Vector2f Subtract(Point2f point1, Point2f point2)
        {
            return new Vector2f(point1._x - point2._x, point1._y - point2._y);
        }

        public static Point2f operator *(Point2f point, Matrix2f matrix)
        {
            return matrix.Transform(point);
        }

        public static Point2f Multiply(Point2f point, Matrix2f matrix)
        {
            return matrix.Transform(point);
        }

        public static explicit operator Sizef(Point2f point)
        {
            return new Sizef(Math.Abs(point._x), Math.Abs(point._y));
        }

        public static implicit operator Vector2f(Point2f point)
        {
            return new Vector2f(point._x, point._y);
        }

        public static bool operator ==(Point2f point1, Point2f point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y;
        }
        public static bool operator !=(Point2f point1, Point2f point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point2f point1, Point2f point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point2f))
            {
                return false;
            }

            Point2f value = (Point2f)o;
            return Point2f.Equals(this, value);
        }
        public bool Equals(Point2f value)
        {
            return Point2f.Equals(this, value);
        }
        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        public Float X
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

        public Float Y
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

        internal Float _x;
        internal Float _y;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}");
        }

        public string ToString(int accuracy)
        {
            return string.Format("[{0}, {1}]", _x.ToString(string.Format("f{0}", accuracy)), _y.ToString(string.Format("f{0}", accuracy)));
        }
    }
}