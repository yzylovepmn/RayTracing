using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing.Core
{
    public struct OBNAxis
    {
        public OBNAxis(Vector3f zAxis)
        {
            zAxis.Normalize();
            ZAxis = zAxis;
            var up = Math.Abs(zAxis.X) > 0.9 ? Vector3f.YAxis : Vector3f.XAxis;
            YAxis = Vector3f.CrossProduct(zAxis, up);
            XAxis = Vector3f.CrossProduct(zAxis, YAxis);
        }

        public Vector3f GetWorldPosition(Vector3f local)
        {
            return local.X * XAxis + local.Y * YAxis + local.Z * ZAxis;
        }

        public Vector3f XAxis;
        public Vector3f YAxis;
        public Vector3f ZAxis;
    }
}