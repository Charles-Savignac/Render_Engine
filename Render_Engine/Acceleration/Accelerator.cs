using Render_Engine.Shapes;
using Render_Engine.Util;

namespace Render_Engine.Acceleration
{
    abstract class Accelerator
    {
        public List<Shape> Shapes { get; set; }

        public Accelerator(List<Shape> shapes)
        {
            Shapes = shapes;
        }

        public abstract bool Intersects(ref Intersection inter);
    }
}
