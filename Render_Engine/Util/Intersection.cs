using Render_Engine.Shapes;

namespace Render_Engine.Util
{
    internal struct Intersection
    {
        public Ray? Ray { get; set; }
        public Point3D? HitPoint { get; set; }
        public Normal? Normal { get; set; }
        public Shape? HitShape { get; set; }
        public World World { get; set; }
        public double t { get; set; }
        public int Depth { get; set; }
        public int MaxDepth { get; set; }

        public Intersection()
        {
            t = 0f;
        }
    }
}
