using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Pipeline
    {
        public const string EarthPath = "RayTracing.Resources.Image.earthmap.jpg";

        public Pipeline()
        {
            _scene = new BVHScene();
            _image = new PPMImage();
            _maxRayDepth = 50;
            _background = Colorf.Black;
        }

        public PPMImage Image { get { return _image; } }
        protected PPMImage _image;

        public Scene Scene { get { return _scene; } }
        protected Scene _scene;

        internal int MaxRayDepth { get { return _maxRayDepth; } }
        private int _maxRayDepth;

        public Colorf Background { get { return _background; } set { _background = value; } }
        private Colorf _background;

        public virtual void Init()
        {
            // setup scene
            _SceneFirstWeekend();

            _scene.BuildScene();
        }

        private void _CornellSmoke()
        {
            _scene.Camera.Aspect = 1;
            _scene.Camera.Position = new Point3f(278, 278, -800);
            _scene.Camera.LookDirection = new Point3f(278, 278, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 40;
            _scene.Camera.Update();

            _image.Width = 600;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var red = new LambertianMaterial(new Colorf(0.65f, 0.05f, 0.05f));
            var white = new LambertianMaterial(new Colorf(0.73f, 0.73f, 0.73f));
            var green = new LambertianMaterial(new Colorf(0.12f, 0.45f, 0.15f));
            var light = new EmissiveMaterial(new Colorf(7f, 7f, 7f));

            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.YZ, new Vector2f(0, 0), new Vector2f(555, 555), 555), green));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.YZ, new Vector2f(0, 0), new Vector2f(555, 555), 0), red));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(127, 113), new Vector2f(432, 443), 554, true), light));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(0, 0), new Vector2f(555, 555), 0), white));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(0, 0), new Vector2f(555, 555), 555), white));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.XY, new Vector2f(0, 0), new Vector2f(555, 555), 555), white));
            var box1 = new Box3f(new Point3f(0, 0, 0), new Point3f(165, 330, 165));
            var box2 = new Box3f(new Point3f(0, 0, 0), new Point3f(165, 165, 165));
            var volume1 = new ConstantVolume(box1, 0.01f);
            var volume2 = new ConstantVolume(box2, 0.01f);
            _scene.AddHittable(new TransformedSceneObject(volume1, new IsotropicMaterial(Colorf.Black)) { Position = new Point3f(265, 0, 295), Orientation = new Quaternionf(Vector3f.YAxis, 15) });
            _scene.AddHittable(new TransformedSceneObject(volume2, new IsotropicMaterial(Colorf.White)) { Position = new Point3f(130, 0, 65), Orientation = new Quaternionf(Vector3f.YAxis, -18) });
        }

        private void _CornellBox()
        {
            _scene.Camera.Aspect = 1;
            _scene.Camera.Position = new Point3f(278, 278, -800);
            _scene.Camera.LookDirection = new Point3f(278, 278, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 40;
            _scene.Camera.Update();

            _image.Width = 600;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var red = new LambertianMaterial(new Colorf(0.65f, 0.05f, 0.05f));
            var white = new LambertianMaterial(new Colorf(0.73f, 0.73f, 0.73f));
            var green = new LambertianMaterial(new Colorf(0.12f, 0.45f, 0.15f));
            var light = new EmissiveMaterial(new Colorf(15f, 15f, 15f));

            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.YZ, new Vector2f(0, 0), new Vector2f(555, 555), 555), green));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.YZ, new Vector2f(0, 0), new Vector2f(555, 555), 0), red));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(227, 213), new Vector2f(332, 343), 554, true), light));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(0, 0), new Vector2f(555, 555), 0), white));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(0, 0), new Vector2f(555, 555), 555), white));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.XY, new Vector2f(0, 0), new Vector2f(555, 555), 555), white));
            _scene.AddHittable(new TransformedSceneObject(new Box3f(new Point3f(0, 0, 0), new Point3f(165, 330, 165)), white) { Position = new Point3f(265, 0, 295), Orientation = new Quaternionf(Vector3f.YAxis, 15) });
            _scene.AddHittable(new TransformedSceneObject(new Box3f(new Point3f(0, 0, 0), new Point3f(165, 165, 165)), white) { Position = new Point3f(130, 0, 65), Orientation = new Quaternionf(Vector3f.YAxis, -18) });
        }

        private void _SimpleLight()
        {
            _scene.Camera.Position = new Point3f(26, 3, 6);
            _scene.Camera.LookDirection = new Point3f(0, 2, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.Update();

            _image.Width = 600;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var texture = new NoiseTexture(new PerlinNoise(), 4);
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1000, Center = new Point3f(0, -1000f, 0) }, new LambertianMaterial(texture)));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 2, Center = new Point3f(0, 2, 0) }, new LambertianMaterial(texture)));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.XY, new Vector2f(3, 1), new Vector2f(5, 3), -2), new EmissiveMaterial(new Colorf(4f, 4f, 4f))));
        }

        private void _Earth()
        {
            _background = new Colorf(0.7f, 0.8f, 1);
            _scene.Camera.Position = new Point3f(13, 2, 3);
            _scene.Camera.LookDirection = new Point3f(0, 0, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.Update();

            _image.Width = 600;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var texture = new ImageTexture(Utilities.OpenStream(EarthPath));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 2, Center = new Point3f(0, 0, 0) }, new LambertianMaterial(texture)));
        }

        private void _TwoSpheres()
        {
            _background = new Colorf(0.7f, 0.8f, 1);
            _scene.Camera.Position = new Point3f(13, 2, 3);
            _scene.Camera.LookDirection = new Point3f(0, 0, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.Update();

            _image.Width = 600;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var texture = new NoiseTexture(new PerlinNoise(), 4);
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1000, Center = new Point3f(0, -1000f, 0) }, new LambertianMaterial(texture)));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 2, Center = new Point3f(0, 2, 0) }, new LambertianMaterial(texture)));
        }

        private void _SceneSecondWeek()
        {
            _scene.Camera.Aspect = 1;
            _scene.Camera.Position = new Point3f(478, 278, -600);
            _scene.Camera.LookDirection = new Point3f(278, 278, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0f;
            _scene.Camera.Fovy = 40;
            _scene.Camera.Update();
            _scene.Camera.ShutterTime1 = 0;
            _scene.Camera.ShutterTime2 = 1;
            _scene.Camera.EnableShutter = true;

            _image.Width = 800;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var ground = new LambertianMaterial(new Colorf(0.48f, 0.83f, 0.53f));
            var box1 = new List<IHittable>();
            var boxNumberPerSide = 20;
            for (int i = 0; i < boxNumberPerSide; i++)
            {
                for (int j = 0; j < boxNumberPerSide; j++)
                {
                    var w = 100;
                    var x0 = -1000 + i * w;
                    var z0 = -1000 + j * w;
                    var y0 = 0;
                    var x1 = x0 + w;
                    var y1 = Utilities.RandomFloat(1, 101);
                    var z1 = z0 + w;

                    box1.Add(new Box3f(new Point3f(x0, y0, z0), new Point3f(x1, y1, z1)));
                }
            }
            var node1 = new BVHNode();
            node1.Build(box1, 9);
            _scene.AddHittable(new SceneObject(node1, ground));

            var light = new EmissiveMaterial(new Colorf(7f, 7f, 7f));
            _scene.AddHittable(new SceneObject(new AxisAlignedRect(RectPlane.ZX, new Vector2f(147, 123), new Vector2f(412, 423), 554, true), light));

            var center1 = new Point3f(400, 400, 200);
            var center2 = center1 + new Vector3f(30, 0, 0);
            var movingSphereMaterial = new LambertianMaterial(new Colorf(0.7f, 0.3f, 0.1f));
            _scene.AddHittable(new SceneObject(new MoveableSphere(0, 1, center1, center2, 50), movingSphereMaterial));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 50, Center = new Point3f(260, 150, 45) }, new DielectricMaterial(1.5f)));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 50, Center = new Point3f(0, 150, 145) }, new MetalMaterial(new Colorf(0.8f, 0.8f, 0.9f), 1f)));

            var boundary = new SceneObject(new Sphere() { Radius = 70, Center = new Point3f(360, 150, 145) }, new DielectricMaterial(1.5f));
            _scene.AddHittable(boundary);
            _scene.AddHittable(new SceneObject(new ConstantVolume(boundary, 0.2f), new IsotropicMaterial(new Colorf(0.2f, 0.4f, 0.9f))));

            boundary = new SceneObject(new Sphere() { Radius = 5000, Center = new Point3f(0, 0, 0) }, new DielectricMaterial(1.5f));
            _scene.AddHittable(new SceneObject(new ConstantVolume(boundary, 0.0001f), new IsotropicMaterial(Colorf.White)));

            var earth = new ImageTexture(Utilities.OpenStream(EarthPath));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 100, Center = new Point3f(400, 200, 400) }, new LambertianMaterial(earth)));

            var noise = new NoiseTexture(new PerlinNoise(), 0.1f);
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 80, Center = new Point3f(220, 280, 300) }, new LambertianMaterial(noise)));

            var white = new LambertianMaterial(new Colorf(0.73f, 0.73f, 0.73f));
            var box2 = new List<IHittable>();
            var ns = 1000;
            for (int i = 0; i < ns; i++)
                box2.Add(new Sphere() { Radius = 10, Center = (Point3f)Utilities.RandomVector3(0, 165) });
            var node2 = new BVHNode();
            node2.Build(box2, 10);
            _scene.AddHittable(new TransformedSceneObject(node2, white) { Position = new Point3f(-100, 270, 395), Orientation = new Quaternionf(Vector3f.YAxis, 15) });
        }

        private void _SceneFirstWeekend()
        {
            _background = new Colorf(0.7f, 0.8f, 1);
            _scene.Camera.Aspect = 3 / 2f;
            _scene.Camera.Position = new Point3f(13, 2, 3);
            _scene.Camera.LookDirection = new Point3f(0, 0, 0) - _scene.Camera.Position;
            _scene.Camera.NearPlane = 10;
            _scene.Camera.Aperture = 0.1f;
            _scene.Camera.Fovy = 20;
            _scene.Camera.Update();

            _image.Width = 1200;
            _image.Height = (int)(_image.Width / _scene.Camera.Aspect);

            var groundMaterial = new LambertianMaterial(new Colorf(0.5f, 0.5f, 0.5f));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1000, Center = new Point3f(0, -1000, 0) }, groundMaterial));

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
                            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            var albedo = Utilities.RandomColor(0.5f, 1);
                            var fuzz = Utilities.RandomFloat(0, 0.5f);
                            mat = new MetalMaterial(albedo, fuzz);
                            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                        else
                        {
                            mat = new DielectricMaterial(1.5f);
                            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 0.2f, Center = center }, mat));
                        }
                    }
                }
            }

            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1, Center = new Point3f(0, 1, 0) }, new DielectricMaterial(1.5f)));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1, Center = new Point3f(-4, 1, 0) }, new LambertianMaterial(new Colorf(0.4f, 0.2f, 0.1f))));
            _scene.AddHittable(new SceneObject(new Sphere() { Radius = 1, Center = new Point3f(4, 1, 0) }, new MetalMaterial(new Colorf(0.7f, 0.6f, 0.5f))));
        }

        #region Render
        public void RenderTo(ISampler sampler, string fileName)
        {
            _image.RenderToParallel(sampler, fileName);
        }
        #endregion
    }
}