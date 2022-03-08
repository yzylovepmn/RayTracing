using System;
using System.Windows;
using Float = System.Single;

namespace RayTracing.Core
{
    [Serializable]
    public struct Sizef
    {
        public Sizef(Float width, Float height)
        {
            if (width < 0 || height < 0)
            {
                throw new System.ArgumentException("size can not be negetive!");
            }

            _width = width;
            _height = height;
        }

        public static Sizef Empty
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

        public Float Width
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

        public Float Height
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

        public static explicit operator Vector2f(Sizef size)
        {
            return new Vector2f(size._width, size._height);
        }

        public static explicit operator Point2f(Sizef size)
        {
            return new Point2f(size._width, size._height);
        }

        static private Sizef CreateEmptySize()
        {
            Sizef size = new Sizef();
            // We can't set these via the property setters because negatives widths
            // are rejected in those APIs.
            size._width = Float.NegativeInfinity;
            size._height = Float.NegativeInfinity;
            return size;
        }
        private readonly static Sizef s_empty = CreateEmptySize();

        public static bool operator ==(Sizef size1, Sizef size2)
        {
            return size1.Width == size2.Width &&
                   size1.Height == size2.Height;
        }

        public static bool operator !=(Sizef size1, Sizef size2)
        {
            return !(size1 == size2);
        }
        public static bool Equals(Sizef size1, Sizef size2)
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
            if ((null == o) || !(o is Sizef))
            {
                return false;
            }

            Sizef value = (Sizef)o;
            return Sizef.Equals(this, value);
        }

        public bool Equals(Sizef value)
        {
            return Sizef.Equals(this, value);
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

        internal Float _width;
        internal Float _height;
    }
}