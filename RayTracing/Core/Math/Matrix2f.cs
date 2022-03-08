using System;
using System.Diagnostics;
using System.Windows;
using System.Xml.Linq;
using Float = System.Single;

namespace RayTracing.Core
{
    internal enum MatrixTypes
    {
        TRANSFORM_IS_IDENTITY = 0,
        TRANSFORM_IS_TRANSLATION = 1,
        TRANSFORM_IS_SCALING = 2,
        TRANSFORM_IS_UNKNOWN = 4
    }

    /// <summary>
    /// Left hand
    /// </summary>
    public struct Matrix2f
    {
        private static Matrix2f s_identity = CreateIdentity();

        public Matrix2f(Float m11, Float m12,
                      Float m21, Float m22,
                      Float offsetX, Float offsetY)
        {
            this._m11 = m11;
            this._m12 = m12;
            this._m21 = m21;
            this._m22 = m22;
            this._offsetX = offsetX;
            this._offsetY = offsetY;
            _type = MatrixTypes.TRANSFORM_IS_UNKNOWN;

            DeriveMatrixType();
        }

        public static Matrix2f Identity
        {
            get
            {
                return s_identity;
            }
        }

        public void SetIdentity()
        {
            _type = MatrixTypes.TRANSFORM_IS_IDENTITY;
        }

        public bool IsIdentity
        {
            get
            {
                return (_type == MatrixTypes.TRANSFORM_IS_IDENTITY ||
                        (_m11 == 1 && _m12 == 0 && _m21 == 0 && _m22 == 1 && _offsetX == 0 && _offsetY == 0));
            }
        }

        public static Matrix2f operator *(Matrix2f trans1, Matrix2f trans2)
        {
            MultiplyMatrix(ref trans1, ref trans2);
#if DEBUG
            trans1.Debug_CheckType();
#endif
            return trans1;
        }

        public static Matrix2f Multiply(Matrix2f trans1, Matrix2f trans2)
        {
            MultiplyMatrix(ref trans1, ref trans2);
#if DEBUG
            trans1.Debug_CheckType();
#endif
            return trans1;
        }

