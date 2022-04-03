using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Float = System.Single;

namespace RayTracing.Core
{
    public class MatrixUtil
    {
        /// <summary>
        /// keep sure the matrix is orthogonal
        /// </summary>
        public static void ApplyMatrix(Matrix3f mat, ref Point3f position, ref Vector3f scale, ref Quaternionf orientation)
        {
            scale = ExtractScale(mat);
            mat.M11 /= scale.X;
            mat.M12 /= scale.X;
            mat.M13 /= scale.X;
            mat.M21 /= scale.Y;
            mat.M22 /= scale.Y;
            mat.M23 /= scale.Y;
            mat.M31 /= scale.Z;
            mat.M32 /= scale.Z;
            mat.M33 /= scale.Z;
            orientation = ToQuaternion(mat, false);
            position = new Point3f(mat.OffsetX, mat.OffsetX, mat.OffsetZ);
        }

        /// <summary>
        /// keep sure the matrix is orthogonal
        /// </summary>
        public static Vector3f ExtractScale(Matrix3f mat)
        {
            var scaleX = (float)Math.Sqrt(mat.M11 * mat.M11 + mat.M12 * mat.M12 + mat.M13 * mat.M13);
            var scaleY = (float)Math.Sqrt(mat.M21 * mat.M21 + mat.M22 * mat.M22 + mat.M23 * mat.M23);
            var scaleZ = (float)Math.Sqrt(mat.M31 * mat.M31 + mat.M32 * mat.M32 + mat.M33 * mat.M33);
            return new Vector3f(scaleX, scaleY, scaleZ);
        }

        public static Matrix3f CreatePerspectiveMatrix(Float left, Float right, Float top, Float bottom, Float nearDist, Float farDist, bool isLeftHanded = true)
        {
            if (isLeftHanded)
            {
                nearDist = -nearDist;
                farDist = -farDist;
            }

            var wi = 1 / (right - left);
            var hi = 1 / (bottom - top);
            var di = 1 / (farDist - nearDist);

            var mat = Matrix3f.Identity;

            mat.M11 = 2 * nearDist * wi;
            mat.M22 = 2 * nearDist * hi;
            mat.M33 = Float.IsInfinity(farDist) ? -1 + 1e-5f : -(nearDist + farDist) * di;
            mat.OffsetZ = Float.IsInfinity(farDist) ? (-2 + 1e-5f) * nearDist : -2 * nearDist * farDist * di;
            mat.M31 = (left + right) * wi;
            mat.M32 = (top + bottom) * hi;
            mat.M34 = -1;
            mat.M44 = 0;

            return mat;
        }

        public static Matrix3f CreateOrthogonalMatrix(Float left, Float right, Float top, Float bottom, Float nearDist, Float farDist, bool isLeftHanded = true)
        {
            if (isLeftHanded)
            {
                nearDist = -nearDist;
                farDist = -farDist;
            }

            var wi = 1 / (right - left);
            var hi = 1 / (bottom - top);
            var di = 1 / (farDist - nearDist);

            var mat = Matrix3f.Identity;

            mat.M11 = 2 * wi;
            mat.M22 = 2 * hi;
            mat.M33 = Float.IsInfinity(farDist) ? -1e-5f / nearDist : - 2 * di;
            mat.OffsetX = -(left + right) * wi;
            mat.OffsetY = -(top + bottom) * hi;
            mat.OffsetZ = Float.IsInfinity(farDist) ? -1 + 1e-5f : -(nearDist + farDist) * di;

            return mat;
        }

        public static Matrix3f CreateViewMatrix(Point3f position, Quaternionf orientation)
        {
            var rotMat = ToMatrix(orientation);
            rotMat.Transpose();
            var offset = -(Vector3f)position * rotMat;
            rotMat.OffsetX = offset.X;
            rotMat.OffsetY = offset.Y;
            rotMat.OffsetZ = offset.Z;
            return rotMat;
        }

