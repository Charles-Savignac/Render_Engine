using Render_Engine.Acceleration;
using Render_Engine.Cameras;
using Render_Engine.Illumination;
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
            Tracer = new DirectIllumination(this);
            CameraType = new Pinhole();

            CreateShapes();
            CreateLightSources();

            AccelerationTec = new GridAccelerator(Tracer.Shapes);
        }

        private void CreateShapes()
        {
            Sphere sp1 = new Sphere(new Point3D(), Color.Red, 250);
            Sphere sp2 = new Sphere(new Point3D(), Color.Green, 250);


            sp1.AddTransformation(GT.Translate(-300, 0, -100));
            sp2.AddTransformation(GT.Translate(300, 100, 0));

            Tracer.AddShapes(sp1, sp2);
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
