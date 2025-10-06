using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace Render_Engine
{
    internal class World
    {
        public ViewPlan View_plan { get; private set; }
        public Color Background_color { get; private set; }
        public List<Shape> Shapes { get; set; }

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan();
            Background_color = Color.Black;
            Shapes = new List<Shape>();
            AddShapes();
        }

        public void AddShapes()
        {
            Sphere s1 = new Sphere(this, new Util.Point(0, 0, 0), Color.Red, 100);
            s1.AddTransformation(GeometricTransform.Translate(100, 0, 0));

            Shapes.Add(s1);
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

                    foreach (Shape shape in Shapes)
                    {
                        ray.Origin = new Util.Point(x, y, distanceCameraViewPlan);
                        bitmap.SetPixel(row, colunm, shape.TraceRay(ray));
                    }
                }
            }

            return bitmap;
        }
    }
}
