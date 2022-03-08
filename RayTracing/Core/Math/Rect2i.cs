using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ele = System.Int32;

namespace RayTracing.Core
{
    [Serializable]
    public struct Rect2i
    {
        public Rect2i(Point2i location,
                    Sizei size)
        {
            if (size.IsEmpty)
            {
                this = s_empty;
            }
            else
            {
                _x = location._x;
                _y = location._y;
                _width = size._width;
                _height = size._height;
            }
        }

        public Rect2i(Ele left,
                    Ele top,
                    Ele right,
                    Ele bottom)
        {
            if (bottom < top || right < left)
            {
                throw new System.ArgumentException();
            }

            _x = left;
            _y = top;
            _width = right - left;
            _height = bottom - top;
        }

        public Rect2i(Point2i point1,
                    Point2i point2)
        {
            _x = Math.Min(point1._x, point2._x);
            _y = Math.Min(point1._y, point2._y);

            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
            _width = Math.Max(Math.Max(point1._x, point2._x) - _x, 0);
            _height = Math.Max(Math.Max(point1._y, point2._y) - _y, 0);
        }

        public Rect2i(Point2i point,
                    Vector2i vector) : this(point, point + vector)
        {
        }

        public Rect2i(Sizei size)
        {
            if (size.IsEmpty)
            {
                this = s_empty;
            }
            else
            {
                _x = _y = 0;
                _width = size.Width;
                _height = size.Height;
            }
        }

