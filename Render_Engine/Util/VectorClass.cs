namespace Render_Engine.Util
{
    /// <summary>
    /// Abstract parent class for vector-type data structures.
    /// Provides common vector operations and operator overloads.
    /// </summary>
    internal abstract class VectorClass
    {

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
        public VectorClass(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Computes the Euclidean length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public float GetLength() => MathF.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Computes the norm (magnitude) of the vector.
        /// </summary>
        /// <returns>Norm of the vector.</returns>
        public float Norm() => (float)Math.Sqrt(SquareNorm());

        /// <summary>
        /// Computes the squared norm of the vector.
        /// </summary>
        /// <returns>Squared norm (avoids sqrt for efficiency).</returns>
        public float SquareNorm() => (float)(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        /// <summary>
        /// Computes the dot product between this vector and another.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>Dot product value.</returns>
        public float Dot(VectorClass v2) => X * v2.X + Y * v2.Y + Z * v2.Z;

        /// <summary>
        /// Normalizes the vector in place to unit length.
        /// </summary>
        public void Normalize()
        {
            float norm = Norm();
            X /= norm;
            Y /= norm;
            Z /= norm;
        }

        /// <summary>
        /// Computes the cross product with another 3D vector.
        /// </summary>
        /// <param name="v">Other vector.</param>
        /// <returns>A new <see cref="Normal"/> vector perpendicular to both.</returns>
        public Normal CrossProduct(Vector3D v)
        {
            float crossX = Y * v.Z - Z * v.Y;
            float crossY = Z * v.X - X * v.Z;
            float crossZ = X * v.Y - Y * v.X;

            return new Normal(crossX, crossY, crossZ);
        }

        /// <summary>
        /// Returns the maximum component (X, Y, or Z).
        /// </summary>
        public float Max()
        {
            if (X > Y && X > Z) return X;
            if (Y > Z) return Y;
            return Z;
        }

        /// <summary>
        /// Returns the minimum component (X, Y, or Z).
        /// </summary>
        public float Min()
        {
            if (X < Y && X < Z) return X;
            if (Y < Z) return Y;
            return Z;
        }

        public override string ToString() => $"[{X}, {Y}, {Z}]";

        public static VectorClass operator +(VectorClass v1, VectorClass v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static VectorClass operator -(VectorClass v1, VectorClass v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static VectorClass operator -(VectorClass v) => -1 * v;
        public static VectorClass operator *(float scalar, VectorClass v) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static VectorClass operator *(VectorClass v, float scalar) => scalar * v;
        public static float operator *(VectorClass v1, VectorClass v2) => v1.Dot(v2);
        public static VectorClass operator /(VectorClass a, float scalair) => 1 / scalair * a;

        public static bool operator ==(VectorClass v1, VectorClass v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        public static bool operator !=(VectorClass v1, VectorClass v2) => !(v1 == v2);
    }
}
