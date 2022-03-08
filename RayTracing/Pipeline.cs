using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Pipeline
    {
        public Pipeline()
        {
            _scene = new Scene();
            _image = new PPMImage();
            _maxRayDepth = 50;
        }

        public PPMImage Image { get { return _image; } }
        protected PPMImage _image;

        public Scene Scene { get { return _scene; } }
        protected Scene _scene;

        internal int MaxRayDepth { get { return _maxRayDepth; } }
        private int _maxRayDepth;

        public virtual void Init()
        {
            _image.Width = 400;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            // setup scene
            _scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 100, Center = new Point3f(0, -100.5f, -1) }, new LambertianMaterial(new Colorf(0.8f, 0.8f, 0))));
            _scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(0, 0, -1) }, new LambertianMaterial(new Colorf(0.1f, 0.2f, 0.5f))));
            _scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(-1, 0, -1) }, new DielectricMaterial(1.5f)));
            _scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = -0.4f, Center = new Point3f(-1, 0, -1) }, new DielectricMaterial(1.5f)));
            _scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(1, 0, -1) }, new MetalMaterial(new Colorf(0.8f, 0.6f, 0.2f))));
        }

        #region Render
        public void RenderTo(ISampler sampler, string fileName)
        {
            _image.RenderTo(sampler, fileName);
        }
        #endregion
    }
}