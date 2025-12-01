using Render_Engine.Acceleration;
using Render_Engine.Cameras;
using Render_Engine.Illumination;
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

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan(1080, 720);
            Background_color = Color.Black;
            Tracer = new DirectIllumination(this);
            CreateShapes();
            CreateLightSources();

            AccelerationTec = new GridAccelerator(Tracer.Shapes);
        }

        private void CreateShapes()
        {
            Sphere sp1 = new Sphere(new Point3D(), Color.Red, 100);
            Sphere sp2 = new Sphere(new Point3D(), Color.Green, 100);
            Sphere sp3 = new Sphere(new Point3D(), Color.Blue, 100);



            Plan lp1 = new Plan(new Point3D(), Color.Red, 100, 100);
            Cylinder cy1 = new Cylinder(new Point3D(), Color.Red, 100, -25, 25);
            Cube cu1 = new Cube(new Point3D(), Color.Red, 100);
            Cone co1 = new Cone(new Point3D(), Color.Red, 100, 200);
            Disk di1 = new Disk(new Point3D(), Color.White, 100, 200);
            Triangle tr = new Triangle(new Point3D(), Color.Red, new Point3D(-100, -50, 0), new Point3D(100, -50, 0), new Point3D(0, 100, 0));


            di1.AddTransformation(GT.RotateX(20), GT.RotateY(45));

            Tracer.AddShapes(sp1, di1);
        }

        public void CreateLightSources()
        {
            PointLight l1 = new PointLight(new Point3D(0, 0, 300), Color.White);


            Tracer.AddLightSource(l1);
        }


        public Bitmap RenderScene()
        {
            float distanceCameraViewPlan = 100;

            Ray ray = new Ray();
            ray.T_min = 0;
            ray.T_max = 500;
            ray.Direction = new Vector3D(0, 0, -1);

            Bitmap bitmap = new Bitmap(View_plan.X_res, View_plan.Y_res, PixelFormat.Format16bppRgb555);

            float halfW = 0.5f * (View_plan.X_res - 1);
            float halfH = 0.5f * (View_plan.Y_res - 1);

            for (int y = 0; y < View_plan.Y_res; y++)
            {
                for (int x = 0; x < View_plan.X_res; x++)
                {
                    float px = View_plan.Pixel_size * (x - halfW);
                    float py = View_plan.Pixel_size * (halfH - y);

                    ray.Origin = new Point3D(px, py, distanceCameraViewPlan);
                    Color c = Tracer.TraceRay(ray);
                    bitmap.SetPixel(x, y, c);
                }
            }

            return bitmap;
        }
    }
}