        public static Rect2i Empty
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
#if DEBUG
                // The funny width and height tests are to handle NaNs
                Debug.Assert((!(_width < 0) && !(_height < 0)) || (this == Empty));
#endif
                return _width < 0;
            }
        }

        public Point2i Location
        {
            get
            {
                return new Point2i(_x, _y);
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException();
                }

                _x = value._x;
                _y = value._y;
            }
        }

        public Sizei Size
        {
            get
            {
                if (IsEmpty)
                    return Sizei.Empty;
                return new Sizei(_width, _height);
            }
            set
            {
                if (value.IsEmpty)
                {
                    this = s_empty;
                }
                else
                {
                    if (IsEmpty)
                    {
                        throw new System.InvalidOperationException();
                    }

                    _width = value._width;
                    _height = value._height;
                }
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
                    throw new System.InvalidOperationException();
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
                    throw new System.InvalidOperationException();
                }

                _y = value;
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
                    throw new System.ArgumentException();
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
                    throw new System.ArgumentException();
                }

                _height = value;
            }
        }

        public Ele Left
        {
            get
            {
                return _x;
            }
        }

        public Ele Top
        {
            get
            {
                return _y;
            }
        }

        public Ele Right
        {
            get
            {
                if (IsEmpty)
                {
                    return Ele.MinValue;
                }

                return _x + _width;
            }
        }

        public Ele Bottom
        {
            get
            {
                if (IsEmpty)
                {
                    return Ele.MinValue;
                }

                return _y + _height;
            }
        }

        public Point2i BottomLeft
        {
            get
            {
                return new Point2i(Left, Bottom);
            }
        }

        public Point2i BottomRight
        {
            get
            {
                return new Point2i(Right, Bottom);
            }
        }

        public Point2i TopLeft
        {
            get
            {
                return new Point2i(Left, Top);
            }
        }

        public Point2i TopRight
        {
            get
            {
                return new Point2i(Right, Top);
            }
        }

        public void Extents(Ele length)
        {
            var dl = length * 2;
            _x -= length;
            _y -= length;
            _width += dl;
            _height += dl;
        }

        public bool Contains(Point2i point)
        {
            return Contains(point._x, point._y);
        }

        public bool Contains(Ele x, Ele y)
        {
            if (IsEmpty)
            {
                return false;
            }

            return ContainsInternal(x, y);
        }

        public bool Contains(Rect2i rect)
        {
            if (IsEmpty || rect.IsEmpty)
            {
                return false;
            }

            return (_x <= rect._x &&
                    _y <= rect._y &&
                    _x + _width >= rect._x + rect._width &&
                    _y + _height >= rect._y + rect._height);
        }

        public bool IntersectsWith(Rect2i rect)
        {
            if (IsEmpty || rect.IsEmpty)
            {
                return false;
            }

            return (rect.Left < Right) &&
                   (rect.Right > Left) &&
                   (rect.Top < Bottom) &&
                   (rect.Bottom > Top);
        }

        public void Intersect(Rect2i rect)
        {
            if (!this.IntersectsWith(rect))
            {
                this = Empty;
            }
            else
            {
                Ele left = Math.Max(Left, rect.Left);
                Ele top = Math.Max(Top, rect.Top);

                //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
                _width = Math.Max(Math.Min(Right, rect.Right) - left, 0);
                _height = Math.Max(Math.Min(Bottom, rect.Bottom) - top, 0);

                _x = left;
                _y = top;
            }
        }

        public static Rect2i Intersect(Rect2i rect1, Rect2i rect2)
        {
            rect1.Intersect(rect2);
            return rect1;
        }

        public void Union(Rect2i rect)
        {
            if (IsEmpty)
            {
                this = rect;
            }
            else if (!rect.IsEmpty)
            {
                Ele left = Math.Min(Left, rect.Left);
                Ele top = Math.Min(Top, rect.Top);


                // We need this check so that the math does not result in NaN
                //if ((rect.Width == Float.PositiveInfinity) || (Width == Float.PositiveInfinity))
                //{
                //    _width = Float.PositiveInfinity;
                //}
                //else
                //{
                    //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
                    Ele maxRight = Math.Max(Right, rect.Right);
                    _width = Math.Max(maxRight - left, 0);
                //}

                // We need this check so that the math does not result in NaN
                //if ((rect.Height == Float.PositiveInfinity) || (Height == Float.PositiveInfinity))
                //{
                //    _height = Float.PositiveInfinity;
                //}
                //else
                //{
                    //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
                    Ele maxBottom = Math.Max(Bottom, rect.Bottom);
                    _height = Math.Max(maxBottom - top, 0);
                //}

                _x = left;
                _y = top;
            }
        }

        public static Rect2i Union(Rect2i rect1, Rect2i rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public void Union(Point2i point)
        {
            Union(new Rect2i(point, point));
        }

        public static Rect2i Union(Rect2i rect, Point2i point)
        {
            rect.Union(new Rect2i(point, point));
            return rect;
        }

        public void Offset(Vector2i offsetVector)
        {
            if (IsEmpty)
            {
                throw new System.InvalidOperationException();
            }

            _x += offsetVector._x;
            _y += offsetVector._y;
        }

        public void Offset(Ele offsetX, Ele offsetY)
        {
            if (IsEmpty)
            {
                throw new System.InvalidOperationException();
            }

            _x += offsetX;
            _y += offsetY;
        }

        public static Rect2i Offset(Rect2i rect, Vector2i offsetVector)
        {
            rect.Offset(offsetVector.X, offsetVector.Y);
            return rect;
        }

        public static Rect2i Offset(Rect2i rect, Ele offsetX, Ele offsetY)
        {
            rect.Offset(offsetX, offsetY);
            return rect;
        }

        public void Inflate(Sizei size)
        {
            Inflate(size._width, size._height);
        }

        public void Inflate(Ele width, Ele height)
        {
            if (IsEmpty)
            {
                throw new System.InvalidOperationException();
            }

            _x -= width;
            _y -= height;

            // Do two additions rather than multiplication by 2 to avoid spurious overflow
            // That is: (A + 2 * B) != ((A + B) + B) if 2*B overflows.
            // Note that multiplication by 2 might work in this case because A should start
            // positive & be "clamped" to positive after, but consider A = Inf & B = -MAX.
            _width += width;
            _width += width;
            _height += height;
            _height += height;

            // We catch the case of inflation by less than -width/2 or -height/2 here.  This also
            // maintains the invariant that either the Rect is Empty or _width and _height are
            // non-negative, even if the user parameters were NaN, though this isn't strictly maintained
            // by other methods.
            if (!(_width >= 0 && _height >= 0))
            {
                this = s_empty;
            }
        }

        public static Rect2i Inflate(Rect2i rect, Sizei size)
        {
            rect.Inflate(size._width, size._height);
            return rect;
        }

        public static Rect2i Inflate(Rect2i rect, Ele width, Ele height)
        {
            rect.Inflate(width, height);
            return rect;
        }

        public void Scale(Ele scaleX, Ele scaleY)
        {
            if (IsEmpty)
            {
                return;
            }

            _x *= scaleX;
            _y *= scaleY;
            _width *= scaleX;
            _height *= scaleY;

            // If the scale in the X dimension is negative, we need to normalize X and Width
            if (scaleX < 0)
            {
                // Make X the left-most edge again
                _x += _width;

                // and make Width positive
                _width *= -1;
            }

            // Do the same for the Y dimension
            if (scaleY < 0)
            {
                // Make Y the top-most edge again
                _y += _height;

                // and make Height positive
                _height *= -1;
            }
        }

        private bool ContainsInternal(Ele x, Ele y)
        {
            return ((x >= _x) && (x - _width <= _x) &&
                    (y >= _y) && (y - _height <= _y));
        }

        static private Rect2i CreateEmptyRect()
        {
            Rect2i rect = new Rect2i();
            // We can't set these via the property setters because negatives widths
            // are rejected in those APIs.
            rect._x = Ele.MaxValue;
            rect._y = Ele.MaxValue;
            rect._width = Ele.MinValue;
            rect._height = Ele.MinValue;
            return rect;
        }

        private readonly static Rect2i s_empty = CreateEmptyRect();

        public static bool operator ==(Rect2i rect1, Rect2i rect2)
        {
            return rect1.X == rect2.X &&
                   rect1.Y == rect2.Y &&
                   rect1.Width == rect2.Width &&
                   rect1.Height == rect2.Height;
        }

        public static bool operator !=(Rect2i rect1, Rect2i rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(Rect2i rect1, Rect2i rect2)
        {
            if (rect1.IsEmpty)
            {
                return rect2.IsEmpty;
            }
            else
            {
                return rect1.X.Equals(rect2.X) &&
                       rect1.Y.Equals(rect2.Y) &&
                       rect1.Width.Equals(rect2.Width) &&
                       rect1.Height.Equals(rect2.Height);
            }
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Rect2i))
            {
                return false;
            }

            Rect2i value = (Rect2i)o;
            return Rect2i.Equals(this, value);
        }

        public bool Equals(Rect2i value)
        {
            return Rect2i.Equals(this, value);
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
                       Width.GetHashCode() ^
                       Height.GetHashCode();
            }
        }

        internal Ele _x;
        internal Ele _y;
        internal Ele _width;
        internal Ele _height;
    }
}