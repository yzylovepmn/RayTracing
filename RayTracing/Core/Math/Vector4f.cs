using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector4f
    {
        public Vector4f(Float x, Float y, Float z, Float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public bool IsZero { get { return _x == 0 && _y == 0 && _z == 0 && _w == 0; } }

        public static Vector4f Unit { get { return new Vector4f(1, 1, 1, 1); } }

        public static Vector4f Zero { get { return new Vector4f(); } }

        public bool IsNaN { get { return Float.IsNaN(_x) || Float.IsNaN(_y) || Float.IsNaN(_z) || Float.IsNaN(_w); } }

        public Float Length
        {
            get
            {
                return (Float)Math.Sqrt(_x * _x + _y * _y + _z * _z + _w * _w);
            }
        }

        public Float LengthSquared
        {
            get
            {
                return _x * _x + _y * _y + _z * _z + _w * _w;
            }
        }

        public Float MaxComponent { get { return Math.Max(Math.Max(Math.Max(Math.Abs(_x), Math.Abs(_y)), Math.Abs(_z)), Math.Abs(_w)); } }

        /// <summary>
        /// Normalize the vector and return the previous length of it.
        /// </summary>
        /// <returns></returns>
        public Float Normalize()
        {
            if (IsZero) return 0;

            // Computation of length can overflow easily because it
            // first computes squared length, so we first divide by
            // the largest coefficient.
            Float m = Math.Abs(_x);
            Float absy = Math.Abs(_y);
            Float absz = Math.Abs(_z);
            Float absw = Math.Abs(_w);
            if (absy > m)
            {
                m = absy;
            }
            if (absz > m)
            {
                m = absz;
            }
            if (absw > m)
            {
                m = absw;
            }

            _x /= m;
            _y /= m;
            _z /= m;
            _w /= m;

            Float length = (Float)Math.Sqrt(_x * _x + _y * _y + _z * _z + _w * _w);
            this /= length;
            return length;
        }

        public static Vector4f operator -(Vector4f vector)
        {
            return new Vector4f(-vector._x, -vector._y, -vector._z, -vector._w);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
            _w = -_w;
        }

        public static Vector4f operator +(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z,
                                vector1._w + vector2._w);
        }

        public static Vector4f Add(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z,
                                vector1._w + vector2._w);
        }

        public static Vector4f operator -(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z,
                                vector1._w - vector2._w);
        }

        public static Vector4f Subtract(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z,
                                vector1._w - vector2._w);
        }

        public static Point4f operator +(Vector4f vector, Point4f point)
        {
            return new Point4f(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z,
                               vector._w + point._w);
        }

        public static Point4f Add(Vector4f vector, Point4f point)
        {
            return new Point4f(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z,
                               vector._w + point._w);
        }

        public static Point4f operator -(Vector4f vector, Point4f point)
        {
            return new Point4f(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z,
                               vector._w - point._w);
        }

        public static Point4f Subtract(Vector4f vector, Point4f point)
        {
            return new Point4f(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z,
                               vector._w - point._w);
        }

        public static Vector4f operator *(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x * vector2._x, vector1._y * vector2._y, vector1._z * vector2._z, vector1._w * vector2._w);
        }

        public static Vector4f Multiply(Vector4f vector1, Vector4f vector2)
        {
            return vector1 * vector2;
        }

        public static Vector4f Multiply(Vector4f vector, Float scalar)
        {
            return new Vector4f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4f operator *(Vector4f vector, Float scalar)
        {
            return new Vector4f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4f operator *(Float scalar, Vector4f vector)
        {
            return new Vector4f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4f Multiply(Float scalar, Vector4f vector)
        {
            return new Vector4f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar,
                                vector._w * scalar);
        }

        public static Vector4f operator /(Vector4f vector1, Vector4f vector2)
        {
            return new Vector4f(vector1._x / vector2._x, vector1._y / vector2._y, vector1._z / vector2._z, vector1._w / vector2._w);
        }

        public static Vector4f Divide(Vector4f vector1, Vector4f vector2)
        {
            return vector1 / vector2;
        }

        public static Vector4f operator /(Vector4f vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        public static Vector4f Divide(Vector4f vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        public static Float DotProduct(Vector4f vector1, Vector4f vector2)
        {
            return vector1._x * vector2._x +
                   vector1._y * vector2._y +
                   vector1._z * vector2._z +
                   vector1._w * vector2._w;
        }

        public static Float AbsDotProduct(Vector4f vector1, Vector4f vector2)
        {
            return Math.Abs(vector1._x * vector2._x) +
                   Math.Abs(vector1._y * vector2._y) +
                   Math.Abs(vector1._z * vector2._z) +
                   Math.Abs(vector1._w * vector2._w);
        }

        public void Floor(Vector4f vector)
        {
            _x = Math.Min(_x, vector._x);
            _y = Math.Min(_y, vector._y);
            _z = Math.Min(_z, vector._z);
            _w = Math.Min(_w, vector._w);
        }

        public void Ceiling(Vector4f vector)
        {
            _x = Math.Max(_x, vector._x);
            _y = Math.Max(_y, vector._y);
            _z = Math.Max(_z, vector._z);
            _w = Math.Max(_w, vector._w);
        }

        public void Floor(Float x, Float y, Float z, Float w)
        {
            _x = Math.Min(_x, x);
            _y = Math.Min(_y, y);
            _z = Math.Min(_z, z);
            _w = Math.Min(_w, w);
        }

        public void Ceiling(Float x, Float y, Float z, Float w)
        {
            _x = Math.Max(_x, x);
            _y = Math.Max(_y, y);
            _z = Math.Max(_z, z);
            _w = Math.Max(_w, w);
        }

        public static explicit operator Point4f(Vector4f vector)
        {
            return new Point4f(vector._x, vector._y, vector._z, vector._w);
        }

        public static explicit operator Vector3f(Vector4f vector)
        {
            return new Vector3f(vector._x, vector._y, vector._z);
        }

        public static bool operator ==(Vector4f vector1, Vector4f vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z &&
                   vector1.W == vector2.W;
        }

        public static bool operator !=(Vector4f vector1, Vector4f vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector4f vector1, Vector4f vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z) &&
                   vector1.W.Equals(vector2.W);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector4f))
            {
                return false;
            }

            Vector4f value = (Vector4f)o;
            return Vector4f.Equals(this, value);
        }

        public bool Equals(Vector4f value)
        {
            return Vector4f.Equals(this, value);
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
            return string.Format($"{_x}, {_y}, {_z}, { _w}");
        }
    }
}