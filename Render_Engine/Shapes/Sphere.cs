using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Shapes
{
    internal class Sphere : Shape
    {
        public float Radius { get; set; }

        public Sphere(World w, Util.Point origine, Color color,float radius) : base(w, origine, color) => Radius = radius;

        public override Color TraceRay(Ray r)
        {
            float t = 0;

            return Intersects(r, ref t) ? Shape_color : Present_world.Background_color;
        }

        protected override bool Intersects(Ray r, ref float t)
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
    }
}
