using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Core
{
    public static class Bitwise
    {
        public static uint FloatToFixed(float value, int bits)
        {
            if(value <= 0.0f) return 0;
            else if (value >= 1.0f) return (uint)(1 << bits)-1;
            else return (uint)(value * (1 << bits));
        }

        public static float FixedToFloat(uint value, int bits)
        {
            return value / (float)((1 << bits) - 1);
        }

        unsafe public static uint IntRead(IntPtr src, int n)
        {
            var p = src.ToPointer();
            switch (n)
            {
                case 1:
                    return ((byte*)p)[0];
                case 2:
                    return ((UInt16*)p)[0];
                case 3:
                    if (BitConverter.IsLittleEndian)
                        return (uint)((byte*)p)[0] | (uint)(((byte*)p)[1] << 8) | (uint)(((byte*)p)[2] << 16);
                    else
                        return (uint)(((byte*)p)[0] << 16) | (uint)(((byte*)p)[1] << 8) | (uint)((byte*)p)[2];
                case 4:
                    return ((UInt32*)p)[0];
            }
            return 0;
        }

        unsafe public static void IntWrite(IntPtr dest, int n, uint data)
        {
            var p = dest.ToPointer();
            switch (n)
            {
                case 1:
                    ((byte*)p)[0] = (byte)data;
                    break;
                case 2:
                    ((UInt16*)p)[0] = (UInt16)data;
                    break;
                case 3:
                    if (BitConverter.IsLittleEndian)
                    {
                        ((byte*)p)[0] = (byte)(data & 0xff);
                        ((byte*)p)[1] = (byte)((data >> 8) & 0xff);
                        ((byte*)p)[2] = (byte)((data >> 16) & 0xff);
                    }
                    else
                    {
                        ((byte*)p)[0] = (byte)((data >> 16) & 0xff);
                        ((byte*)p)[1] = (byte)((data >> 8) & 0xff);
                        ((byte*)p)[2] = (byte)(data & 0xff);
                    }
                    break;
                case 4:
                    ((UInt32*)p)[0] = data;
                    break;
            }
        }

        public static UInt16 FloatToHalf(float f)
        {
            FloatUInt32Union u = new FloatUInt32Union(f);
            return FloatToHalfI(u.UValue);
        }

        /// <summary>
        /// Converts float in uint32 format to a a half in uint16 format
        /// </summary>
        public static UInt16 FloatToHalfI(UInt32 i)
        {
            int s = (int)((i >> 16) & 0x00008000);
            int e = (int)(((i >> 23) & 0x000000ff) - (127 - 15));
            int m = (int)(i & 0x007fffff);

            if (e <= 0)
            {
                if (e < -10)
                {
                    return 0;
                }
                m = (m | 0x00800000) >> (1 - e);

                return (UInt16)(s | (m >> 13));
            }
            else if (e == 0xff - (127 - 15))
            {
                if (m == 0) // Inf
                {
                    return (UInt16)(s | 0x7c00);
                }
                else    // NAN
                {
                    m >>= 13;
                    return (UInt16)(s | 0x7c00 | m | (m == 0 ? 1 : 0));
                }
            }
            else
            {
                if (e > 30) // Overflow
                {
                    return (UInt16)(s | 0x7c00);
                }

                return (UInt16)(s | (e << 10) | (m >> 13));
            }
        }

        public static float HalfToFloat(UInt16 y)
        {
            FloatUInt32Union u = new FloatUInt32Union();
            u.UValue = HalfToFloatI(y);
            return u.FValue;
        }

        /// <summary>
        /// Converts a half in uint16 format to a float in uint32 format
        /// </summary>
        public static UInt32 HalfToFloatI(UInt16 y)
        {
            int s = (y >> 15) & 0x00000001;
            int e = (y >> 10) & 0x0000001f;
            int m = y & 0x000003ff;

            if (e == 0)
            {
                if (m == 0) // Plus or minus zero
                {
                    return (UInt32)(s << 31);
                }
                else // Denormalized number -- renormalize it
                {
                    while ((m & 0x00000400) == 0)
                    {
                        m <<= 1;
                        e -= 1;
                    }

                    e += 1;
                    m &= ~0x00000400;
                }
            }
            else if (e == 31)
            {
                if (m == 0) // Inf
                {
                    return (UInt32)((s << 31) | 0x7f800000);
                }
                else // NaN
                {
                    return (UInt32)((s << 31) | 0x7f800000 | (m << 13));
                }
            }

            e += (127 - 15);
            m <<= 13;

            return (UInt32)((s << 31) | (e << 23) | m);
        }
    }
}