using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector4i
    {
        public Vector4i(Ele x, Ele y, Ele z, Ele w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public bool IsZero { get { return _x == 0 && _y == 0 && _z == 0 && _w == 0; } }

        public static Vector4i Unit { get { return new Vector4i(1, 1, 1, 1); } }

        public static Vector4i Zero { get { return new Vector4i(); } }

        public Ele LengthSquared
        {
            get
            {
                return _x * _x + _y * _y + _z * _z + _w * _w;
            }
        }

        public Ele MaxComponent { get { return Math.Max(Math.Max(Math.Max(Math.Abs(_x), Math.Abs(_y)), Math.Abs(_z)), Math.Abs(_w)); } }

        public static Vector4i operator -(Vector4i vector)
        {
            return new Vector4i(-vector._x, -vector._y, -vector._z, -vector._w);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
            _w = -_w;
        }

        public static Vector4i operator +(Vector4i vector1, Vector4i vector2)
        {
            return new Vector4i(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z,
                                vector1._w + vector2._w);
        }

        public static Vector4i Add(Vector4i vector1, Vector4i vector2)
        {
            return new Vector4i(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z,
                                vector1._w + vector2._w);
        }

        public static Vector4i operator -(Vector4i vector1, Vector4i vector2)
        {
            return new Vector4i(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z,
                                vector1._w - vector2._w);
        }

        public static Vector4i Subtract(Vector4i vector1, Vector4i vector2)
        {
            return new Vector4i(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z,
                                vector1._w - vector2._w);
        }

        public static Point4i operator +(Vector4i vector, Point4i point)
        {
            return new Point4i(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z,
                               vector._w + point._w);
        }

        public static Point4i Add(Vector4i vector, Point4i point)
        {
            return new Point4i(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z,
                               vector._w + point._w);
        }

        public static Point4i operator -(Vector4i vector, Point4i point)
        {
            return new Point4i(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z,
                               vector._w - point._w);
        }

        public static Point4i Subtract(Vector4i vector, Point4i point)
        {
            return new Point4i(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z,
                               vector._w - point._w);
        }

        public static Vector4i operator *(Vector4i vector1, Vector4i vector2)
        {
            return new Vector4i(vector1._x * vector2._x, vector1._y * vector2._y, vector1._z * vector2._z, vector1._w * vector2._w);
        }

        public static Vector4i Multiply(Vector4i vector1, Vector4i vector2)
        {
            return vector1 * vector2;
        }

        public static Vector4i Multiply(Vector4i vector, Ele scalar)
        {
            return new Vector4i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4i operator *(Vector4i vector, Ele scalar)
        {
            return new Vector4i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4i operator *(Ele scalar, Vector4i vector)
        {
            return new Vector4i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4i Multiply(Ele scalar, Vector4i vector)
        {
            return new Vector4i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public void Floor(Vector4i vector)
        {
            _x = Math.Min(_x, vector._x);
            _y = Math.Min(_y, vector._y);
            _z = Math.Min(_z, vector._z);
            _w = Math.Min(_w, vector._w);
        }

        public void Ceiling(Vector4i vector)
        {
            _x = Math.Max(_x, vector._x);
            _y = Math.Max(_y, vector._y);
            _z = Math.Max(_z, vector._z);
            _w = Math.Max(_w, vector._w);
        }

        public void Floor(Ele x, Ele y, Ele z, Ele w)
        {
            _x = Math.Min(_x, x);
            _y = Math.Min(_y, y);
            _z = Math.Min(_z, z);
            _w = Math.Min(_w, w);
        }

        public void Ceiling(Ele x, Ele y, Ele z, Ele w)
        {
            _x = Math.Max(_x, x);
            _y = Math.Max(_y, y);
            _z = Math.Max(_z, z);
            _w = Math.Max(_w, w);
        }

        public static explicit operator Point4i(Vector4i vector)
        {
            return new Point4i(vector._x, vector._y, vector._z, vector._w);
        }

        public static explicit operator Vector3i(Vector4i vector)
        {
            return new Vector3i(vector._x, vector._y, vector._z);
        }

        public static bool operator ==(Vector4i vector1, Vector4i vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z &&
                   vector1.W == vector2.W;
        }

        public static bool operator !=(Vector4i vector1, Vector4i vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector4i vector1, Vector4i vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z) &&
                   vector1.W.Equals(vector2.W);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector4i))
            {
                return false;
            }

            Vector4i value = (Vector4i)o;
            return Vector4i.Equals(this, value);
        }

        public bool Equals(Vector4i value)
        {
            return Vector4i.Equals(this, value);
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
            return string.Format($"{_x}, {_y}, {_z}, { _w}");
        }
    }
}