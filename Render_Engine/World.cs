using Render_Engine.Util;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Metadata.Ecma335;

namespace Render_Engine
{

    internal class sphere
    {
        public World w { get; set; }
        public Util.Point o { get; set; }
        public float Radius { get; set; }

        public sphere(World wo)
        {
            w = wo;
            o = new Util.Point(0, 0 , 100);
            Radius = 100.0f;
        }

        private bool intersects(Ray r, ref float t)
        {
            float t0;
            float t1;

            float a = MathF.Pow(r.Direction.X, 2) + MathF.Pow(r.Direction.Y, 2) + MathF.Pow(r.Direction.Z, 2);
            float b = 2 * (r.Direction.X * r.Origin.X + r.Direction.Y * r.Origin.Y + r.Direction.Z * r.Origin.Z);
            float c = MathF.Pow(r.Origin.X, 2) + MathF.Pow(r.Origin.Y, 2) + MathF.Pow(r.Origin.Z, 2) - MathF.Pow(Radius, 2);

            float delta = MathF.Pow(b, 2) - 4 * a * c;

            if (delta < 0)
                return false;

            t0 = (-b - MathF.Sqrt(delta)) / (2 * a);
            t1 = (-b + MathF.Sqrt(delta)) / (2 * a);

            if (t0 < r.T_min || t0 > r.T_max)
                if (t1 < r.T_min || t1 > r.T_max)
                    return false;

            if (r.T_min <= t0 && t0 <= r.T_max)
            {
                t = t0;
                return true;
            }
            if (r.T_min <= t1 && t1 <= r.T_max)
            {
                t = t1;
                return true;
            }

            return false;
        }

        public Color traceray(Ray r)
        {

            float t = 0;

           return intersects(r, ref t) ? Color.Red : w.Background_color;
        }
    }

    internal class World
    {
        public ViewPlan View_plan { get; set; }
        public Color Background_color { get; set; }
        public sphere s { get; set; }

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan();
            Background_color = Color.Black;

            s = new sphere(this);
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
                    float x = View_plan.Pixel_size * (colunm - 0.5f * (View_plan.X_res - 1));
                    float y = View_plan.Pixel_size * (row - 0.5f * (View_plan.Y_res - 1));

                    ray.Origin = new Util.Point(x, y, distanceCameraViewPlan);
                    bitmap.SetPixel(row, colunm, s.traceray(ray));
                }

            }

            return bitmap;
        }
    }
}
