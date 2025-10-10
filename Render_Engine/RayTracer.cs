using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render_Engine.Util;
using Render_Engine.Shapes;


namespace Render_Engine
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

            float t = 0;
            float tMin = float.MaxValue;

            foreach (Shape s in Present_World.Shapes)
            {
                if (s.Intersects(r, ref t) && t < tMin)
                {
                    Normal n = s.GetNormal(r, t);
                    float blendScalair = MathF.Abs(r.Direction * n);

                    pixelColor = s.Blend(blendScalair);
                    tMin = t;
                }
            }

            return pixelColor;
        }
    }
}
