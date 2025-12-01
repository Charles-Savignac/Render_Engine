using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Cameras
{
    internal class Pinhole : Camera
    {
        protected override Vector3D GetRayDirection(float px, float py)
        {
            Point3D pixelWorld = Position + (-W * DistanceToViewPlane) + U * px + V * py;

            Vector3D rayDir = pixelWorld - Position;
            rayDir.Normalize();

            return rayDir;
        }

        protected override Point3D GetRayOrigin(float px, float py)
        {
            return Position;
        }

        public override Bitmap RenderScene(World world, Bitmap bitmap)
        {
            BuildCCS();

            float halfW = 0.5f * (world.View_plan.X_res - 1);
            float halfH = 0.5f * (world.View_plan.Y_res - 1);


            for (int y = 0; y < world.View_plan.Y_res; y++)
            {
                for (int x = 0; x < world.View_plan.X_res; x++)
                {
                    float px = world.View_plan.Pixel_size * (x - halfW);
                    float py = world.View_plan.Pixel_size * (halfH - y);

                    Ray ray = new Ray
                    {
                        Origin = GetRayOrigin(px, py),
                        Direction = GetRayDirection(px, py),
                        T_min = 0,
                        T_max = 1000
                    };

                    Color c = world.Tracer.TraceRay(ray);
                    bitmap.SetPixel(x, y, c);
                }
            }

            return bitmap;
        }
    }
}
