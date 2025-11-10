using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;


namespace Render_Engine.Illumination
{
    internal class RayTracer
    {
        public World Present_World { get; init; }

        public RayTracer(World w)
        {
            Present_World = w;
        }

        public Color TraceRay(Ray r)
        {
            Color pixelColor = Present_World.Background_color;
            Intersection intersection = new Intersection();

            intersection.Ray = r;

            if (Present_World.AccelerationTec.Intersects(ref intersection))
            {
                intersection.Normal = intersection.HitShape.GetNormal(r, intersection.t);
                float blendScalair = MathF.Abs(r.Direction * intersection.Normal);

                pixelColor = intersection.HitShape.Blend(blendScalair);
            }
            return pixelColor;
        }

    }
}
