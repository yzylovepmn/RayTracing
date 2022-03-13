using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point3f
    {
        public static Point3f Origin { get { return new Point3f(0, 0, 0); } }

        public Point3f(Float x, Float y, Float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Get component of vector without range check
        /// </summary>
        public Float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _x;
                    case 1:
                        return _y;
                    case 2:
                        return _z;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public bool IsNaN { get { return Float.IsNaN(_x) || Float.IsNaN(_y) || Float.IsNaN(_z); } }

        public void Offset(Float offsetX, Float offsetY, Float offsetZ)
        {
            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }

        public static Point3f operator +(Point3f point, Vector3f vector)
        {
            return new Point3f(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3f Add(Point3f point, Vector3f vector)
        {
            return new Point3f(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3f operator -(Point3f point, Vector3f vector)
        {
            return new Point3f(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Point3f operator -(Point3f point)
        {
            return new Point3f(-point._x,
                               -point._y,
                               -point._z);
        }

        public static Point3f Subtract(Point3f point, Vector3f vector)
        {
            return new Point3f(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Vector3f operator -(Point3f point1, Point3f point2)
        {
            return new Vector3f(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z);
        }

        public static Vector3f Subtract(Point3f point1, Point3f point2)
        {
            Vector3f v = new Vector3f();
            Subtract(ref point1, ref point2, out v);
            return v;
        }

        internal static void Subtract(ref Point3f p1, ref Point3f p2, out Vector3f result)
        {
            result._x = p1._x - p2._x;
            result._y = p1._y - p2._y;
            result._z = p1._z - p2._z;
        }

        public static Point3f operator *(Point3f point, Matrix3f matrix)
        {
            return matrix.Transform(point);
        }

        public static Point3f Multiply(Point3f point, Matrix3f matrix)
        {
            return matrix.Transform(point);
        }

        public static explicit operator Vector3f(Point3f point)
        {
            return new Vector3f(point._x, point._y, point._z);
        }

        public static implicit operator Point2f(Point3f point)
        {
            return new Point2f(point._x, point._y);
        }

        public static explicit operator Point4f(Point3f point)
        {
            return new Point4f(point._x, point._y, point._z, (Float)1.0);
        }

        public static bool operator ==(Point3f point1, Point3f point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y &&
                   point1.Z == point2.Z;
        }

        public static bool operator !=(Point3f point1, Point3f point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point3f point1, Point3f point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y) &&
                   point1.Z.Equals(point2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point3f))
            {
                return false;
            }

            Point3f value = (Point3f)o;
            return Point3f.Equals(this, value);
        }

        public bool Equals(Point3f value)
        {
            return Point3f.Equals(this, value);
        }

        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode();
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

        public Float Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }

        }

        internal Float _x;
        internal Float _y;
        internal Float _z;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}, {_z}");
        }
    }
}