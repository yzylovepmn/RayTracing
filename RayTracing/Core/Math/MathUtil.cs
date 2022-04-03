using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Core
{
    public static class MathUtil
    {
        public const double DBL_EPSILON = 2.2204460492503131e-016;
        public const float SIN_EPSILON = 1.192092896e-07F;
        public const float ZeroTolerancef = 1e-06f;
        public const float PIF = (float)Math.PI;
        public const float PIOver2F = (float)Math.PI / 2;
        public const float PIOver4F = (float)Math.PI / 4;
        public const float TwoPIF = (float)Math.PI * 2;
        public const float InvPIF = (float)(1 / Math.PI);
        public const float Inv2PIF = (float)(0.5 / Math.PI);
        public const float Inv4PIF = (float)(0.25 / Math.PI);
        public const float ThreeOverFourPIF = (float)(0.75 / Math.PI);
        public const float OneThird = (float)(1.0 / 3);

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float)(radians * (180.0 / Math.PI));
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)(degrees * (Math.PI / 180.0));
        }

        public static float Clamp(float f, float low = 0, float high = 1)
        {
            return (f < low) ? low : (f > high) ? high : f;
        }

        public static double Clamp(double f, double low = 0, double high = 1)
        {
            return (f < low) ? low : (f > high) ? high : f;
        }

        public static Vector3f ProjectToTrackball(Point3f point, float w, float h)
        {
            double r = Math.Sqrt(w * w + h * h) / 2;
            double x = (point.X - w / 2) / r;
            double y = (h / 2 - point.Y) / r;
            double z2 = 1 - x * x - y * y;
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3f((float)x, (float)y, (float)z);
        }

        public static Vector3f CalcNormal(Point3f p1, Point3f p2, Point3f p3)
        {
            var normal = Vector3f.CrossProduct(p2 - p1, p3 - p2);
            normal.Normalize();
            return normal;
        }

        public static Vector3d CalcNormal(Point3d p1, Point3d p2, Point3d p3)
        {
            var normal = Vector3d.CrossProduct(p2 - p1, p3 - p2);
            normal.Normalize();
            return normal;
        }

        public static Vector3f CalcNormal(Vector3f v1, Vector3f v2, Vector3f v3)
        {
            var normal = Vector3f.CrossProduct(v2 - v1, v3 - v2);
            normal.Normalize();
            return normal;
        }

        public static Vector3d CalcNormal(Vector3d v1, Vector3d v2, Vector3d v3)
        {
            var normal = Vector3d.CrossProduct(v2 - v1, v3 - v2);
            normal.Normalize();
            return normal;
        }

        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 10.0 * DBL_EPSILON;
        }

        public static bool IsOne(double value)
        {
            return IsZero(value - 1);
        }

        public static bool AreClose(double value1, double value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2) return true;
            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DBL_EPSILON;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        public static bool IsZero(float value)
        {
            return Math.Abs(value) < 10.0 * SIN_EPSILON;
        }

        public static bool IsOne(float value)
        {
            return IsZero(value - 1);
        }

        public static bool AreClose(float value1, float value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2) return true;
            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < SIN_EPSILON
            float eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0f) * SIN_EPSILON;
            float delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        public static bool IsIntersect(Plane plane, AxisAlignedBox3f box)
        {
            return plane.GetSide(box) == PlaneSide.BothSide;
        }
    }
}