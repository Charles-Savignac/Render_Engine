namespace Render_Engine.Util
{
    /// <summary>
    /// Represents a point in 3D space with X, Y, and Z coordinates.
    /// Supports basic operations such as distance computation and conversion to vectors.
    /// </summary>
    internal class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Default constructor.
        /// Creates a point at the origin (0, 0, 0).
        /// </summary>
        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Creates a point from individual components.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns the largest of the three coordinates.
        /// </summary>
        public float Max()
        {
            if (X > Y && X > Z) return X;
            if (Y > Z) return Y;
            return Z;
        }

        /// <summary>
        /// Returns the smallest of the three coordinates.
        /// </summary>
        public float Min()
        {
            if (X < Y && X < Z) return X;
            if (Y < Z) return Y;
            return Z;
        }

        /// <summary>
        /// Computes the Euclidean distance between two points in 3D space.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Distance between <paramref name="p1"/> and <paramref name="p2"/>.</returns>
        public static float distance(Point p1, Point p2) => (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));

        /// <summary>
        /// Converts this point to a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>A new <see cref="Vector3D"/> with the same coordinates.</returns>
        public Vector3D ToVector3D() => new Vector3D(this);

        public override string ToString() => $"({X}, {Y}, {Z})";

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point operator +(Point p, VectorClass v) => new Point(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Point operator +(VectorClass v, Point p) => p + v;
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point operator *(float scalair, Point p) => new Point(p.X * scalair, p.Y * scalair, p.Z * scalair);
        public static Point operator *(Point p, float scalair) => scalair * p;
        public static Point operator /(Point p, float scalair) => 1 / scalair * p;

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Point a, Point b) => !(a == b);
    }
}
