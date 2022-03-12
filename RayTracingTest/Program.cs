using RayTracing;
using System;
using System.IO;

namespace RayTracingTest
{
    class Program
    {
        static string TestFile;
        static Pipeline pipeline;

        static Program()
        {
            TestFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RayTracing.ppm");
        }

        static void Main(string[] args)
        {
            pipeline = new Pipeline();
            pipeline.Init();
            var sampler = new MultiSampler(pipeline);
            pipeline.RenderTo(sampler, TestFile);
        }
    }
}