using Render_Engine.Acceleration;
using Render_Engine.Cameras;
using Render_Engine.Illumination;
using Render_Engine.Materials;
using Render_Engine.Sampling;
using Render_Engine.Shapes;
using Render_Engine.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;

using GT = Render_Engine.Util.GeometricTransform;


namespace Render_Engine
{
    internal class World
    {
        public ViewPlan View_plan { get; private set; }
        public Color Background_color { get; private set; }
        public RayTracer Tracer { get; private set; }
        public Accelerator AccelerationTec { get; set; }
        public Camera CameraType { get; set; }

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan(1080, 720, new UniformSampler(1));
            Background_color = Color.Black;
            Tracer = new WhittedRayTracer(this);
            CameraType = new Pinhole();

            CreateShapes();
            CreateLightSources();

            AccelerationTec = new GridAccelerator(Tracer.Shapes);
        }

        private void CreateShapes()
        {
            Phong phong = new Phong(Color.Red);
            Matte matte = new Matte(Color.Red);

            Reflective refMatRed = new Reflective(Color.Red);
            Reflective refMatBlue = new Reflective(Color.Blue);
            Reflective refMatYellow = new Reflective(Color.Yellow);
            Reflective refMatBluOrange = new Reflective(Color.Orange);

            Mirror mirrorMat = new Mirror();

            Sphere sp1 = new Sphere(new Point3D(), refMatRed, 250);
            Sphere sp2 = new Sphere(new Point3D(), refMatBlue, 250);
            Sphere sp3 = new Sphere(new Point3D(), refMatYellow, 250);
            Sphere sp4 = new Sphere(new Point3D(), refMatBluOrange, 250);

            sp1.AddTransformation(GT.Translate(400));
            sp2.AddTransformation(GT.Translate(-400));
            sp3.AddTransformation(GT.Translate(0, -400));
            sp4.AddTransformation(GT.Translate(0, 400));


            Tracer.AddShapes(sp1, sp2, sp3, sp4);
        }

        public void CreateLightSources()
        {
            PointLight l1 = new PointLight(new Point3D(0, 0, 300), Color.White);

            Tracer.AddLightSource(l1);
        }

        public Bitmap RenderScene()
        {
            Bitmap bitmap = new Bitmap(View_plan.X_res, View_plan.Y_res);
            bitmap = CameraType.RenderScene(this, bitmap);

            return bitmap;
        }
    }
}
