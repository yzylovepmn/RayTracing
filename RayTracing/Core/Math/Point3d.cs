using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Double;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point3d
    {
        public static Point3d Origin { get { return new Point3d(0, 0, 0); } }

        public Point3d(Float x, Float y, Float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool IsNaN { get { return Float.IsNaN(_x) || Float.IsNaN(_y) || Float.IsNaN(_z); } }

        public void Offset(Float offsetX, Float offsetY, Float offsetZ)
        {
            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }

        public static Point3d operator +(Point3d point, Vector3d vector)
        {
            return new Point3d(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3d Add(Point3d point, Vector3d vector)
        {
            return new Point3d(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3d operator -(Point3d point, Vector3d vector)
        {
            return new Point3d(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Point3d operator -(Point3d point)
        {
            return new Point3d(-point._x,
                               -point._y,
                               -point._z);
        }

        public static Point3d Subtract(Point3d point, Vector3d vector)
        {
            return new Point3d(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Vector3d operator -(Point3d point1, Point3d point2)
        {
            return new Vector3d(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z);
        }

        public static Vector3d Subtract(Point3d point1, Point3d point2)
        {
            Vector3d v = new Vector3d();
            Subtract(ref point1, ref point2, out v);
            return v;
        }

        internal static void Subtract(ref Point3d p1, ref Point3d p2, out Vector3d result)
        {
            result._x = p1._x - p2._x;
            result._y = p1._y - p2._y;
            result._z = p1._z - p2._z;
        }

        //public static Point3d operator *(Point3d point, Matrix3f matrix)
        //{
        //    return matrix.Transform(point);
        //}

        //public static Point3d Multiply(Point3d point, Matrix3f matrix)
        //{
        //    return matrix.Transform(point);
        //}

        public static explicit operator Vector3d(Point3d point)
        {
            return new Vector3d(point._x, point._y, point._z);
        }

        //public static explicit operator Point2f(Point3d point)
        //{
        //    return new Point2f(point._x, point._y);
        //}

        //public static explicit operator Point4f(Point3d point)
        //{
        //    return new Point4f(point._x, point._y, point._z, (Float)1.0);
        //}

        public static bool operator ==(Point3d point1, Point3d point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y &&
                   point1.Z == point2.Z;
        }

        public static bool operator !=(Point3d point1, Point3d point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point3d point1, Point3d point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y) &&
                   point1.Z.Equals(point2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point3d))
            {
                return false;
            }

            Point3d value = (Point3d)o;
            return Point3d.Equals(this, value);
        }

        public bool Equals(Point3d value)
        {
            return Point3d.Equals(this, value);
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