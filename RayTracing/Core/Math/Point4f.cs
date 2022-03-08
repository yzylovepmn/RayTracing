using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Point4f
    {
        public Point4f(Float x, Float y, Float z, Float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public void Offset(Float deltaX, Float deltaY, Float deltaZ, Float deltaW)
        {
            _x += deltaX;
            _y += deltaY;
            _z += deltaZ;
            _w += deltaW;
        }

        public static Point4f operator +(Point4f point1, Vector4f vec)
        {
            return new Point4f(point1._x + vec._x,
                               point1._y + vec._y,
                               point1._z + vec._z,
                               point1._w + vec._w);
        }

        public static Point4f Add(Point4f point1, Vector4f vec)
        {
            return new Point4f(point1._x + vec._x,
                               point1._y + vec._y,
                               point1._z + vec._z,
                               point1._w + vec._w);
        }

        public static Vector4f operator -(Point4f point1, Point4f point2)
        {
            return new Vector4f(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z,
                                point1._w - point2._w);
        }

        public static Vector4f Subtract(Point4f point1, Point4f point2)
        {
            return new Vector4f(point1._x - point2._x,
                                point1._y - point2._y,
                                point1._z - point2._z,
                                point1._w - point2._w);
        }

        public static Point4f operator *(Point4f point, Matrix3f matrix)
        {
            return matrix.Transform(point);
        }

        public static Point4f Multiply(Point4f point, Matrix3f matrix)
        {
            return matrix.Transform(point);
        }

        public static bool operator ==(Point4f point1, Point4f point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y &&
                   point1.Z == point2.Z &&
                   point1.W == point2.W;
        }

        public static bool operator !=(Point4f point1, Point4f point2)
        {
            return !(point1 == point2);
        }

        public static explicit operator Point3f(Point4f point)
        {
            return new Point3f(point._x, point._y, point._z);
        }

        public static explicit operator Vector3f(Point4f point)
        {
            return new Vector3f(point._x, point._y, point._z);
        }

        public static explicit operator Vector4f(Point4f point)
        {
            return new Vector4f(point._x, point._y, point._z, point._w);
        }

        public static bool Equals(Point4f point1, Point4f point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y) &&
                   point1.Z.Equals(point2.Z) &&
                   point1.W.Equals(point2.W);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point4f))
            {
                return false;
            }

            Point4f value = (Point4f)o;
            return Point4f.Equals(this, value);
        }

        public bool Equals(Point4f value)
        {
            return Point4f.Equals(this, value);
        }

        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode() ^
                   W.GetHashCode();
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

        public Float W
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

        internal Float _x;
        internal Float _y;
        internal Float _z;
        internal Float _w;

        public override string ToString()
        {
            return string.Format($"{_x}, {_y}, {_z}, {_w}");
        }
    }
}