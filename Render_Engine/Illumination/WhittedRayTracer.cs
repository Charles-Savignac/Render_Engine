using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Illumination
{
    internal class WhittedRayTracer : RayTracer
    {
        public int MaxDepth { get; set; } = 5;

        public WhittedRayTracer(World w) : base(w) { }

        public override Color TraceRay(Ray ray, int depth)
        {
            Color pixelColor;

            if (PresentWorld.Skybox == null)
                pixelColor = PresentWorld.Background_color;
            else
                pixelColor = PresentWorld.Skybox.Sample(ray.Direction);


            Intersection inter = new Intersection();
            inter.World = PresentWorld;
            inter.Ray = ray;
            inter.Depth = depth;

            if (depth > MaxDepth || !PresentWorld.AccelerationTec.Intersects(ref inter))
                return pixelColor;

            inter.HitPoint = ray.Origin + inter.t * ray.Direction;
            inter.Normal = inter.HitShape.GetNormal(ray, inter.t);

            pixelColor = inter.HitShape.Material.Shade(inter);

            return pixelColor;
        }
    }
}
