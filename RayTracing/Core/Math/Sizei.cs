using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Sizei
    {
        public Sizei(Ele width, Ele height)
        {
            if (width < 0 || height < 0)
            {
                throw new System.ArgumentException("size can not be negetive!");
            }

            _width = width;
            _height = height;
        }

        public static Sizei Empty
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
                return _width < 0;
            }
        }

        public Ele Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException();
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("width can not be nagetive!");
                }

                _width = value;
            }
        }

        public Ele Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException();
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("heigth can not be nagetive!");
                }

                _height = value;
            }
        }

        public static explicit operator Vector2i(Sizei size)
        {
            return new Vector2i(size._width, size._height);
        }

        public static explicit operator Point2i(Sizei size)
        {
            return new Point2i(size._width, size._height);
        }

        static private Sizei CreateEmptySize()
        {
            Sizei size = new Sizei();
            // We can't set these via the property setters because negatives widths
            // are rejected in those APIs.
            size._width = Ele.MinValue;
            size._height = Ele.MinValue;
            return size;
        }
        private readonly static Sizei s_empty = CreateEmptySize();

        public static bool operator ==(Sizei size1, Sizei size2)
        {
            return size1.Width == size2.Width &&
                   size1.Height == size2.Height;
        }

        public static bool operator !=(Sizei size1, Sizei size2)
        {
            return !(size1 == size2);
        }
        public static bool Equals(Sizei size1, Sizei size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            else
            {
                return size1.Width.Equals(size2.Width) &&
                       size1.Height.Equals(size2.Height);
            }
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Sizei))
            {
                return false;
            }

            Sizei value = (Sizei)o;
            return Sizei.Equals(this, value);
        }

        public bool Equals(Sizei value)
        {
            return Sizei.Equals(this, value);
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
                return Width.GetHashCode() ^
                       Height.GetHashCode();
            }
        }

        internal Ele _width;
        internal Ele _height;
    }
}