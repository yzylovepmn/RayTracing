using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector3f
    {
        public Vector3f(Float x, Float y, Float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool IsZero { get { return _x == 0 && _y == 0 && _z == 0; } }

        public static Vector3f Unit { get { return new Vector3f(1, 1, 1); } }

        public static Vector3f Zero { get { return new Vector3f(); } }

        public bool IsNaN { get { return Float.IsNaN(_x) || Float.IsNaN(_y) || Float.IsNaN(_z); } }

        public Float Length
        {
            get
            {
                return (Float)Math.Sqrt(_x * _x + _y * _y + _z * _z);
            }
        }

        public Float LengthSquared
        {
            get
            {
                return _x * _x + _y * _y + _z * _z;
            }
        }

        public Float MaxComponent { get { return Math.Max(Math.Max(Math.Abs(_x), Math.Abs(_y)), Math.Abs(_z)); } }

        public static Vector3f XAxis { get { return new Vector3f(1, 0, 0); } }

        public static Vector3f YAxis { get { return new Vector3f(0, 1, 0); } }

        public static Vector3f ZAxis { get { return new Vector3f(0, 0, 1); } }

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
            if (absy > m)
            {
                m = absy;
            }
            if (absz > m)
            {
                m = absz;
            }

            _x /= m;
            _y /= m;
            _z /= m;

            Float length = (Float)Math.Sqrt(_x * _x + _y * _y + _z * _z);
            this /= length;
            return length;
        }

        public static Float AngleBetween(Vector3f vector1, Vector3f vector2)
        {
            vector1.Normalize();
            vector2.Normalize();

            Float ratio = DotProduct(vector1, vector2);

            // The "straight forward" method of acos(u.v) has large precision
            // issues when the dot product is near +/-1.  This is due to the
            // steep slope of the acos function as we approach +/- 1.  Slight
            // precision errors in the dot product calculation cause large
            // variation in the output value.
            //
            //        |                   |
            //         \__                |
            //            ---___          |
            //                  ---___    |
            //                        ---_|_
            //                            | ---___
            //                            |       ---___
            //                            |             ---__
            //                            |                  \
            //                            |                   |
            //       -|-------------------+-------------------|-
            //       -1                   0                   1
            //
            //                         acos(x)
            //
            // To avoid this we use an alternative method which finds the
            // angle bisector by (u-v)/2:
            //
            //                            _>
            //                       u  _-  \ (u-v)/2
            //                        _-  __-v
            //                      _=__--      
            //                    .=----------->
            //                            v
            //
            // Because u and v and unit vectors, (u-v)/2 forms a right angle
            // with the angle bisector.  The hypotenuse is 1, therefore
            // 2*asin(|u-v|/2) gives us the angle between u and v.
            //
            // The largest possible value of |u-v| occurs with perpendicular
            // vectors and is sqrt(2)/2 which is well away from extreme slope
            // at +/-1.
            //
            // (See Windows OS Bug #1706299 for details)

            double theta;

            if (ratio < 0)
            {
                theta = Math.PI - 2.0 * Math.Asin((-vector1 - vector2).Length / 2.0);
            }
            else
            {
                theta = 2.0 * Math.Asin((vector1 - vector2).Length / 2.0);
            }

            return (Float)MathUtil.RadiansToDegrees(theta);
        }

        public static Vector3f operator -(Vector3f vector)
        {
            return new Vector3f(-vector._x, -vector._y, -vector._z);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        public static Vector3f operator +(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3f Add(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3f operator -(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Vector3f Subtract(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Point3f operator +(Vector3f vector, Point3f point)
        {
            return new Point3f(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3f Add(Vector3f vector, Point3f point)
        {
            return new Point3f(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3f operator -(Vector3f vector, Point3f point)
        {
            return new Point3f(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Point3f Subtract(Vector3f vector, Point3f point)
        {
            return new Point3f(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Vector3f operator *(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x * vector2._x, vector1._y * vector2._y, vector1._z * vector2._z);
        }

        public static Vector3f Multiply(Vector3f vector1, Vector3f vector2)
        {
            return vector1 * vector2;
        }

        public static Vector3f Multiply(Vector3f vector, Float scalar)
        {
            return new Vector3f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3f operator *(Vector3f vector, Float scalar)
        {
            return new Vector3f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3f operator *(Float scalar, Vector3f vector)
        {
            return new Vector3f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3f Multiply(Float scalar, Vector3f vector)
        {
            return new Vector3f(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3f operator /(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1._x / vector2._x, vector1._y / vector2._y, vector1._z / vector2._z);
        }

        public static Vector3f Divide(Vector3f vector1, Vector3f vector2)
        {
            return vector1 / vector2;
        }

        public static Vector3f operator /(Vector3f vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        public static Vector3f Divide(Vector3f vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        public static Vector3f operator *(Vector3f vector, Matrix3f matrix)
        {
            return matrix.Transform(vector);
        }

        public static Vector3f Multiply(Vector3f vector, Matrix3f matrix)
        {
            return matrix.Transform(vector);
        }

        public static Float DotProduct(Vector3f vector1, Vector3f vector2)
        {
            return vector1._x * vector2._x +
                   vector1._y * vector2._y +
                   vector1._z * vector2._z;
        }

        public static Float AbsDotProduct(Vector3f vector1, Vector3f vector2)
        {
            return Math.Abs(vector1._x * vector2._x) +
                   Math.Abs(vector1._y * vector2._y) +
                   Math.Abs(vector1._z * vector2._z);
        }

        public static Vector3f CrossProduct(Vector3f vector1, Vector3f vector2)
        {
            Vector3f result;
            result._x = vector1._y * vector2._z - vector1._z * vector2._y;
            result._y = vector1._z * vector2._x - vector1._x * vector2._z;
            result._z = vector1._x * vector2._y - vector1._y * vector2._x;
            return result;
        }

        /// <summary>
        /// Assumes 'this' is pointing to the plane
        /// </summary>
        /// <param name="normal">the normal of the reflect plane</param>
        /// <returns>the Reflect vector is point away from the plane</returns>
        public Vector3f Reflect(Vector3f normal)
        {
            return this - 2 * DotProduct(this, normal) * normal;
        }

        public void Floor(Vector3f vector)
        {
            _x = Math.Min(_x, vector._x);
            _y = Math.Min(_y, vector._y);
            _z = Math.Min(_z, vector._z);
        }

        public void Ceiling(Vector3f vector)
        {
            _x = Math.Max(_x, vector._x);
            _y = Math.Max(_y, vector._y);
            _z = Math.Max(_z, vector._z);
        }

        public void Floor(Float x, Float y, Float z)
        {
            _x = Math.Min(_x, x);
            _y = Math.Min(_y, y);
            _z = Math.Min(_z, z);
        }

        public void Ceiling(Float x, Float y, Float z)
        {
            _x = Math.Max(_x, x);
            _y = Math.Max(_y, y);
            _z = Math.Max(_z, z);
        }

        public static explicit operator Point3f(Vector3f vector)
        {
            return new Point3f(vector._x, vector._y, vector._z);
        }

        public static implicit operator Vector3d(Vector3f vector)
        {
            return new Vector3d(vector._x, vector._y, vector._z);
        }

        public static explicit operator Vector2f(Vector3f vector)
        {
            return new Vector2f(vector._x, vector._y);
        }

        public static explicit operator Size3f(Vector3f vector)
        {
            return new Size3f(Math.Abs(vector._x), Math.Abs(vector._y), Math.Abs(vector._z));
        }

        public static bool operator ==(Vector3f vector1, Vector3f vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z;
        }

        public static bool operator !=(Vector3f vector1, Vector3f vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector3f vector1, Vector3f vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector3f))
            {
                return false;
            }

            Vector3f value = (Vector3f)o;
            return Vector3f.Equals(this, value);
        }

        public bool Equals(Vector3f value)
        {
            return Vector3f.Equals(this, value);
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