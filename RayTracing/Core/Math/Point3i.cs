using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point3i

    {
        public static Point3i Origin { get { return new Point3i(0, 0, 0); } }

        public Point3i(Ele x, Ele y, Ele z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public void Offset(Ele offsetX, Ele offsetY, Ele offsetZ)
        {
            _x += offsetX;
            _y += offsetY;
            _z += offsetZ;
        }

        public static Point3i operator +(Point3i point, Vector3i vector)
        {
            return new Point3i(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3i Add(Point3i point, Vector3i vector)
        {
            return new Point3i(point._x + vector._x,
                               point._y + vector._y,
                               point._z + vector._z);
        }

        public static Point3i operator -(Point3i point, Vector3i vector)
        {
            return new Point3i(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Point3i operator -(Point3i point)
        {
            return new Point3i(-point._x,
                               -point._y,
                               -point._z);
        }

        public static Point3i Subtract(Point3i point, Vector3i vector)
        {
            return new Point3i(point._x - vector._x,
                               point._y - vector._y,
                               point._z - vector._z);
        }

        public static Vector3i operator -(Point3i point1, Point3i point2)
        {
            return new Vector3i(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z);
        }

        public static Vector3i Subtract(Point3i point1, Point3i point2)
        {
            Vector3i v = new Vector3i();
            Subtract(ref point1, ref point2, out v);
            return v;
        }

        internal static void Subtract(ref Point3i p1, ref Point3i p2, out Vector3i result)
        {
            result._x = p1._x - p2._x;
            result._y = p1._y - p2._y;
            result._z = p1._z - p2._z;
        }

        public static explicit operator Vector3i(Point3i point)
        {
            return new Vector3i(point._x, point._y, point._z);
        }

        public static explicit operator Point2i(Point3i point)
        {
            return new Point2i(point._x, point._y);
        }

        public static explicit operator Point4i(Point3i point)
        {
            return new Point4i(point._x, point._y, point._z, (Ele)1.0);
        }

        public static bool operator ==(Point3i point1, Point3i point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y &&
                   point1.Z == point2.Z;
        }

        public static bool operator !=(Point3i point1, Point3i point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point3i point1, Point3i point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y) &&
                   point1.Z.Equals(point2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point3i))
            {
                return false;
            }

            Point3i value = (Point3i)o;
            return Point3i.Equals(this, value);
        }

        public bool Equals(Point3i value)
        {
            return Point3i.Equals(this, value);
        }

        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode();
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

        public Ele Z
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

        internal Ele _x;
        internal Ele _y;
        internal Ele _z;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}, {_z}");
        }
    }
}