        public static Matrix3f CreateInverseViewMatrix(Point3f position, Quaternionf orientation)
        {
            return CreateMatrix(position, Vector3f.Unit, orientation);
        }

        public static Matrix3f InverseMatrix(Matrix3f mat, bool isOrthogonal = true)
        {
            if (isOrthogonal)
            {
                var vec = new Vector3f(mat.OffsetX, mat.OffsetY, mat.OffsetZ);
                mat.Transpose();
                var offset = -vec * mat;
                mat.OffsetX = offset.X;
                mat.OffsetY = offset.Y;
                mat.OffsetZ = offset.Z;
                return mat;
            }
            else return mat.Inverse;
        }

        public static Matrix3f CreateViewMatrix(Vector3f lookDirection, Vector3f upDirection, Point3f position, bool isLeftHanded = true)
        {
            var zAxis = isLeftHanded ? lookDirection : - lookDirection;
            zAxis.Normalize();
            var xAxis = Vector3f.CrossProduct(upDirection, zAxis);
            xAxis.Normalize();
            var yAxis = Vector3f.CrossProduct(zAxis, xAxis);

            var positionVec = (Vector3f)position;
            var cx = -Vector3f.DotProduct(xAxis, positionVec);
            var cy = -Vector3f.DotProduct(yAxis, positionVec);
            var cz = -Vector3f.DotProduct(zAxis, positionVec);

            var viewMatrix = new Matrix3f(
                xAxis.X, yAxis.X, zAxis.X, 0,
                xAxis.Y, yAxis.Y, zAxis.Y, 0,
                xAxis.Z, yAxis.Z, zAxis.Z, 0,
                cx, cy, cz, 1);

            return viewMatrix;
        }

        public static Matrix3f CreateInverseViewMatrix(Vector3f lookDirection, Vector3f upDirection, Point3f position, bool isLeftHanded = true)
        {
            var zAxis = isLeftHanded ? lookDirection : -lookDirection;
            zAxis.Normalize();
            var xAxis = Vector3f.CrossProduct(upDirection, zAxis);
            xAxis.Normalize();
            var yAxis = Vector3f.CrossProduct(zAxis, xAxis);

            var viewMatrix = new Matrix3f(
                xAxis.X, xAxis.Y, xAxis.Z, 0,
                yAxis.X, yAxis.Y, yAxis.Z, 0,
                zAxis.X, zAxis.Y, zAxis.Z, 0,
                position.X, position.Y, position.Z, 1);

            return viewMatrix;
        }

        public static Matrix3f CreateMatrix(Point3f position, Vector3f scale, Quaternionf orientation)
        {
            // Ordering:
            //    1. Scale
            //    2. Rotate
            //    3. Translate
            var mat = ToMatrix(orientation);

            mat.M11 *= scale.X;
            mat.M12 *= scale.X;
            mat.M13 *= scale.X;
            mat.M21 *= scale.Y;
            mat.M22 *= scale.Y;
            mat.M23 *= scale.Y;
            mat.M31 *= scale.Z;
            mat.M32 *= scale.Z;
            mat.M33 *= scale.Z;
            mat.OffsetX = position.X;
            mat.OffsetY = position.Y;
            mat.OffsetZ = position.Z;
            return mat;
        }

        public static Matrix3f CreateInverseMatrix(Point3f position, Vector3f scale, Quaternionf orientation)
        {
            // Invert the parameters
            Point3f invTranslate = -position;
            Vector3f invScale = new Vector3f(1 / scale.X, 1 / scale.Y, 1 / scale.Z);
            Quaternionf invRot = orientation.Inverse;

            // Because we're inverting, order is translation, rotation, scale
            // So make translation relative to scale & rotation
            invTranslate *= invRot; // rotate
            invTranslate = (Point3f)((Vector3f)invTranslate * invScale); // scale

            return CreateMatrix(invTranslate, invScale, invRot);
        }

        public static Matrix2f CreateTranslateMatrix(float offsetX, float offsetY)
        {
            var mat = Matrix2f.Identity;
            mat.Translate(offsetX, offsetY);
            return mat;
        }

