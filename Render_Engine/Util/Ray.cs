using System;

namespace Render_Engine.Util
{
    /// <summary>
    /// Represents a ray in 3D space, defined by an origin point and a direction vector.
    /// Includes optional min/max intersection parameters and recursion depth.
    /// </summary>
    internal class Ray
    {
        public Point Origin { get; set; }
        public Vector3D Direction { get; set; }
        public float? T_min { get; set; }
        public float? T_max { get; set; }
        public int Depth { get; set; }

        /// <summary>
        /// Default constructor.
        /// Creates a null ray with:
        /// Origin = (0, 0, 0), Direction = (0, 0, 0),
        /// T_min = null, T_max = null, Depth = 0.
        /// </summary>
        public Ray()
        {
            Origin = new Point();
            Direction = new Vector3D();

            T_min = null;
            T_max = null;
            Depth = 0;
        }

        public Ray(Ray r)
        {
            Origin = r.Origin;
            Direction = r.Direction;

            T_min = r.T_min;
            T_max = r.T_max;
            Depth = r.Depth;
        }

        /// <summary>
        /// Creates a ray from given origin, direction, and parameters.
        /// </summary>
        /// <param name="p">Origin point of the ray.</param>
        /// <param name="d">Direction vector of the ray.</param>
        /// <param name="min">Minimum valid intersection parameter.</param>
        /// <param name="max">Maximum valid intersection parameter.</param>
        /// <param name="depth">Recursion depth (e.g., for reflections).</param>
        public Ray(Point p, Vector3D d, float min, float max, int depth)
        {
            Origin = p;
            Direction = d;

            T_min = min;
            T_max = max;
            Depth = depth;
        }
    }
}
