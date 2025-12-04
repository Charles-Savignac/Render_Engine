namespace Render_Engine.Util
{
    /// <summary>
    /// Represents a point in 3D space with X, Y, and Z coordinates.
    /// Supports basic operations such as distance computation and conversion to vectors.
    /// </summary>
    internal class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Default constructor.
        /// Creates a point at the origin (0, 0, 0).
        /// </summary>
        public Point3D()
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
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(Point3D p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }

        /// <summary>
        /// Returns the largest of the three coordinates.
        /// </summary>
        public double Max()
        {
            if (X > Y && X > Z) return X;
            if (Y > Z) return Y;
            return Z;
        }

        /// <summary>
        /// Returns the smallest of the three coordinates.
        /// </summary>
        public double Min()
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
        public static double distance(Point3D p1, Point3D p2) => (double)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));

        /// <summary>
        /// Converts this point to a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>A new <see cref="Vector3D"/> with the same coordinates.</returns>
        public Vector3D ToVector3D() => new Vector3D(this);

        public override string ToString() => $"({X}, {Y}, {Z})";

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point3D operator +(Point3D p, VectorClass v) => new Point3D(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Point3D operator +(VectorClass v, Point3D p) => p + v;
        public static Vector3D operator -(Point3D a, Point3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point3D operator *(double scalair, Point3D p) => new Point3D(p.X * scalair, p.Y * scalair, p.Z * scalair);
        public static Point3D operator *(Point3D p, double scalair) => scalair * p;
        public static Point3D operator /(Point3D p, double scalair) => 1 / scalair * p;

        public static bool operator ==(Point3D a, Point3D b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Point3D a, Point3D b) => !(a == b);
    }
}
