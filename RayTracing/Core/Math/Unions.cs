using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.Core
{
    [StructLayout(LayoutKind.Explicit)]
    public struct FloatUInt32Union
    {
        public FloatUInt32Union(float f)
        {
            UValue = 0;
            FValue = f;
        }

        public FloatUInt32Union(UInt32 u)
        {
            FValue = 0;
            UValue = u;
        }

        [FieldOffset(0)]
        public float FValue;

        [FieldOffset(0)]
        public UInt32 UValue;
    }
}