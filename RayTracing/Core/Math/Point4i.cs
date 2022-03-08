using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point4i
    {
        public Point4i(Ele x, Ele y, Ele z, Ele w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public void Offset(Ele deltaX, Ele deltaY, Ele deltaZ, Ele deltaW)
        {
            _x += deltaX;
            _y += deltaY;
            _z += deltaZ;
            _w += deltaW;
        }

        public static Point4i operator +(Point4i point1, Vector4i vec)
        {
            return new Point4i(point1._x + vec._x,
                               point1._y + vec._y,
                               point1._z + vec._z,
                               point1._w + vec._w);
        }

        public static Point4i Add(Point4i point1, Vector4i vec)
        {
            return new Point4i(point1._x + vec._x,
                               point1._y + vec._y,
                               point1._z + vec._z,
                               point1._w + vec._w);
        }

        public static Vector4i operator -(Point4i point1, Point4i point2)
        {
            return new Vector4i(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z,
                                point1._w - point2._w);
        }

        public static Vector4i Subtract(Point4i point1, Point4i point2)
        {
            return new Vector4i(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z,
                                point1._w - point2._w);
        }

        public static bool operator ==(Point4i point1, Point4i point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y &&
                   point1.Z == point2.Z &&
                   point1.W == point2.W;
        }

        public static bool operator !=(Point4i point1, Point4i point2)
        {
            return !(point1 == point2);
        }

        public static explicit operator Point3i(Point4i point)
        {
            return new Point3i(point._x, point._y, point._z);
        }

        public static explicit operator Vector3i(Point4i point)
        {
            return new Vector3i(point._x, point._y, point._z);
        }

        public static explicit operator Vector4i(Point4i point)
        {
            return new Vector4i(point._x, point._y, point._z, point._w);
        }

        public static bool Equals(Point4i point1, Point4i point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y) &&
                   point1.Z.Equals(point2.Z) &&
                   point1.W.Equals(point2.W);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point4i))
            {
                return false;
            }

            Point4i value = (Point4i)o;
            return Point4i.Equals(this, value);
        }

        public bool Equals(Point4i value)
        {
            return Point4i.Equals(this, value);
        }

        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode() ^
                   W.GetHashCode();
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

        public Ele W
        {
            get
            {
                return _w;
            }

            set
            {
                _w = value;
            }

        }

        internal Ele _x;
        internal Ele _y;
        internal Ele _z;
        internal Ele _w;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}, {_z}, {_w}");
        }
    }
}