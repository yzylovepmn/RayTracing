using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    public struct Size3f
    {
        public Size3f(Float x, Float y, Float z)
        {
            if (x < 0 || y < 0 || z < 0)
            {
                throw new System.ArgumentException("");
            }


            _x = x;
            _y = y;
            _z = z;
        }

        public static Size3f Empty
        {
            get
            {
                return s_empty;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _x < 0;
            }
        }

        public bool IsVolumeEmpty
        {
            get
            {
                return _x < 0 || (_x == 0 && _y == 0 && _z == 0);
            }
        }

        public Float X
        {
            get
            {
                return _x;
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException("");
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("");
                }

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
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException("");
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("");
                }

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
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException("");
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("");
                }

                _z = value;
            }
        }

        internal Float _x;
        internal Float _y;
        internal Float _z;

        public static explicit operator Vector3f(Size3f size)
        {
            return new Vector3f(size._x, size._y, size._z);
        }

        public static explicit operator Point3f(Size3f size)
        {
            return new Point3f(size._x, size._y, size._z);
        }

        private static Size3f CreateEmptySize3D()
        {
            Size3f empty = new Size3f();
            // Can't use setters because they throw on negative values
            empty._x = Float.NegativeInfinity;
            empty._y = Float.NegativeInfinity;
            empty._z = Float.NegativeInfinity;
            return empty;
        }

        private readonly static Size3f s_empty = CreateEmptySize3D();

        public static bool operator ==(Size3f size1, Size3f size2)
        {
            return size1.X == size2.X &&
                   size1.Y == size2.Y &&
                   size1.Z == size2.Z;
        }

        public static bool operator !=(Size3f size1, Size3f size2)
        {
            return !(size1 == size2);
        }

        public static bool Equals(Size3f size1, Size3f size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            else
            {
                return size1.X.Equals(size2.X) &&
                       size1.Y.Equals(size2.Y) &&
                       size1.Z.Equals(size2.Z);
            }
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Size3f))
            {
                return false;
            }

            Size3f value = (Size3f)o;
            return Size3f.Equals(this, value);
        }

        public bool Equals(Size3f value)
        {
            return Size3f.Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }
            else
            {
                // Perform field-by-field XOR of HashCodes
                return X.GetHashCode() ^
                       Y.GetHashCode() ^
                       Z.GetHashCode();
            }
        }
    }
}