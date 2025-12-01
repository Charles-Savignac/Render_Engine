using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;


namespace Render_Engine.Illumination
{
    abstract class RayTracer
    {
        public World PresentWorld { get; init; }
        public List<Shape> Shapes { get; private set; }
        public List<Light> Lights { get; private set; }



        public RayTracer(World w)
        {
            PresentWorld = w;
            Shapes = new List<Shape>();
            Lights = new List<Light>();
        }

        public void AddShapes(params Shape[] s)
        {
            foreach (Shape shape in s)
            {
                Shapes.Add(shape);
                Console.WriteLine(shape);
            }
        }
        public void AddLightSource(params Light[] l)
        {
            foreach (Light light in l)
            {
                Lights.Add(light);
                
                //Console.WriteLine(light);
            }
        }

        public abstract Color TraceRay(Ray r);
    }
}
