using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public abstract class Texture
    {
        public abstract Colorf Sample(float u, float v, Point3f p);
    }
}