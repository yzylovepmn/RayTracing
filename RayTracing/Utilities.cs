using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public static class Utilities
    {
        private static Random _rand;

        static Utilities()
        {
            _rand = new Random(Environment.TickCount);
        }

        public static bool IsZero(Vector3f v)
        {
            return MathUtil.IsZero(v.X) && MathUtil.IsZero(v.Y) && MathUtil.IsZero(v.Z);
        }

        public static bool IsZero(Vector3d v)
        {
            return MathUtil.IsZero(v.X) && MathUtil.IsZero(v.Y) && MathUtil.IsZero(v.Z);
        }

        public static double RandomDouble()
        {
            return _rand.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            var delta = max - min;
            return _rand.NextDouble() * delta + min;
        }

        public static Vector3f RandomVector()
        {
            return new Vector3f((float)RandomDouble(), (float)RandomDouble(), (float)RandomDouble());
        }

        public static Vector3f RandomVector(double min, double max)
        {
            return new Vector3f((float)RandomDouble(min, max), (float)RandomDouble(min, max), (float)RandomDouble(min, max));
        }

        public static Vector3f RandomVectorOnUnitSphere()
        {
            var vec = RandomVectorInUnitSphere();
            vec.Normalize();
            return vec;
        }

        public static Vector3f RandomVectorInHemiSphere(Vector3f normal)
        {
            var vec = RandomVectorInUnitSphere();
            if (Vector3f.DotProduct(normal, vec) < 0)
                vec.Negate();
            return vec;
        }

        public static Vector3f RandomVectorInUnitSphere()
        {
            while (true)
            {
                var v = RandomVector(-1, 1);
                if (v.Length < 1)
                    return v;
            }
        }

        public static Vector3f Reflect(Vector3f vecIn, Vector3f normal)
        {
            return vecIn - 2 * Vector3f.DotProduct(vecIn, normal) * normal;
        }
    }
}