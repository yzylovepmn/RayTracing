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
            _scene.Camera.Aspect = 16 / 9f;
            _scene.Camera.Position = new Point3f(13, 2, 3);
            _scene.Camera.LookDirection = new Point3f(0, 0, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0.1f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.ShutterTime1 = 0;
            _scene.Camera.ShutterTime2 = 1;
            _scene.Camera.EnableShutter = true;
            _scene.Camera.Update();

            _image.Width = 400;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            // setup scene
            //_scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 100, Center = new Point3f(0, -100.5f, -1) }, new LambertianMaterial(new Colorf(0.8f, 0.8f, 0))));
            //_scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(0, 0, -1) }, new LambertianMaterial(new Colorf(0.1f, 0.2f, 0.5f))));
            //_scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(-1, 0, -1) }, new DielectricMaterial(1.5f)));
            //_scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = -0.45f, Center = new Point3f(-1, 0, -1) }, new DielectricMaterial(1.5f)));
            //_scene.HittableList.AddHittable(new MeshObject(new Sphere() { Radius = 0.5f, Center = new Point3f(1, 0, -1) }, new MetalMaterial(new Colorf(0.8f, 0.6f, 0.2f))));
            _SceneSecondWeek();
        }

        private void _SceneSecondWeek()
        {
            var objectList = _scene.HittableList;
            var groundMaterial = new LambertianMaterial(new Colorf(0.5f, 0.5f, 0.5f));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1000, Center = new Point3f(0, -1000, 0) }, groundMaterial));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var chooseMat = Utilities.RandomFloat();
                    var center = new Point3f(a + 0.9f * Utilities.RandomFloat(), 0.2f, b + 0.9f * Utilities.RandomFloat());
                    var rp = new Point3f(4, 0.2f, 0);
                    if ((center - rp).Length > 0.9)
                    {
                        Material mat = null;

                        if (chooseMat < 0.8f)
                        {
                            var albedo = Utilities.RandomColor() * Utilities.RandomColor();
                            mat = new LambertianMaterial(albedo);
                            objectList.AddHittable(new MeshObject(new MoveableSphere() { Radius = 0.2f, Position = center, Target = center + new Vector3f(0, Utilities.RandomFloat(0, 0.5f), 0), Time1 = 0, Time2 = 1 }, mat));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            var albedo = Utilities.RandomColor(0.5f, 1);
                            var fuzz = Utilities.RandomFloat(0, 0.5f);
                            mat = new MetalMaterial(albedo, fuzz);
                            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                        else
                        {
                            mat = new DielectricMaterial(1.5f);
                            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                    }
                }
            }

            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(0, 1, 0) }, new DielectricMaterial(1.5f)));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(-4, 1, 0) }, new LambertianMaterial(new Colorf(0.4f, 0.2f, 0.1f))));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(4, 1, 0) }, new MetalMaterial(new Colorf(0.7f, 0.6f, 0.5f))));
        }

        private void _SceneFirstWeekend()
        {
            _scene.Camera.Aspect = 3 / 2f;
            _scene.Camera.Position = new Point3f(13, 2, 3);
            _scene.Camera.LookDirection = new Point3f(0, 0, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0.1f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.Update();

            _image.Width = 1200;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var objectList = _scene.HittableList;
            var groundMaterial = new LambertianMaterial(new Colorf(0.5f, 0.5f, 0.5f));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1000, Center = new Point3f(0, -1000, 0) }, groundMaterial));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var chooseMat = Utilities.RandomFloat();
                    var center = new Point3f(a + 0.9f * Utilities.RandomFloat(), 0.2f, b + 0.9f * Utilities.RandomFloat());
                    var rp = new Point3f(4, 0.2f, 0);
                    if ((center - rp).Length > 0.9)
                    {
                        Material mat = null;

                        if (chooseMat < 0.8f)
                        {
                            var albedo = Utilities.RandomColor() * Utilities.RandomColor();
                            mat = new LambertianMaterial(albedo);
                            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            var albedo = Utilities.RandomColor(0.5f, 1);
                            var fuzz = Utilities.RandomFloat(0, 0.5f);
                            mat = new MetalMaterial(albedo, fuzz);
                            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                        else
                        {
                            mat = new DielectricMaterial(1.5f);
                            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                    }
                }
            }

            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(0, 1, 0) }, new DielectricMaterial(1.5f)));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(-4, 1, 0) }, new LambertianMaterial(new Colorf(0.4f, 0.2f, 0.1f))));
            objectList.AddHittable(new MeshObject(new Sphere() { Radius = 1, Center = new Point3f(4, 1, 0) }, new MetalMaterial(new Colorf(0.7f, 0.6f, 0.5f))));
        }

        #region Render
        public void RenderTo(ISampler sampler, string fileName)
        {
            _image.RenderToParallel(sampler, fileName);
        }
        #endregion
    }
}