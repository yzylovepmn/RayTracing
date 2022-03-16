using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public interface INoise
    {
        float Noise(Point3f p);

        float Turb(Point3f p, int depth = 7);
    }
}