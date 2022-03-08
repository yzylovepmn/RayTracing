using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector2i

    {
        public Vector2i(Ele x, Ele y)
        {
            _x = x;
            _y = y;
        }

        public static Vector2i Zero { get { return new Vector2i(); } }

        public bool IsEmpty { get { return _x == (Ele)0 && _y == (Ele)0; } }

        public Ele LengthSquared
        {
            get
            {
                return _x * _x + _y * _y;
            }
        }

        public static Vector2i operator -(Vector2i vector)
        {
            return new Vector2i(-vector._x, -vector._y);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
        }

        public static Vector2i operator +(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1._x + vector2._x,
                              vector1._y + vector2._y);
        }

        public static Vector2i Add(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1._x + vector2._x,
                              vector1._y + vector2._y);
        }

        public static Vector2i operator -(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1._x - vector2._x,
                              vector1._y - vector2._y);
        }

        public static Vector2i Subtract(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1._x - vector2._x,
                              vector1._y - vector2._y);
        }

        public static Point2i operator +(Vector2i vector, Point2i point)
        {
            return new Point2i(point._x + vector._x, point._y + vector._y);
        }

        public static Point2i Add(Vector2i vector, Point2i point)
        {
            return new Point2i(point._x + vector._x, point._y + vector._y);
        }

        public static Vector2i operator *(Vector2i vector, Ele scalar)
        {
            return new Vector2i(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2i Multiply(Vector2i vector, Ele scalar)
        {
            return new Vector2i(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2i operator *(Ele scalar, Vector2i vector)
        {
            return new Vector2i(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2i Multiply(Ele scalar, Vector2i vector)
        {
            return new Vector2i(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Ele operator *(Vector2i vector1, Vector2i vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static Ele Multiply(Vector2i vector1, Vector2i vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static explicit operator Point2i(Vector2i vector)
        {
            return new Point2i(vector._x, vector._y);
        }

        public static bool operator ==(Vector2i vector1, Vector2i vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y;
        }

        public static bool operator !=(Vector2i vector1, Vector2i vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector2i vector1, Vector2i vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector2i))
            {
                return false;
            }

            Vector2i value = (Vector2i)o;
            return Vector2i.Equals(this, value);
        }

        public bool Equals(Vector2i value)
        {
            return Vector2i.Equals(this, value);
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