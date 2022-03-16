using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Double;

namespace RayTracing.Core
{
    public class AxisAlignedBox3d
    {
        public static readonly AxisAlignedBox3d Empty = new AxisAlignedBox3d(false);
        public static readonly AxisAlignedBox3d Zero = new AxisAlignedBox3d(0);
        public static readonly AxisAlignedBox3d UnitPositive = new AxisAlignedBox3d(1);
        public static readonly AxisAlignedBox3d Infinite =
            new AxisAlignedBox3d(Float.MinValue, Float.MinValue, Float.MinValue, Float.MaxValue, Float.MaxValue, Float.MaxValue);

        // only for empty
        private AxisAlignedBox3d(bool flag)
        {
            _min = new Vector3d(Float.MaxValue, Float.MaxValue, Float.MaxValue);
            _max = new Vector3d(Float.MinValue, Float.MinValue, Float.MinValue);
        }

        public AxisAlignedBox3d(Float cubeSize)
        {
            cubeSize = Math.Abs(cubeSize);
            _min = new Vector3d(0, 0, 0);
            _max = new Vector3d(cubeSize, cubeSize, cubeSize);
        }

        public AxisAlignedBox3d(Float xMin, Float yMin, Float zMin, Float xMax, Float yMax, Float zMax)
        {
            _min = new Vector3d(xMin, yMin, zMin);
            _max = new Vector3d(xMax, yMax, zMax);
        }

        public AxisAlignedBox3d(Float xSize, Float ySize, Float zSize)
        {
            _min = new Vector3d(0, 0, 0);
            _max = new Vector3d(Math.Abs(xSize), Math.Abs(ySize), Math.Abs(zSize));
        }

