using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Double;

namespace RayTracing.Core
{
    [Serializable]
    public struct Vector3d
    {
        public Vector3d(Float x, Float y, Float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool IsZero { get { return _x == 0 && _y == 0 && _z == 0; } }

        public static Vector3d Unit { get { return new Vector3d(1, 1, 1); } }

        public static Vector3d Zero { get { return new Vector3d(); } }

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

        public static Vector3d XAxis { get { return new Vector3d(1, 0, 0); } }

        public static Vector3d YAxis { get { return new Vector3d(0, 1, 0); } }

        public static Vector3d ZAxis { get { return new Vector3d(0, 0, 1); } }

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

        public static Float AngleBetween(Vector3d vector1, Vector3d vector2)
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

        public static Vector3d operator -(Vector3d vector)
        {
            return new Vector3d(-vector._x, -vector._y, -vector._z);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
            _z = -_z;
        }

        public static Vector3d operator +(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3d Add(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x + vector2._x,
                                vector1._y + vector2._y,
                                vector1._z + vector2._z);
        }

        public static Vector3d operator -(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Vector3d Subtract(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x - vector2._x,
                                vector1._y - vector2._y,
                                vector1._z - vector2._z);
        }

        public static Point3d operator +(Vector3d vector, Point3d point)
        {
            return new Point3d(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3d Add(Vector3d vector, Point3d point)
        {
            return new Point3d(vector._x + point._x,
                               vector._y + point._y,
                               vector._z + point._z);
        }

        public static Point3d operator -(Vector3d vector, Point3d point)
        {
            return new Point3d(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Point3d Subtract(Vector3d vector, Point3d point)
        {
            return new Point3d(vector._x - point._x,
                               vector._y - point._y,
                               vector._z - point._z);
        }

        public static Vector3d operator *(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x * vector2._x, vector1._y * vector2._y, vector1._z * vector2._z);
        }

        public static Vector3d Multiply(Vector3d vector1, Vector3d vector2)
        {
            return vector1 * vector2;
        }

        public static Vector3d Multiply(Vector3d vector, Float scalar)
        {
            return new Vector3d(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3d operator *(Vector3d vector, Float scalar)
        {
            return new Vector3d(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3d operator *(Float scalar, Vector3d vector)
        {
            return new Vector3d(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3d Multiply(Float scalar, Vector3d vector)
        {
            return new Vector3d(vector._x * scalar,
                                vector._y * scalar,
                                vector._z * scalar);
        }

        public static Vector3d operator /(Vector3d vector1, Vector3d vector2)
        {
            return new Vector3d(vector1._x / vector2._x, vector1._y / vector2._y, vector1._z / vector2._z);
        }

        public static Vector3d Divide(Vector3d vector1, Vector3d vector2)
        {
            return vector1 / vector2;
        }

        public static Vector3d operator /(Vector3d vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        public static Vector3d Divide(Vector3d vector, Float scalar)
        {
            return vector * (Float)(1.0 / scalar);
        }

        //public static Vector3d operator *(Vector3d vector, Matrix3f matrix)
        //{
        //    return matrix.Transform(vector);
        //}

        //public static Vector3d Multiply(Vector3d vector, Matrix3f matrix)
        //{
        //    return matrix.Transform(vector);
        //}

        public static Float DotProduct(Vector3d vector1, Vector3d vector2)
        {
            return vector1._x * vector2._x +
                   vector1._y * vector2._y +
                   vector1._z * vector2._z;
        }

        public static Float AbsDotProduct(Vector3d vector1, Vector3d vector2)
        {
            return Math.Abs(vector1._x * vector2._x) +
                   Math.Abs(vector1._y * vector2._y) +
                   Math.Abs(vector1._z * vector2._z);
        }

        public static Vector3d CrossProduct(Vector3d vector1, Vector3d vector2)
        {
            Vector3d result;
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
        public Vector3d Reflect(Vector3d normal)
        {
            return this - 2 * DotProduct(this, normal) * normal;
        }

        public void Floor(Vector3d vector)
        {
            _x = Math.Min(_x, vector._x);
            _y = Math.Min(_y, vector._y);
            _z = Math.Min(_z, vector._z);
        }

        public void Ceiling(Vector3d vector)
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

        public static explicit operator Point3d(Vector3d vector)
        {
            return new Point3d(vector._x, vector._y, vector._z);
        }

        public static explicit operator Vector3f(Vector3d vector)
        {
            return new Vector3f((float)vector._x, (float)vector._y, (float)vector._z);
        }

        //public static explicit operator Vector2f(Vector3d vector)
        //{
        //    return new Vector2f(vector._x, vector._y);
        //}

        //public static explicit operator Size3f(Vector3d vector)
        //{
        //    return new Size3f(Math.Abs(vector._x), Math.Abs(vector._y), Math.Abs(vector._z));
        //}

        public static bool operator ==(Vector3d vector1, Vector3d vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z;
        }

        public static bool operator !=(Vector3d vector1, Vector3d vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector3d vector1, Vector3d vector2)
        {
            return vector1.X.Equals(vector2.X) &&
                   vector1.Y.Equals(vector2.Y) &&
                   vector1.Z.Equals(vector2.Z);
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Vector3d))
            {
                return false;
            }

            Vector3d value = (Vector3d)o;
            return Vector3d.Equals(this, value);
        }

        public bool Equals(Vector3d value)
        {
            return Vector3d.Equals(this, value);
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