using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector3i

    {
        public Vector3i(Ele x, Ele y, Ele z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool IsZero { get { return _x == 0 && _y == 0 && _z == 0; } }

        public static Vector3i Unit { get { return new Vector3i(1, 1, 1); } }

        public static Vector3i Zero { get { return new Vector3i(); } }

        public Ele LengthSquared
        {
            get
            {
                return _x * _x + _y * _y + _z * _z;
            }
        }

        public Ele MaxComponent { get { return Math.Max(Math.Max(Math.Abs(_x), Math.Abs(_y)), Math.Abs(_z)); } }

        public static Vector3i XAxis { get { return new Vector3i(1, 0, 0); } }

        public static Vector3i YAxis { get { return new Vector3i(0, 1, 0); } }

        public static Vector3i ZAxis { get { return new Vector3i(0, 0, 1); } }


        public static Vector3i operator -(Vector3i vector)
        {
            return new Vector3i(-vector._x, -vector._y, -vector._z);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        public static Vector3i operator +(Vector3i vector1, Vector3i vector2)
        {
            return new Vector3i(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3i Add(Vector3i vector1, Vector3i vector2)
        {
            return new Vector3i(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3i operator -(Vector3i vector1, Vector3i vector2)
        {
            return new Vector3i(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Vector3i Subtract(Vector3i vector1, Vector3i vector2)
        {
            return new Vector3i(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Point3i operator +(Vector3i vector, Point3i point)
        {
            return new Point3i(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3i Add(Vector3i vector, Point3i point)
        {
            return new Point3i(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3i operator -(Vector3i vector, Point3i point)
        {
            return new Point3i(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Point3i Subtract(Vector3i vector, Point3i point)
        {
            return new Point3i(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Vector3i operator *(Vector3i vector1, Vector3i vector2)
        {
            return new Vector3i(vector1._x * vector2._x, vector1._y * vector2._y, vector1._z * vector2._z);
        }

        public static Vector3i Multiply(Vector3i vector1, Vector3i vector2)
        {
            return vector1 * vector2;
        }

        public static Vector3i Multiply(Vector3i vector, Ele scalar)
        {
            return new Vector3i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3i operator *(Vector3i vector, Ele scalar)
        {
            return new Vector3i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3i operator *(Ele scalar, Vector3i vector)
        {
            return new Vector3i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3i Multiply(Ele scalar, Vector3i vector)
        {
            return new Vector3i(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }
        public void Floor(Vector3i vector)
        {
            _x = Math.Min(_x, vector._x);
            _y = Math.Min(_y, vector._y);
            _z = Math.Min(_z, vector._z);
        }

        public void Ceiling(Vector3i vector)
        {
            _x = Math.Max(_x, vector._x);
            _y = Math.Max(_y, vector._y);
            _z = Math.Max(_z, vector._z);
        }

        public void Floor(Ele x, Ele y, Ele z)
        {
            _x = Math.Min(_x, x);
            _y = Math.Min(_y, y);
            _z = Math.Min(_z, z);
        }

        public void Ceiling(Ele x, Ele y, Ele z)
        {
            _x = Math.Max(_x, x);
            _y = Math.Max(_y, y);
            _z = Math.Max(_z, z);
        }

        public static explicit operator Point3i(Vector3i vector)
        {
            return new Point3i(vector._x, vector._y, vector._z);
        }

        public static explicit operator Vector2i(Vector3i vector)
        {
            return new Vector2i(vector._x, vector._y);
        }

        public static bool operator ==(Vector3i vector1, Vector3i vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z;
        }

        public static bool operator !=(Vector3i vector1, Vector3i vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector3i vector1, Vector3i vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector3i))
            {
                return false;
            }

            Vector3i value = (Vector3i)o;
            return Vector3i.Equals(this, value);
        }

        public bool Equals(Vector3i value)
        {
            return Vector3i.Equals(this, value);
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