        public AxisAlignedBox3d(Vector3d min, Vector3d max)
        {
            _min = new Vector3d(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y), Math.Min(min.Z, max.Z));
            _max = new Vector3d(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y), Math.Max(min.Z, max.Z));
        }

        public AxisAlignedBox3d(Point3d center, Float halfSize)
        {
            halfSize = Math.Abs(halfSize);
            _min = new Vector3d(center.X - halfSize, center.Y - halfSize, center.Z - halfSize);
            _max = new Vector3d(center.X + halfSize, center.Y + halfSize, center.Z + halfSize);
        }

        public AxisAlignedBox3d(Point3d center, Vector3d halfSize)
        {
            var halfSizeX = Math.Abs(halfSize.X);
            var halfSizeY = Math.Abs(halfSize.Y);
            var halfSizeZ = Math.Abs(halfSize.Z);
            _min = new Vector3d(center.X - halfSizeX, center.Y - halfSizeY, center.Z - halfSizeZ);
            _max = new Vector3d(center.X + halfSizeX, center.Y + halfSizeY, center.Z + halfSizeZ);
        }

        public AxisAlignedBox3d(Point3d center, Float halfSizeX, Float halfSizeY, Float halfSizeZ)
        {
            halfSizeX = Math.Abs(halfSizeX);
            halfSizeY = Math.Abs(halfSizeY);
            halfSizeZ = Math.Abs(halfSizeZ);
            _min = new Vector3d(center.X - halfSizeX, center.Y - halfSizeY, center.Z - halfSizeZ);
            _max = new Vector3d(center.X + halfSizeX, center.Y + halfSizeY, center.Z + halfSizeZ);
        }

        public AxisAlignedBox3d(Point3d center)
        {
            _min = _max = (Vector3d)center;
        }

        public bool IsEmpty { get { return _min.X > _max.X || _min.Y > _max.Y || _min.Z > _max.Z; } }

        public bool IsInfinite { get { return this == Infinite; } }

        public bool IsZero { get { return _min.X == _max.X || _min.Y == _max.Y || _min.Z == _max.Z; } }

        public Point3d Center { get { return (Point3d)((_min + _max) * (Float)0.5); } }

        public Vector3d Diagonal { get { return _max - _min; } }

        public Vector3d Extents { get { return new Vector3d(_max.X - _min.X, _max.Y - _min.Y, _max.Z - _min.Z) * (Float)0.5; } }

        public Float MaxDimension { get { return Math.Max(SizeX, Math.Max(SizeY, SizeZ)); } }

        public Float SizeX { get { return Math.Max(_max.X - _min.X, 0); } }

        public Float SizeY { get { return Math.Max(_max.Y - _min.Y, 0); } }

        public Float SizeZ { get { return Math.Max(_max.Z - _min.Z, 0); } }

        public Float Volume { get { return SizeX * SizeY * SizeZ; } }

        public Float DiagonalLength { get { return Diagonal.Length; } }

        public Vector3d Max { get { return _max; } }
        private Vector3d _max;

        public Vector3d Min { get { return _min; } }
        private Vector3d _min;

        /// <summary>
        /// Returns point on face/edge/corner. For each coord value neg==min, 0==center, pos==max
        /// </summary>
        public Vector3d Point(int xi, int yi, int zi)
        {
            Float x = (xi < 0) ? _min.X : ((xi == 0) ? ((Float)0.5 * (_min.X + _max.X)) : _max.X);
            Float y = (yi < 0) ? _min.Y : ((yi == 0) ? ((Float)0.5 * (_min.Y + _max.Y)) : _max.Y);
            Float z = (zi < 0) ? _min.Z : ((zi == 0) ? ((Float)0.5 * (_min.Z + _max.Z)) : _max.Z);
            return new Vector3d(x, y, z);
        }

        public void Union(Point3d p)
        {
            _min.X = Math.Min(p.X, _min.X);
            _min.Y = Math.Min(p.Y, _min.Y);
            _min.Z = Math.Min(p.Z, _min.Z);
            _max.X = Math.Max(p.X, _max.X);
            _max.Y = Math.Max(p.Y, _max.Y);
            _max.Z = Math.Max(p.Z, _max.Z);
        }

        public void Union(Vector3d v)
        {
            _min.X = Math.Min(v.X, _min.X);
            _min.Y = Math.Min(v.Y, _min.Y);
            _min.Z = Math.Min(v.Z, _min.Z);
            _max.X = Math.Max(v.X, _max.X);
            _max.Y = Math.Max(v.Y, _max.Y);
            _max.Z = Math.Max(v.Z, _max.Z);
        }

        public void Union(AxisAlignedBox3d box)
        {
            if (IsEmpty)
            {
                _min = box._min;
                _max = box._max;
            }
            else if (box.IsEmpty) return;
            else
            {
                _min.X = Math.Min(box._min.X, _min.X);
                _min.Y = Math.Min(box._min.Y, _min.Y);
                _min.Z = Math.Min(box._min.Z, _min.Z);
                _max.X = Math.Max(box._max.X, _max.X);
                _max.Y = Math.Max(box._max.Y, _max.Y);
                _max.Z = Math.Max(box._max.Z, _max.Z);
            }
        }

        public bool Contains(Vector3d v)
        {
            return _min.X <= v.X && _max.X >= v.X
                && _min.Y <= v.Y && _max.Y >= v.Y
                && _min.Z <= v.Z && _max.Z >= v.Z;
        }

        public bool Contains(Point3d p)
        {
            return _min.X <= p.X && _max.X >= p.X
                && _min.Y <= p.Y && _max.Y >= p.Y
                && _min.Z <= p.Z && _max.Z >= p.Z;
        }

        public bool Contains(AxisAlignedBox3d box)
        {
            return _min.X <= box._min.X && _max.X >= box._max.X
                && _min.Y <= box._min.Y && _max.Y >= box._max.Y
                && _min.Z <= box._min.Z && _max.Z >= box._max.Z;
        }

        public bool IsIntersects(AxisAlignedBox3d box)
        {
            return !((box._max.X <= _min.X) || (box._min.X >= _max.X)
                || (box._max.Y <= _min.Y) || (box._min.Y >= _max.Y)
                || (box._max.Z <= _min.Z) || (box._min.Z >= _max.Z));
        }

        public AxisAlignedBox3d IntersectWith(AxisAlignedBox3d box)
        {
            if (IsInfinite)
                return box;
            if (box.IsInfinite)
                return this;

            AxisAlignedBox3d intersect = new AxisAlignedBox3d(
                Math.Max(_min.X, box._min.X), Math.Max(_min.Y, box._min.Y), Math.Max(_min.Z, box._min.Z),
                Math.Min(_max.X, box._max.X), Math.Min(_max.Y, box._max.Y), Math.Min(_max.Z, box._max.Z));
            if (intersect.SizeX <= 0 || intersect.SizeY <= 0 || intersect.SizeZ <= 0)
                return Empty;
            else
                return intersect;
        }

        public double DistanceSquared(Point3d p)
        {
            Float dx = (p.X < _min.X) ? _min.X - p.X : (p.X > _max.X ? p.X - _max.X : 0);
            Float dy = (p.Y < _min.Y) ? _min.Y - p.Y : (p.Y > _max.Y ? p.Y - _max.Y : 0);
            Float dz = (p.Z < _min.Z) ? _min.Z - p.Z : (p.Z > _max.Z ? p.Z - _max.Z : 0);
            return dx * dx + dy * dy + dz * dz;
        }

        public Float Distance(Point3d p)
        {
            return (Float)Math.Sqrt(DistanceSquared(p));
        }

        public Point3d NearestPoint(Point3d p)
        {
            Float x = (p.X < _min.X) ? _min.X : (p.X > _max.X ? _max.X : p.X);
            Float y = (p.Y < _min.Y) ? _min.Y : (p.Y > _max.Y ? _max.Y : p.Y);
            Float z = (p.Z < _min.Z) ? _min.Z : (p.Z > _max.Z ? _max.Z : p.Z);
            return new Point3d(x, y, z);
        }

        public void ScaleAt(Vector3d scale, Point3d scaleCenter)
        {
            if (IsInfinite) return;
            var sc = (Vector3d)scaleCenter;
            var left = _min - sc;
            var right = _max - sc;
            left *= scale;
            right *= scale;
            _min = sc + left;
            _max = sc + right;
        }

        public void Scale(Vector3d scale)
        {
            ScaleAt(scale, new Point3d());
        }

        //public static AxisAlignedBox3d Multiply(AxisAlignedBox3d box, Matrix3f matrix)
        //{
        //    return matrix.Transform(box);
        //}

        //public static AxisAlignedBox3d operator *(AxisAlignedBox3d box, Matrix3f matrix)
        //{
        //    return matrix.Transform(box);
        //}

        public static bool operator ==(AxisAlignedBox3d left, AxisAlignedBox3d right)
        {
            return left._min == right._min && left._max == right._max;
        }

        public static bool operator !=(AxisAlignedBox3d left, AxisAlignedBox3d right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is AxisAlignedBox3d)
                return this == (AxisAlignedBox3d)obj;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            { // Overflow is fine, just wrap
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ _min.GetHashCode();
                hash = (hash * 16777619) ^ _max.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("x[{0:F8},{1:F8}] y[{2:F8},{3:F8}] z[{4:F8},{5:F8}]", _min.X, _max.X, _min.Y, _max.Y, _min.Z, _max.Z);
        }
    }
}