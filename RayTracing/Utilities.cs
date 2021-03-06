using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace RayTracing
{
    public static class Utilities
    {
        private static Random _rand;
        private static SpinLock _lock;

        static Utilities()
        {
            _rand = new Random(Environment.TickCount);
            _lock = new SpinLock();
        }

        public static bool IsZero(Vector3f v)
        {
            return MathUtil.IsZero(v.X) && MathUtil.IsZero(v.Y) && MathUtil.IsZero(v.Z);
        }

        public static bool IsZero(Vector3d v)
        {
            return MathUtil.IsZero(v.X) && MathUtil.IsZero(v.Y) && MathUtil.IsZero(v.Z);
        }

        public static void Swap<T>(ref T a, ref T b) where T : struct
        {
            var t = a;
            a = b;
            b = t;
        }

        public static int RandomInt(int min, int max)
        {
            var take = false;
            _lock.Enter(ref take);
            var value = _rand.Next(min, max);
            _lock.Exit();
            return value;
        }

        public static float RandomFloat()
        {
            var take = false;
            _lock.Enter(ref take);
            var value = _RandomFloat();
            _lock.Exit();
            return value;
        }

        private static float _RandomFloat()
        {
            return (float)_rand.NextDouble();
        }

        public static double RandomDouble()
        {
            var take = false;
            _lock.Enter(ref take);
            var value = _RandomDouble();
            _lock.Exit();
            return value;
        }

        private static double _RandomDouble()
        {
            return _rand.NextDouble();
        }

        private static double _RandomDouble(double min, double max)
        {
            var span = max - min;
            return min + span * _rand.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            var take = false;
            _lock.Enter(ref take);
            var delta = max - min;
            var value = _rand.NextDouble() * delta + min;
            _lock.Exit();
            return value;
        }

        public static float RandomFloat(float min, float max)
        {
            var take = false;
            _lock.Enter(ref take);
            var value = _RandomFloat(min, max);
            _lock.Exit();
            return value;
        }

        private static float _RandomFloat(float min, float max)
        {
            var delta = max - min;
            var value = (float)_rand.NextDouble() * delta + min;
            return value;
        }

        public static Vector3f RandomVector3()
        {
            var take = false;
            _lock.Enter(ref take);
            var value = new Vector3f((float)_RandomDouble(), (float)_RandomDouble(), (float)_RandomDouble());
            _lock.Exit();
            return value;
        }

        public static Vector3f RandomVector3(float min, float max)
        {
            var take = false;
            _lock.Enter(ref take);
            var value = new Vector3f((float)_RandomDouble(min, max), (float)_RandomDouble(min, max), (float)_RandomDouble(min, max));
            _lock.Exit();
            return value;
        }

        public static Vector2f RandomVector2()
        {
            var take = false;
            _lock.Enter(ref take);
            var value = new Vector2f((float)_RandomDouble(), (float)_RandomDouble());
            _lock.Exit();
            return value;
        }

        public static Colorf RandomColor()
        {
            var take = false;
            _lock.Enter(ref take);
            var value = new Colorf(_RandomFloat(), _RandomFloat(), _RandomFloat());
            _lock.Exit();
            return value;
        }

        public static Colorf RandomColor(float min, float max)
        {
            var take = false;
            _lock.Enter(ref take);
            var value = new Colorf(_RandomFloat(min, max), _RandomFloat(min, max), _RandomFloat(min, max));
            _lock.Exit();
            return value;
        }

        public static Vector3f UniformSampleInSphere()
        {
            var take = false;
            _lock.Enter(ref take);
            var u1 = _RandomDouble();
            var u2 = _RandomDouble();
            var u3 = _RandomDouble();
            _lock.Exit();
            var ratio = Math.Pow(u3, MathUtil.OneThird);
            var z = 1 - 2 * u1;
            var r = Math.Sqrt(1 - z * z);
            var theta = MathUtil.TwoPIF * u2;
            return new Vector3f((float)(r * Math.Cos(theta) * ratio), (float)(r * Math.Sin(theta) * ratio), (float)(z * ratio));
        }

        public static Vector3f UniformSampleOnSphere()
        {
            var take = false;
            _lock.Enter(ref take);
            var u1 = _RandomDouble();
            var u2 = _RandomDouble();
            _lock.Exit();
            var z = 1 - 2 * u1;
            var r = Math.Sqrt(1 - z * z);
            var theta = MathUtil.TwoPIF * u2;
            return new Vector3f((float)(r * Math.Cos(theta)), (float)(r * Math.Sin(theta)), (float)z);
        }

        public static float UniformOnSpherePdf()
        {
            return MathUtil.Inv4PIF;
        }

        public static float UniformInSpherePdf()
        {
            return MathUtil.ThreeOverFourPIF;
        }

        public static Vector3f UniformSampleOnHemisphere(ref Vector3f normal)
        {
            var take = false;
            _lock.Enter(ref take);
            var u1 = _RandomDouble();
            var u2 = _RandomDouble();
            _lock.Exit();
            var z = u1;
            var r = Math.Sqrt(1 - z * z);
            var theta = MathUtil.TwoPIF * u2;
            var ret = new Vector3f((float)(r * Math.Cos(theta)), (float)(r * Math.Sin(theta)), (float)z);
            var q = MatrixUtil.RotationBetween(Vector3f.ZAxis, normal);
            ret *= q;
            //var obn = new OBNAxis(normal);
            //ret = obn.GetWorldPosition(ret);
            return ret;
        }

        public static Vector3f CosineSampleOnHemisphere(ref Vector3f normal)
        {
            var d = ConcentricSampleInDisk();
            var z = (float)Math.Sqrt(1 - d.X * d.X - d.Y * d.Y);
            var ret = new Vector3f(d.X, d.Y, z);
            var q = MatrixUtil.RotationBetween(Vector3f.ZAxis, normal);
            ret *= q;
            //var obn = new OBNAxis(normal);
            //ret = obn.GetWorldPosition(ret);
            return ret;
        }

        public static float UniformOnHemispherePdf()
        {
            return MathUtil.Inv2PIF;
        }

        public static float CosineOnHemispherePdf(float cosTheta)
        {
            return cosTheta * MathUtil.InvPIF;
        }

        public static Vector3f UniformSampleInDisk()
        {
            var take = false;
            _lock.Enter(ref take);
            var u1 = _RandomDouble();
            var u2 = _RandomDouble();
            _lock.Exit();
            var r = Math.Sqrt(u1);
            var theta = MathUtil.TwoPIF * u2;
            return new Vector3f((float)(r * Math.Cos(theta)), (float)(r * Math.Sin(theta)), 0);
        }

        public static Vector3f ConcentricSampleInDisk()
        {
            var take = false;
            _lock.Enter(ref take);
            var u1 = _RandomDouble(-1, 1);
            var u2 = _RandomDouble(-1, 1);
            _lock.Exit();
            if (MathUtil.IsZero(u1) && MathUtil.IsZero(u1))
                return new Vector3f();
            float r, theta;
            if (Math.Abs(u1) > Math.Abs(u2))
            {
                r = (float)u1;
                theta = (float)(MathUtil.PIOver4F * u2 / u1);
            }
            else
            {
                r = (float)u2;
                theta = (float)(MathUtil.PIOver2F - MathUtil.PIOver4F * u1 / u2);
            }
            return new Vector3f((float)(r * Math.Cos(theta)), (float)(r * Math.Sin(theta)), 0);
        }

        public static float UniformInDiskPdf()
        {
            return MathUtil.InvPIF;
        }

        public static Vector3f Reflect(Vector3f vecIn, Vector3f normal)
        {
            return vecIn - 2 * Vector3f.DotProduct(vecIn, normal) * normal;
        }

        /// <summary>
        /// // Use Schlick's approximation for reflectance.
        /// </summary>
        public static float Reflectance(float c, float refractRadio)
        {
            var r = (1 - refractRadio) / (1 + refractRadio);
            r *= r;
            var x = 1 - c;
            return r + (1 - r) * x * x * x * x * x;
        }

        /// <summary>
        /// Assume all vectors is normalized
        /// </summary>
        public static Vector3f Refract(Vector3f vecIn, Vector3f normal, float c, float sSquared, float refractRadio)
        {
            var coeff1 = refractRadio;
            var coeff2 = (float)(refractRadio * c - Math.Sqrt(1 - refractRadio * refractRadio * sSquared));
            return vecIn * coeff1 + normal * coeff2;
        }

        /// <summary>
        /// Assume all vectors is normalized
        /// </summary>
        public static Vector3f Refract(Vector3f vecIn, Vector3f normal, float refractRadio)
        {
            var c = Math.Min(Vector3f.DotProduct(-vecIn, normal), 1);
            var coeff1 = refractRadio;
            var coeff2 = (float)(refractRadio * c - Math.Sqrt(1 - refractRadio * refractRadio * (1 - c * c)));
            return vecIn * coeff1 + normal * coeff2;
        }

        /// <param name="p">p on sphere</param>
        public static void ComputeUVOfSphere(Point3f p, out float u, out float v)
        {
            var theta = Math.Acos(-p.Y);
            var phi = Math.Atan2(-p.Z, p.X) + Math.PI;
            u = (float)(phi / (Math.PI * 2));
            v = (float)(theta / Math.PI);
        }

        public static float SmoothStep(float t)
        {
            return t * t * (3 - 2 * t);
        }

        public static Colorf ToColorf(Color color)
        {
            return new Colorf(color.R, color.G, color.B);
        }

        public static Stream OpenStream(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
                throw new FileNotFoundException("The resource file '" + name + "' was not found!");
            return stream;
        }
    }
}