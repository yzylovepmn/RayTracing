using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracing.Core
{
    public class PPMImage
    {
        public PPMImage()
        {
            _width = 256;
            _height = 256;
        }

        public int Width { get { return _width; } set { _width = value; } }
        private int _width;

        public int Height { get { return _height; } set { _height = value; } }
        private int _height;

        public void RenderTo(ISampler sampler, string fileName)
        {
            var tick = Environment.TickCount;

            var sb = new StringBuilder();

            sb.AppendLine("P3");
            sb.AppendLine(string.Format("{0} {1}", _width, _height));
            sb.AppendLine(string.Format("{0}", byte.MaxValue));

            for (int j = _height - 1; j >= 0; j--)
            {
                Console.WriteLine(string.Format("Line : {0} done", j));
                for (int i = 0; i < _width; i++)
                {
                    try
                    {
                        var color = sampler.Sample(j, i);

                        // gamma correction (gamma = 2)
                        color.R = (float)Math.Sqrt(color.R);
                        color.G = (float)Math.Sqrt(color.G);
                        color.B = (float)Math.Sqrt(color.B);
                        var cb = color.ToBytes();

                        sb.Append(string.Format("{0}{1} {2} {3}", i == 0 ? "" : " ", cb.R, cb.G, cb.B));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("Exception occur\r\nStacktrace : {0}\r\nMessage : {1}", e.StackTrace, e.Message));
                    }
                }
                if (j > 0)
                    sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());

            Console.WriteLine(string.Format("Total execution time : {0}s", (Environment.TickCount - tick) / 1000f));
        }

        public void RenderToParallel(ISampler sampler, string fileName)
        {
            var tick = Environment.TickCount;
            var count = 0;
            Colorf[,] colors = new Colorf[_height, _width];
            Parallel.For(0, _height, (k) =>
            {
                var j = _height - 1 - k;
                try
                {
                    Parallel.For(0, _width, (i) =>
                    {
                        var color = sampler.Sample(j, i);

                        // gamma correction
                        color.R = (float)Math.Sqrt(color.R);
                        color.G = (float)Math.Sqrt(color.G);
                        color.B = (float)Math.Sqrt(color.B);

                        colors[j, i] = color;
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Exception occur\r\nStacktrace : {0}\r\nMessage : {1}", e.StackTrace, e.Message));
                }
                Interlocked.Increment(ref count);
                Console.WriteLine(string.Format("Line : {0} done - Total done {1}", j, count));
            });

            var sb = new StringBuilder();
            sb.AppendLine("P3");
            sb.AppendLine(string.Format("{0} {1}", _width, _height));
            sb.AppendLine(string.Format("{0}", byte.MaxValue));

            for (int j = _height - 1; j >= 0; j--)
            {
                for (int i = 0; i < _width; i++)
                {
                    var cb = colors[j, i].ToBytes();

                    sb.Append(string.Format("{0}{1} {2} {3}", i == 0 ? "" : " ", cb.R, cb.G, cb.B));
                }
                if (j > 0)
                    sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());

            Console.WriteLine(string.Format("Total execution time : {0}s", (Environment.TickCount - tick) / 1000f));
        }
    }
}