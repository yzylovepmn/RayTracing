using RayTracing.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracing
{
    public class Scene
    {
        public Scene()
        {
            _camera = new Camera();
            _hittableList = new HittableList();
        }

        public Camera Camera { get { return _camera; } }
        protected Camera _camera;

        public HittableList HittableList { get { return _hittableList; } }
        private HittableList _hittableList;
    }
}