using Render_Engine.Acceleration;
using Render_Engine.Illumination;
using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;
using System.Drawing.Imaging;

using GT = Render_Engine.Util.GeometricTransform;


namespace Render_Engine
{
    internal class World
    {
        public ViewPlan View_plan { get; private set; }
        public Color Background_color { get; private set; }
        public List<Shape> Shapes { get; private set; }
        public RayTracer Tracer { get; private set; }
        public Accelerator AccelerationTec { get; set; }

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan();
            Background_color = Color.Black;
            Shapes = new List<Shape>();
            Tracer = new RayTracer(this);
            CreateShapes();

            AccelerationTec = new GridAccelerator(Shapes);
        }

        private void CreateShapes()
        {
            Sphere sp1 = new Sphere(new Util.Point(), Color.Red, 100);
            Sphere sp2 = new Sphere(new Util.Point(), Color.Green, 100);
            Sphere sp3 = new Sphere(new Util.Point(), Color.Blue, 100);



            Plan lp1 = new Plan(new Util.Point(), Color.Red, 100, 100);
            Cylinder cy1 = new Cylinder(new Util.Point(), Color.Red, 100, -25, 25);
            Cube cu1 = new Cube(new Util.Point(), Color.Red, 100);
            Cone co1 = new Cone(new Util.Point(), Color.Red, 100, 200);
            Disk di1 = new Disk(new Util.Point(), Color.Red, 50, 100);
            Triangle tr = new Triangle(new Util.Point(), Color.Red, new Util.Point(-100, -50, 0), new Util.Point(100, -50, 0), new Util.Point(0, 100, 0));

            sp1.AddTransformation(GT.Translate(-100));
            sp3.AddTransformation(GT.Translate(100));


            cy1.AddTransformation(GT.RotateX(85));
            cu1.AddTransformation(GT.RotateX(45), GT.RotateY(45));
            di1.AddTransformation(GT.RotateX(45));

            //AddShapes(sp1, sp2, sp3);
            AddShapes(cu1);
        }

        private void AddShapes(params Shape[] s)
        {
            foreach (Shape shape in s)
            {
                Shapes.Add(shape);
                Console.WriteLine(shape);
            }
        }

        public Bitmap RenderScene()
        {
            float distanceCameraViewPlan = 100;

            Ray ray = new Ray();
            ray.T_min = 0;
            ray.T_max = 500;
            ray.Direction = new Vector3D(0, 0, -1);

            Bitmap bitmap = new Bitmap(View_plan.X_res, View_plan.Y_res, PixelFormat.Format16bppRgb555);


            for (int row = 0; row < View_plan.X_res; row++)
            {
                for (int colunm = 0; colunm < View_plan.Y_res; colunm++)
                {
                    float x = View_plan.Pixel_size * (row - 0.5f * (View_plan.X_res - 1));
                    float y = View_plan.Pixel_size * (colunm - 0.5f * (View_plan.Y_res - 1));

                    ray.Origin = new Util.Point(x, y, distanceCameraViewPlan);
                    Color c = Tracer.TraceRay(ray);
                    bitmap.SetPixel(row, colunm, c);
                }
            }

            return bitmap;
        }
    }
}
