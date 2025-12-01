using Render_Engine.Shapes;

namespace Render_Engine.Util
{
    internal struct Intersection
    {
        public Ray? Ray { get; set; }
        public Point3D? HitPoint { get; set; }
        public Normal? Normal { get; set; }
        public Shape? HitShape { get; set; }
        public float t { get; set; }

        public Intersection()
        {
            t = 0f;
        }
    }
}