        internal static void MultiplyMatrix(ref Matrix2f matrix1, ref Matrix2f matrix2)
        {
            MatrixTypes type1 = matrix1._type;
            MatrixTypes type2 = matrix2._type;

            // Check for idents

            // If the second is ident, we can just return
            if (type2 == MatrixTypes.TRANSFORM_IS_IDENTITY)
            {
                return;
            }

            // If the first is ident, we can just copy the memory across.
            if (type1 == MatrixTypes.TRANSFORM_IS_IDENTITY)
            {
                matrix1 = matrix2;
                return;
            }

            // Optimize for translate case, where the second is a translate
            if (type2 == MatrixTypes.TRANSFORM_IS_TRANSLATION)
            {
                // 2 additions
                matrix1._offsetX += matrix2._offsetX;
                matrix1._offsetY += matrix2._offsetY;

                // If matrix 1 wasn't unknown we added a translation
                if (type1 != MatrixTypes.TRANSFORM_IS_UNKNOWN)
                {
                    matrix1._type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
                }

                return;
            }

            // Check for the first value being a translate
            if (type1 == MatrixTypes.TRANSFORM_IS_TRANSLATION)
            {
                // Save off the old offsets
                float offsetX = matrix1._offsetX;
                float offsetY = matrix1._offsetY;

                // Copy the matrix
                matrix1 = matrix2;

                matrix1._offsetX = offsetX * matrix2._m11 + offsetY * matrix2._m21 + matrix2._offsetX;
                matrix1._offsetY = offsetX * matrix2._m12 + offsetY * matrix2._m22 + matrix2._offsetY;

                if (type2 == MatrixTypes.TRANSFORM_IS_UNKNOWN)
                {
                    matrix1._type = MatrixTypes.TRANSFORM_IS_UNKNOWN;
                }
                else
                {
                    matrix1._type = MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION;
                }
                return;
            }

            // The following code combines the type of the transformations so that the high nibble
            // is "this"'s type, and the low nibble is mat's type.  This allows for a switch rather
            // than nested switches.

            // trans1._type |  trans2._type
            //  7  6  5  4   |  3  2  1  0
            int combinedType = ((int)type1 << 4) | (int)type2;

            switch (combinedType)
            {
                case 34:  // S * S
                    // 2 multiplications
                    matrix1._m11 *= matrix2._m11;
                    matrix1._m22 *= matrix2._m22;
                    return;

                case 35:  // S * S|T
                    matrix1._m11 *= matrix2._m11;
                    matrix1._m22 *= matrix2._m22;
                    matrix1._offsetX = matrix2._offsetX;
                    matrix1._offsetY = matrix2._offsetY;

                    // Transform set to Translate and Scale
                    matrix1._type = MatrixTypes.TRANSFORM_IS_TRANSLATION | MatrixTypes.TRANSFORM_IS_SCALING;
                    return;

                case 50: // S|T * S
                    matrix1._m11 *= matrix2._m11;
                    matrix1._m22 *= matrix2._m22;
                    matrix1._offsetX *= matrix2._m11;
                    matrix1._offsetY *= matrix2._m22;
                    return;

                case 51: // S|T * S|T
                    matrix1._m11 *= matrix2._m11;
                    matrix1._m22 *= matrix2._m22;
                    matrix1._offsetX = matrix2._m11 * matrix1._offsetX + matrix2._offsetX;
                    matrix1._offsetY = matrix2._m22 * matrix1._offsetY + matrix2._offsetY;
                    return;
                case 36: // S * U
                case 52: // S|T * U
                case 66: // U * S
                case 67: // U * S|T
                case 68: // U * U
                    matrix1 = new Matrix2f(
                        matrix1._m11 * matrix2._m11 + matrix1._m12 * matrix2._m21,
                        matrix1._m11 * matrix2._m12 + matrix1._m12 * matrix2._m22,

                        matrix1._m21 * matrix2._m11 + matrix1._m22 * matrix2._m21,
                        matrix1._m21 * matrix2._m12 + matrix1._m22 * matrix2._m22,

                        matrix1._offsetX * matrix2._m11 + matrix1._offsetY * matrix2._m21 + matrix2._offsetX,
                        matrix1._offsetX * matrix2._m12 + matrix1._offsetY * matrix2._m22 + matrix2._offsetY);
                    return;
            }
        }

        public void Append(Matrix2f matrix)
        {
            this *= matrix;
        }

        public void Prepend(Matrix2f matrix)
        {
            this = matrix * this;
        }

        public void Rotate(Float angle)
        {
            angle %= 360.0F; // Doing the modulo before converting to radians reduces total error
            this *= CreateRotationRadians(angle * (Float)(Math.PI / 180.0));
        }

        public void RotatePrepend(Float angle)
        {
            angle %= 360.0F; // Doing the modulo before converting to radians reduces total error
            this = CreateRotationRadians(angle * (Float)(Math.PI / 180.0)) * this;
        }

        public void RotateAt(Float angle, Float centerX, Float centerY)
        {
            angle %= 360.0F; // Doing the modulo before converting to radians reduces total error
            this *= CreateRotationRadians(angle * (Float)(Math.PI / 180.0), centerX, centerY);
        }

        public void RotateAtPrepend(Float angle, Float centerX, Float centerY)
        {
            angle %= 360.0F; // Doing the modulo before converting to radians reduces total error
            this = CreateRotationRadians(angle * (Float)(Math.PI / 180.0), centerX, centerY) * this;
        }

        public void Scale(Float scaleX, Float scaleY)
        {
            this *= CreateScaling(scaleX, scaleY);
        }

        public void ScalePrepend(Float scaleX, Float scaleY)
        {
            this = CreateScaling(scaleX, scaleY) * this;
        }

        public void ScaleAt(Float scaleX, Float scaleY, Float centerX, Float centerY)
        {
            this *= CreateScaling(scaleX, scaleY, centerX, centerY);
        }

