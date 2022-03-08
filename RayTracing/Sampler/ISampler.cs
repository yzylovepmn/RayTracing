using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public interface ISampler
    {
        Pipeline Pipeline { get; }

        Colorf Sample(int j, int i);
    }
}