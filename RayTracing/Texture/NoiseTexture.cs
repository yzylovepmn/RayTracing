using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class NoiseTexture : Texture
    {
        public NoiseTexture(INoise noise, float scale = 1)
        {
            _noise = noise;
            _scale = scale;
        }

        public INoise Noise { get { return _noise; } }
        private INoise _noise;

        public float Scale { get { return _scale; } }
        private float _scale;

        public override Colorf Sample(float u, float v, Point3f p)
        {
            var color = Colorf.White * 0.5f * (float)(1 + Math.Sin(_noise.Turb(p) * 10 + _scale * p.Z));
            color.A = 1;
            return color;
        }
    }
}