        public void ScaleAtPrepend(Float scaleX, Float scaleY, Float centerX, Float centerY)
        {
            this = CreateScaling(scaleX, scaleY, centerX, centerY) * this;
        }

        public void Skew(Float skewX, Float skewY)
        {
            skewX %= 360;
            skewY %= 360;
            this *= CreateSkewRadians(skewX * (Float)(Math.PI / 180.0),
                                      skewY * (Float)(Math.PI / 180.0));
        }

        public void SkewPrepend(Float skewX, Float skewY)
        {
            skewX %= 360;
            skewY %= 360;
            this = CreateSkewRadians(skewX * (Float)(Math.PI / 180.0),
                                     skewY * (Float)(Math.PI / 180.0)) * this;
        }

        public void Translate(Float offsetX, Float offsetY)
        {
            //
            // / a b 0 \   / 1 0 0 \    / a      b       0 \
            // | c d 0 | * | 0 1 0 | = |  c      d       0 |
            // \ e f 1 /   \ x y 1 /    \ e+x    f+y     1 /
            //
            // (where e = _offsetX and f == _offsetY)
            //

            if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
            {
                // Values would be incorrect if matrix was created using default constructor.
                // or if SetIdentity was called on a matrix which had values.
                //
                SetMatrix(1, 0,
                          0, 1,
                          offsetX, offsetY,
                          MatrixTypes.TRANSFORM_IS_TRANSLATION);
            }
            else if (_type == MatrixTypes.TRANSFORM_IS_UNKNOWN)
            {
                _offsetX += offsetX;
                _offsetY += offsetY;
            }
            else
            {
                _offsetX += offsetX;
                _offsetY += offsetY;

                // If matrix wasn't unknown we added a translation
                _type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
            }

#if DEBUG
            Debug_CheckType();
#endif
        }

        public void TranslatePrepend(Float offsetX, Float offsetY)
        {
            this = CreateTranslation(offsetX, offsetY) * this;
        }

        public Point2f Transform(Point2f point)
        {
            Point2f newPoint = point;
            MultiplyPoint(ref newPoint._x, ref newPoint._y);
            return newPoint;
        }

        public void Transform(Point2f[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    MultiplyPoint(ref points[i]._x, ref points[i]._y);
                }
            }
        }

        public Vector2f Transform(Vector2f vector)
        {
            Vector2f newVector = vector;
            MultiplyVector(ref newVector._x, ref newVector._y);
            return newVector;
        }

        public void Transform(Vector2f[] vectors)
        {
            if (vectors != null)
            {
                for (int i = 0; i < vectors.Length; i++)
                {
                    MultiplyVector(ref vectors[i]._x, ref vectors[i]._y);
                }
            }
        }

        public Float Determinant
        {
            get
            {
                switch (_type)
                {
                    case MatrixTypes.TRANSFORM_IS_IDENTITY:
                    case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                        return 1.0F;
                    case MatrixTypes.TRANSFORM_IS_SCALING:
                    case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                        return (_m11 * _m22);
                    default:
                        return (_m11 * _m22) - (_m12 * _m21);
                }
            }
        }

        public bool HasInverse
        {
            get
            {
                return !MathUtil.IsZero(Determinant);
            }
        }

        public void Invert()
        {
            Float determinant = Determinant;

            if (MathUtil.IsZero(determinant))
            {
                throw new System.InvalidOperationException();
            }

            // Inversion does not change the type of a matrix.
            switch (_type)
            {
                case MatrixTypes.TRANSFORM_IS_IDENTITY:
                    break;
                case MatrixTypes.TRANSFORM_IS_SCALING:
                    {
                        _m11 = 1.0F / _m11;
                        _m22 = 1.0F / _m22;
                    }
                    break;
                case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    _offsetX = -_offsetX;
                    _offsetY = -_offsetY;
                    break;
                case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    {
                        _m11 = 1.0F / _m11;
                        _m22 = 1.0F / _m22;
                        _offsetX = -_offsetX * _m11;
                        _offsetY = -_offsetY * _m22;
                    }
                    break;
                default:
                    {
                        Float invdet = 1.0F / determinant;
                        SetMatrix(_m22 * invdet,
                                  -_m12 * invdet,
                                  -_m21 * invdet,
                                  _m11 * invdet,
                                  (_m21 * _offsetY - _offsetX * _m22) * invdet,
                                  (_offsetX * _m12 - _m11 * _offsetY) * invdet,
                                  MatrixTypes.TRANSFORM_IS_UNKNOWN);
                    }
                    break;
            }
        }

