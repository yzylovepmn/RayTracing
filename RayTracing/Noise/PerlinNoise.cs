using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class PerlinNoise : NoiseBase
    {
        private const int _DIMENSION = 256;

        public PerlinNoise()
        {
            _values = new Vector3f[_DIMENSION];
            for (int i = 0; i < _values.Length; i++)
                _values[i] = Utilities.RandomVectorOnUnitSphere();

            _x = _GenerateIndexPermute();
            _y = _GenerateIndexPermute();
            _z = _GenerateIndexPermute();
        }

        private int[] _x;
        private int[] _y;
        private int[] _z;
        private Vector3f[] _values;

        public override float Noise(Point3f p)
        {
            var xf = (float)Math.Floor(p.X);
            var yf = (float)Math.Floor(p.Y);
            var zf = (float)Math.Floor(p.Z);
            var u = p.X - xf;
            var v = p.Y - yf;
            var w = p.Z - zf;
            var i = (int)xf;
            var j = (int)yf;
            var k = (int)zf;
            var c = new Vector3f[2, 2, 2];
            for (int di = 0; di < 2; di++)
            {
                for (int dj = 0; dj < 2; dj++)
                {
                    for (int dk = 0; dk < 2; dk++)
                        c[di, dj, dk] = _values[_x[(i + di) & 255] ^ _y[(j + dj) & 255] ^ _z[(k + dk) & 255]];
                }
            }

            return _Trilinear(c, u, v, w);
        }

        private static int[] _GenerateIndexPermute()
        {
            var index = new int[_DIMENSION];
            for (int i = 0; i < index.Length; i++)
                index[i] = i;
            _Permute(index);
            return index;
        }

        private static void _Permute(int[] index)
        {
            for (int i = index.Length - 1; i > 0; i--)
            {
                var target = Utilities.RandomInt(0, i);
                Utilities.Swap(ref index[target], ref index[i]);
            }
        }

        private static float _Trilinear(Vector3f[, , ] c, float u, float v, float w)
        {
            var uu = Utilities.SmoothStep(u);
            var vv = Utilities.SmoothStep(v);
            var ww = Utilities.SmoothStep(w);
            var acc = 0f;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var weight = new Vector3f(u - i, v - j, w - k);
                        acc += (i * uu + (1 - i) * (1 - uu)) * (j * vv + (1 - j) * (1 - vv)) * (k * ww + (1 - k) * (1 - ww)) * Vector3f.DotProduct(c[i, j, k], weight);
                    }
                }
            }
            return acc;
        }
    }
}