namespace Render_Engine.Util
{
    /// <summary>
    /// Represents a 3D vector with X, Y, and Z components.
    /// Inherits vector operations and utilities from <see cref="VectorClass"/>.
    /// </summary>
    internal class Vector3D : VectorClass
    {
        /// <summary>
        /// Default constructor.
        /// Initializes the vector to (0, 0, 0).
        /// </summary>
        public Vector3D() : base() { }

        /// <summary>
        /// Creates a 3D vector from individual components.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        /// <param name="z">Z component.</param>
        public Vector3D(float x, float y, float z) : base(x, y, z) { }

        /// <summary>
        /// Creates a 3D vector from a <see cref="Point3D"/> object.
        /// </summary>
        /// <param name="p">A point containing X, Y, and Z values.</param>
        public Vector3D(Point3D p) : base(p.X, p.Y, p.Z) { }
    }
}
