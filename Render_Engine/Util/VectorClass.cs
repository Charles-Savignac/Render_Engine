namespace Render_Engine.Util
{
    /// <summary>
    /// Abstract parent class for vector-type data structures.
    /// Provides common vector operations and operator overloads.
    /// </summary>
    internal abstract class VectorClass
    {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Default constructor, initializes vector to (0, 0, 0).
        /// </summary>
        public VectorClass()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Constructor that sets vector components to the given values.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        /// <param name="z">Z component.</param>
        public VectorClass(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Computes the Euclidean length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public double GetLength() => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Computes the norm (magnitude) of the vector.
        /// </summary>
        /// <returns>Norm of the vector.</returns>
        public double Norm() => (double)Math.Sqrt(SquareNorm());

        /// <summary>
        /// Computes the squared norm of the vector.
        /// </summary>
        /// <returns>Squared norm (avoids sqrt for efficiency).</returns>
        public double SquareNorm() => (double)(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        /// <summary>
        /// Computes the dot product between this vector and another.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>Dot product value.</returns>
        public double Dot(VectorClass v2) => X * v2.X + Y * v2.Y + Z * v2.Z;

        /// <summary>
        /// Normalizes the vector in place to unit length.
        /// </summary>
        public void Normalize()
        {
            double norm = Norm();
            X /= norm;
            Y /= norm;
            Z /= norm;
        }

        public static Vector3D Cross(VectorClass v1, VectorClass v2)
        {
            double crossX = v1.Y * v2.Z - v1.Z * v2.Y;
            double crossY = v1.Z * v2.X - v1.X * v2.Z;
            double crossZ = v1.X * v2.Y - v1.Y * v2.X;

            return new Vector3D(crossX, crossY, crossZ);
        }

        /// <summary>
        /// Computes the cross product with another 3D vector.
        /// </summary>
        /// <param name="v">Other vector.</param>
        /// <returns>A new <see cref="Normal"/> vector perpendicular to both.</returns>
        public Normal CrossProduct(Vector3D v)
        {
            double crossX = Y * v.Z - Z * v.Y;
            double crossY = Z * v.X - X * v.Z;
            double crossZ = X * v.Y - Y * v.X;

            return new Normal(crossX, crossY, crossZ);
        }

        /// <summary>
        /// Returns the maximum component (X, Y, or Z).
        /// </summary>
        public double Max()
        {
            if (X > Y && X > Z) return X;
            if (Y > Z) return Y;
            return Z;
        }

        /// <summary>
        /// Returns the minimum component (X, Y, or Z).
        /// </summary>
        public double Min()
        {
            if (X < Y && X < Z) return X;
            if (Y < Z) return Y;
            return Z;
        }

        public override string ToString() => $"[{X}, {Y}, {Z}]";

        public static VectorClass operator +(VectorClass v1, VectorClass v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static VectorClass operator -(VectorClass v1, VectorClass v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static VectorClass operator -(VectorClass v) => -1 * v;
        public static VectorClass operator *(double scalar, VectorClass v) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static VectorClass operator *(VectorClass v, double scalar) => scalar * v;
        public static double operator *(VectorClass v1, VectorClass v2) => v1.Dot(v2);
        public static VectorClass operator /(VectorClass a, double scalair) => 1 / scalair * a;

        public static bool operator ==(VectorClass v1, VectorClass v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        public static bool operator !=(VectorClass v1, VectorClass v2) => !(v1 == v2);
    }
}
