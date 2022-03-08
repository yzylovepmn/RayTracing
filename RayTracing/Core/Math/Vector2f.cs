using System;
using Float = System.Single;

namespace RayTracing.Core
{
    public struct Vector2f
    {
        public Vector2f(Float x, Float y)
        {
            _x = x;
            _y = y;
        }

        public static Vector2f Zero { get { return new Vector2f(); } }

        public Float Length
        {
            get
            {
                return (Float)Math.Sqrt(_x * _x + _y * _y);
            }
        }

        public bool IsEmpty { get { return _x == 0f && _y == 0f; } }

        public bool IsNaN { get { return Float.IsNaN(_x) || Float.IsNaN(_y); } }

        public Float LengthSquared
        {
            get
            {
                return _x * _x + _y * _y;
            }
        }

        public void Normalize()
        {
            // Avoid overflow
            this /= Math.Max(Math.Abs(_x), Math.Abs(_y));
            this /= Length;
        }

        public static Float CrossProduct(Vector2f vector1, Vector2f vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        public static Float AngleBetween(Vector2f vector1, Vector2f vector2)
        {
            Float sin = vector1._x * vector2._y - vector2._x * vector1._y;
            Float cos = vector1._x * vector2._x + vector1._y * vector2._y;

            return (Float)(Math.Atan2(sin, cos) * (180 / Math.PI));
        }

        public static Vector2f operator -(Vector2f vector)
        {
            return new Vector2f(-vector._x, -vector._y);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
        }

        public static Vector2f operator +(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1._x + vector2._x,
                              vector1._y + vector2._y);
        }

        public static Vector2f Add(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1._x + vector2._x,
                              vector1._y + vector2._y);
        }

        public static Vector2f operator -(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1._x - vector2._x,
                              vector1._y - vector2._y);
        }

        public static Vector2f Subtract(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1._x - vector2._x,
                              vector1._y - vector2._y);
        }

        public static Point2f operator +(Vector2f vector, Point2f point)
        {
            return new Point2f(point._x + vector._x, point._y + vector._y);
        }

        public static Point2f Add(Vector2f vector, Point2f point)
        {
            return new Point2f(point._x + vector._x, point._y + vector._y);
        }

        public static Vector2f operator *(Vector2f vector, Float scalar)
        {
            return new Vector2f(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2f Multiply(Vector2f vector, Float scalar)
        {
            return new Vector2f(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2f operator *(Float scalar, Vector2f vector)
        {
            return new Vector2f(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2f Multiply(Float scalar, Vector2f vector)
        {
            return new Vector2f(vector._x * scalar,
                              vector._y * scalar);
        }

        public static Vector2f operator /(Vector2f vector, Float scalar)
        {
            return vector * (1.0f / scalar);
        }

        public static Vector2f Divide(Vector2f vector, Float scalar)
        {
            return vector * (1.0f / scalar);
        }

        public static Vector2f operator *(Vector2f vector, Matrix2f matrix)
        {
            return matrix.Transform(vector);
        }

        public static Vector2f Multiply(Vector2f vector, Matrix2f matrix)
        {
            return matrix.Transform(vector);
        }

        public static Float operator *(Vector2f vector1, Vector2f vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static Float Multiply(Vector2f vector1, Vector2f vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static Float Determinant(Vector2f vector1, Vector2f vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        public static explicit operator Sizef(Vector2f vector)
        {
            return new Sizef(Math.Abs(vector._x), Math.Abs(vector._y));
        }

        public static implicit operator Point2f(Vector2f vector)
        {
            return new Point2f(vector._x, vector._y);
        }

        public static bool operator ==(Vector2f vector1, Vector2f vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y;
        }

        public static bool operator !=(Vector2f vector1, Vector2f vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector2f vector1, Vector2f vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector2f))
            {
                return false;
            }

            Vector2f value = (Vector2f)o;
            return Vector2f.Equals(this, value);
        }

        public bool Equals(Vector2f value)
        {
            return Vector2f.Equals(this, value);
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
    }
}