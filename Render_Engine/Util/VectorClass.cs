using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Util
{
    /// <summary>
    /// Abstract parent class for vector type data structers.
    /// </summary>
    internal abstract class VectorClass
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Default constructer, sets all values to 0.
        /// </summary>
        public VectorClass() {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Vector contructer, sets X, Y, Z to param values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public VectorClass(float x, float y, float z)
        {
            X = x; 
            Y = y;
            Z = z;
        }

        public float GetLength() => MathF.Sqrt(X * X + Y * Y + Z * Z);

        public float Norm() => (float)Math.Sqrt(SquareNorm());

        public float SquareNorm() => (float)(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        public float Dot(VectorClass v2) => X * v2.X + Y * v2.Y + Z * v2.Z;

        public void Normalize()
        {
            float norm = Norm();

            X /= norm;
            Y /= norm;
            Z /= norm;
        }

        public Normal CrossProduct(Vector3D v)
        {
            float crossX = Y * v.Z - Z * v.Y;
            float crossY = Z * v.X - X * v.Z;
            float crossZ = X * v.Y - Y * v.X;

            return new Normal(crossX, crossY, crossZ);
        }

        public float Max()
        {
            if (X > Y && X > Z) return X;
            if (Y > Z) return Y;
            return Z;
        }

        public float Min()
        {
            if (X < Y && X < Z) return X;
            if (Y < Z) return Y;
            return Z;
        }

        public override string ToString() => $"[{X}, {Y}, {Z}]";

        public static VectorClass operator +(VectorClass v1, VectorClass v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static VectorClass operator -(VectorClass v1, VectorClass v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static VectorClass operator -(VectorClass v) => -1*v;
        public static VectorClass operator *(float scalar, VectorClass v) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static VectorClass operator *(VectorClass v, float scalar) => scalar * v;
        public static float operator *(VectorClass v1, VectorClass v2) => v1.Dot(v2);
        public static VectorClass operator /(VectorClass a, float scalair) => 1 / scalair * a;

        public static bool operator ==(VectorClass v1, VectorClass v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        public static bool operator !=(VectorClass v1, VectorClass v2) => !(v1 == v2);
    }
}
