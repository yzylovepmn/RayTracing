using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace RayTracing
{
    public class ImageTexture : Texture
    {
        public ImageTexture(string imageFile)
        {
            _LoadImage(imageFile);
            _PostLoad();
        }

        public ImageTexture(Stream image)
        {
            _LoadImage(image);
            _PostLoad();
        }

        public ImageTexture(Bitmap image)
        {
            _image = image;
            _PostLoad();
        }

        private Bitmap _image;
        private SpinLock _lock;

        public int Height { get { return _height; } }
        private int _height;

        public int Width { get { return _width; } }
        private int _width;

        private void _LoadImage(string imageFile)
        {
            try
            {
                _image = new Bitmap(imageFile);
            }
            catch (Exception)
            {
                _image = null;
            }
        }

        private void _LoadImage(Stream image)
        {
            try
            {
                _image = new Bitmap(image);
            }
            catch (Exception)
            {
                _image = null;
            }
        }

        private void _PostLoad()
        {
            if (_image != null)
            {
                _width = _image.Width;
                _height = _image.Height;
                _lock = new SpinLock();
            }
        }

        public override Colorf Sample(float u, float v, Point3f p)
        {
            if (_image == null)
                return Colorf.Red;

            u = MathUtil.Clamp(u);
            v = 1 - MathUtil.Clamp(v);
            var x = (int)(u * _width);
            var y = (int)(v * _height);
            if (x == _width)
                x--;
            if (y == _height)
                y--;

            var take = false;
            _lock.Enter(ref take);
            var color = Utilities.ToColorf(_image.GetPixel(x, y));
            _lock.Exit();

            return color;
        }
    }
}