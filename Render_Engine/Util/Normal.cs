namespace Render_Engine.Util
{
    /// <summary>
    /// Represents a normal vector in 3D space.
    /// Inherits functionality from <see cref="VectorClass"/>.
    /// </summary>
    internal class Normal : VectorClass
    {
        /// <summary>
        /// Default constructor.
        /// Initializes the normal vector to (0, 0, 0).
        /// </summary>
        public Normal() : base() { }

        /// <summary>
        /// Creates a normal vector from individual components.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        /// <param name="z">Z component.</param>
        public Normal(float x, float y, float z) : base(x, y, z) { }

        /// <summary>
        /// Creates a normal vector from a <see cref="Point"/> object.
        /// </summary>
        /// <param name="p">A point containing X, Y, and Z values.</param>
        public Normal(Point p) : base(p.X, p.Y, p.Z) { }

        public void Assigne(VectorClass v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }
    }
}