        public static Matrix2f CreateScaleMatrix(Vector2f scale, Point2f center)
        {
            var mat = Matrix2f.Identity;
            mat.ScaleAt(scale.X, scale.Y, center.X, center.Y);
            return mat;
        }

        public static Matrix2f CreateRotateMatrix(float angle, Point2f center)
        {
            var mat = Matrix2f.Identity;
            mat.RotateAt(angle, center.X, center.Y);
            return mat;
        }

        public static Matrix3f ToMatrix(Quaternionf quaternion)
        {
            Point3f center = new Point3f();
            return Matrix3f.CreateRotationMatrix(ref quaternion, ref center);
        }

        public static Quaternionf RotationBetween(Vector3f v1, Vector3f v2)
        {
            // From Sam Hocevar's article "Quaternion from two vectors:
            var a = (Float)Math.Sqrt(v1.LengthSquared * v2.LengthSquared);
            var b = a + Vector3f.DotProduct(v1, v2);

            if (MathUtil.IsZero(2 * a - b) || a == 0)
                return Quaternionf.Identity;

            Vector3f axis;
            if (b < (Float)1e-06 * a)
            {
                b = (Float)0.0;
                axis = Math.Abs(v1.X) > Math.Abs(v1.Z) ? new Vector3f(-v1.Y, v1.X, (Float)0.0)
                     : new Vector3f((Float)0.0, -v1.Z, v1.Y);
            }
            else
            {
                axis = Vector3f.CrossProduct(v1, v2);
            }

            Quaternionf q = new Quaternionf(axis.X, axis.Y, axis.Z, b);
            q.Normalize();
            return q;
        }

        /// <summary>
        /// keep sure the matrix is orthogonal
        /// </summary>
        public static Quaternionf ToQuaternion(Matrix3f mat, bool hasScale = true)
        {
            if (hasScale)
            {
                var scale = ExtractScale(mat);
                mat.M11 /= scale.X;
                mat.M12 /= scale.X;
                mat.M13 /= scale.X;
                mat.M21 /= scale.Y;
                mat.M22 /= scale.Y;
                mat.M23 /= scale.Y;
                mat.M31 /= scale.Z;
                mat.M32 /= scale.Z;
                mat.M33 /= scale.Z;
            }

            double w = 0, x = 0, y = 0, z = 0;
            int index = 0;
            double wv = mat.M11 + mat.M22 + mat.M33;
            double xv = mat.M11 - mat.M22 - mat.M33;
            double yv = mat.M22 - mat.M11 - mat.M33;
            double zv = mat.M33 - mat.M11 - mat.M22;
            double v = wv;
            if (xv > v)
            {
                v = xv;
                index = 1;
            }
            if (yv > v)
            {
                v = yv;
                index = 2;
            }
            if (zv > v)
            {
                v = zv;
                index = 3;
            }
            double bv = Math.Sqrt(v + 1) * 0.5;
            double mult = 0.25 / bv;

            switch (index)
            {
                case 0:
                    w = bv;
                    x = (mat.M23 - mat.M32) * mult;
                    y = (mat.M31 - mat.M13) * mult;
                    z = (mat.M12 - mat.M21) * mult;
                    break;
                case 1:
                    x = bv;
                    w = (mat.M23 - mat.M32) * mult;
                    y = (mat.M12 + mat.M21) * mult;
                    z = (mat.M31 + mat.M13) * mult;
                    break;
                case 2:
                    y = bv;
                    w = (mat.M31 - mat.M13) * mult;
                    x = (mat.M12 + mat.M21) * mult;
                    z = (mat.M23 + mat.M32) * mult;
                    break;
                case 3:
                    z = bv;
                    w = (mat.M12 - mat.M21) * mult;
                    x = (mat.M31 + mat.M13) * mult;
                    y = (mat.M23 + mat.M32) * mult;
                    break;
            }
            return new Quaternionf((float)x, (float)y, (float)z, (float)w);
        }
    }
}