        public Float M11
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 1.0F;
                }
                else
                {
                    return _m11;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(value, 0,
                              0, 1,
                              0, 0,
                              MatrixTypes.TRANSFORM_IS_SCALING);
                }
                else
                {
                    _m11 = value;
                    if (_type != MatrixTypes.TRANSFORM_IS_UNKNOWN)
                    {
                        _type |= MatrixTypes.TRANSFORM_IS_SCALING;
                    }
                }
            }
        }

        public Float M12
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 0;
                }
                else
                {
                    return _m12;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(1, value,
                              0, 1,
                              0, 0,
                              MatrixTypes.TRANSFORM_IS_UNKNOWN);
                }
                else
                {
                    _m12 = value;
                    _type = MatrixTypes.TRANSFORM_IS_UNKNOWN;
                }
            }
        }

        public Float M21
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 0;
                }
                else
                {
                    return _m21;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(1, 0,
                              value, 1,
                              0, 0,
                              MatrixTypes.TRANSFORM_IS_UNKNOWN);
                }
                else
                {
                    _m21 = value;
                    _type = MatrixTypes.TRANSFORM_IS_UNKNOWN;
                }
            }
        }

        public Float M22
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 1.0F;
                }
                else
                {
                    return _m22;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(1, 0,
                              0, value,
                              0, 0,
                              MatrixTypes.TRANSFORM_IS_SCALING);
                }
                else
                {
                    _m22 = value;
                    if (_type != MatrixTypes.TRANSFORM_IS_UNKNOWN)
                    {
                        _type |= MatrixTypes.TRANSFORM_IS_SCALING;
                    }
                }
            }
        }

        public Float OffsetX
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 0;
                }
                else
                {
                    return _offsetX;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(1, 0,
                              0, 1,
                              value, 0,
                              MatrixTypes.TRANSFORM_IS_TRANSLATION);
                }
                else
                {
                    _offsetX = value;
                    if (_type != MatrixTypes.TRANSFORM_IS_UNKNOWN)
                    {
                        _type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
                    }
                }
            }
        }

        public Float OffsetY
        {
            get
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    return 0;
                }
                else
                {
                    return _offsetY;
                }
            }
            set
            {
                if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
                {
                    SetMatrix(1, 0,
                              0, 1,
                              0, value,
                              MatrixTypes.TRANSFORM_IS_TRANSLATION);
                }
                else
                {
                    _offsetY = value;
                    if (_type != MatrixTypes.TRANSFORM_IS_UNKNOWN)
                    {
                        _type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
                    }
                }
            }
        }

        internal void MultiplyVector(ref Float x, ref Float y)
        {
            switch (_type)
            {
                case MatrixTypes.TRANSFORM_IS_IDENTITY:
                case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    return;
                case MatrixTypes.TRANSFORM_IS_SCALING:
                case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    x *= _m11;
                    y *= _m22;
                    break;
                default:
                    Float xadd = y * _m21;
                    Float yadd = x * _m12;
                    x *= _m11;
                    x += xadd;
                    y *= _m22;
                    y += yadd;
                    break;
            }
        }

        internal void MultiplyPoint(ref Float x, ref Float y)
        {
            switch (_type)
            {
                case MatrixTypes.TRANSFORM_IS_IDENTITY:
                    return;
                case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    x += _offsetX;
                    y += _offsetY;
                    return;
                case MatrixTypes.TRANSFORM_IS_SCALING:
                    x *= _m11;
                    y *= _m22;
                    return;
                case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    x *= _m11;
                    x += _offsetX;
                    y *= _m22;
                    y += _offsetY;
                    break;
                default:
                    Float xadd = y * _m21 + _offsetX;
                    Float yadd = x * _m12 + _offsetY;
                    x *= _m11;
                    x += xadd;
                    y *= _m22;
                    y += yadd;
                    break;
            }
        }

        internal static Matrix2f CreateRotationRadians(Float angle)
        {
            return CreateRotationRadians(angle, /* centerX = */ 0, /* centerY = */ 0);
        }

        internal static Matrix2f CreateRotationRadians(Float angle, Float centerX, Float centerY)
        {
            Matrix2f matrix = new Matrix2f();

            Float sin = (Float)Math.Sin(angle);
            Float cos = (Float)Math.Cos(angle);
            Float dx = (centerX * (1.0F - cos)) + (centerY * sin);
            Float dy = (centerY * (1.0F - cos)) - (centerX * sin);

            matrix.SetMatrix(cos, sin,
                              -sin, cos,
                              dx, dy,
                              MatrixTypes.TRANSFORM_IS_UNKNOWN);

            return matrix;
        }

        internal static Matrix2f CreateScaling(Float scaleX, Float scaleY, Float centerX, Float centerY)
        {
            Matrix2f matrix = new Matrix2f();

            matrix.SetMatrix(scaleX, 0,
                             0, scaleY,
                             centerX - scaleX * centerX, centerY - scaleY * centerY,
                             MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION);

            return matrix;
        }

        internal static Matrix2f CreateScaling(Float scaleX, Float scaleY)
        {
            Matrix2f matrix = new Matrix2f();
            matrix.SetMatrix(scaleX, 0,
                             0, scaleY,
                             0, 0,
                             MatrixTypes.TRANSFORM_IS_SCALING);
            return matrix;
        }

        internal static Matrix2f CreateSkewRadians(Float skewX, Float skewY)
        {
            Matrix2f matrix = new Matrix2f();

            matrix.SetMatrix(1.0F, (Float)Math.Tan(skewY),
                             (Float)Math.Tan(skewX), 1.0F,
                             0.0F, 0.0F,
                             MatrixTypes.TRANSFORM_IS_UNKNOWN);

            return matrix;
        }

        internal static Matrix2f CreateTranslation(Float offsetX, Float offsetY)
        {
            Matrix2f matrix = new Matrix2f();

            matrix.SetMatrix(1, 0,
                             0, 1,
                             offsetX, offsetY,
                             MatrixTypes.TRANSFORM_IS_TRANSLATION);

            return matrix;
        }

        private static Matrix2f CreateIdentity()
        {
            Matrix2f matrix = new Matrix2f();
            matrix.SetMatrix(1, 0,
                             0, 1,
                             0, 0,
                             MatrixTypes.TRANSFORM_IS_IDENTITY);
            return matrix;
        }

        private void SetMatrix(Float m11, Float m12,
                               Float m21, Float m22,
                               Float offsetX, Float offsetY,
                               MatrixTypes type)
        {
            this._m11 = m11;
            this._m12 = m12;
            this._m21 = m21;
            this._m22 = m22;
            this._offsetX = offsetX;
            this._offsetY = offsetY;
            this._type = type;
        }

        private void DeriveMatrixType()
        {
            _type = 0;

            // Now classify our matrix.
            if (!(_m21 == 0 && _m12 == 0))
            {
                _type = MatrixTypes.TRANSFORM_IS_UNKNOWN;
                return;
            }

            if (!(_m11 == 1 && _m22 == 1))
            {
                _type = MatrixTypes.TRANSFORM_IS_SCALING;
            }

            if (!(_offsetX == 0 && _offsetY == 0))
            {
                _type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
            }

            if (0 == (_type & (MatrixTypes.TRANSFORM_IS_TRANSLATION | MatrixTypes.TRANSFORM_IS_SCALING)))
            {
                // We have an identity matrix.
                _type = MatrixTypes.TRANSFORM_IS_IDENTITY;
            }
            return;
        }

        private void Debug_CheckType()
        {
            switch (_type)
            {
                case MatrixTypes.TRANSFORM_IS_IDENTITY:
                    return;
                case MatrixTypes.TRANSFORM_IS_UNKNOWN:
                    return;
                case MatrixTypes.TRANSFORM_IS_SCALING:
                    Debug.Assert(_m21 == 0);
                    Debug.Assert(_m12 == 0);
                    Debug.Assert(_offsetX == 0);
                    Debug.Assert(_offsetY == 0);
                    return;
                case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    Debug.Assert(_m21 == 0);
                    Debug.Assert(_m12 == 0);
                    Debug.Assert(_m11 == 1);
                    Debug.Assert(_m22 == 1);
                    return;
                case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    Debug.Assert(_m21 == 0);
                    Debug.Assert(_m12 == 0);
                    return;
                default:
                    Debug.Assert(false);
                    return;
            }
        }

        private bool IsDistinguishedIdentity
        {
            get
            {
                return _type == MatrixTypes.TRANSFORM_IS_IDENTITY;
            }
        }

        private const int c_identityHashCode = 0;

        internal Float _m11;
        internal Float _m12;
        internal Float _m21;
        internal Float _m22;
        internal Float _offsetX;
        internal Float _offsetY;
        internal MatrixTypes _type;

        public static bool operator ==(Matrix2f matrix1, Matrix2f matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
            {
                return matrix1.IsIdentity == matrix2.IsIdentity;
            }
            else
            {
                return matrix1.M11 == matrix2.M11 &&
                       matrix1.M12 == matrix2.M12 &&
                       matrix1.M21 == matrix2.M21 &&
                       matrix1.M22 == matrix2.M22 &&
                       matrix1.OffsetX == matrix2.OffsetX &&
                       matrix1.OffsetY == matrix2.OffsetY;
            }
        }

        public static bool operator !=(Matrix2f matrix1, Matrix2f matrix2)
        {
            return !(matrix1 == matrix2);
        }

        public static XElement GetData(Matrix2f m, string name)
        {
            var ele = new XElement(name);
            ele.Add(new XElement("M11", m.M11));
            ele.Add(new XElement("M12", m.M12));
            ele.Add(new XElement("M21", m.M21));
            ele.Add(new XElement("M22", m.M22));
            ele.Add(new XElement("OffsetX", m.OffsetX));
            ele.Add(new XElement("OffsetY", m.OffsetY));
            return ele;
        }

        public static Matrix2f LoadData(XElement ele)
        {
            var m = new Matrix2f();
            m.M11 = float.Parse(ele.Element("M11").Value);
            m.M12 = float.Parse(ele.Element("M12").Value);
            m.M21 = float.Parse(ele.Element("M21").Value);
            m.M22 = float.Parse(ele.Element("M22").Value);
            m.OffsetX = float.Parse(ele.Element("OffsetX").Value);
            m.OffsetY = float.Parse(ele.Element("OffsetY").Value);
            return m;
        }

        public static bool Equals(Matrix2f matrix1, Matrix2f matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
            {
                return matrix1.IsIdentity == matrix2.IsIdentity;
            }
            else
            {
                return matrix1.M11.Equals(matrix2.M11) &&
                       matrix1.M12.Equals(matrix2.M12) &&
                       matrix1.M21.Equals(matrix2.M21) &&
                       matrix1.M22.Equals(matrix2.M22) &&
                       matrix1.OffsetX.Equals(matrix2.OffsetX) &&
                       matrix1.OffsetY.Equals(matrix2.OffsetY);
            }
        }

        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Matrix2f))
            {
                return false;
            }

            Matrix2f value = (Matrix2f)o;
            return Matrix2f.Equals(this, value);
        }

        public bool Equals(Matrix2f value)
        {
            return Matrix2f.Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsDistinguishedIdentity)
            {
                return c_identityHashCode;
            }
            else
            {
                // Perform field-by-field XOR of HashCodes
                return M11.GetHashCode() ^
                       M12.GetHashCode() ^
                       M21.GetHashCode() ^
                       M22.GetHashCode() ^
                       OffsetX.GetHashCode() ^
                       OffsetY.GetHashCode();
            }
        }
    }
}