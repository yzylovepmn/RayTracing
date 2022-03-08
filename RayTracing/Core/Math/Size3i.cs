using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Size3i
    {
        public Size3i(Ele x, Ele y, Ele z)
        {
            if (x < 0 || y < 0 || z < 0)
            {
                throw new System.ArgumentException("");
            }


            _x = x;
            _y = y;
            _z = z;
        }

        public static Size3i Empty
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

        public Ele X
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

        public Ele Y
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

        public Ele Z
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

        internal Ele _x;
        internal Ele _y;
        internal Ele _z;

        public static explicit operator Vector3i(Size3i size)
        {
            return new Vector3i(size._x, size._y, size._z);
        }

        public static explicit operator Point3i(Size3i size)
        {
            return new Point3i(size._x, size._y, size._z);
        }

        private static Size3i CreateEmptySize3D()
        {
            Size3i empty = new Size3i();
            // Can't use setters because they throw on negative values
            empty._x = Ele.MinValue;
            empty._y = Ele.MinValue;
            empty._z = Ele.MinValue;
            return empty;
        }

        private readonly static Size3i s_empty = CreateEmptySize3D();

        public static bool operator ==(Size3i size1, Size3i size2)
        {
            return size1.X == size2.X &&
                   size1.Y == size2.Y &&
                   size1.Z == size2.Z;
        }

        public static bool operator !=(Size3i size1, Size3i size2)
        {
            return !(size1 == size2);
        }

        public static bool Equals(Size3i size1, Size3i size2)
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
            if ((null == o) || !(o is Size3i))
            {
                return false;
            }

            Size3i value = (Size3i)o;
            return Size3i.Equals(this, value);
        }

        public bool Equals(Size3i value)
        {
            return Size3i.Equals(this, value);
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