using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Shapes
{
    internal abstract class Shape
    {
        public World Present_world { get; init; }
        public Util.Point Origine { get; set; }
        public Color Shape_color { get; set; }

        public Shape(World world, Util.Point o, Color color)
        {
            Present_world = world;
            Origine = o;
            Shape_color = color;

        }

        public abstract Color TraceRay(Ray r);

        protected abstract bool Intersects(Ray r, ref float t);
    }
}
