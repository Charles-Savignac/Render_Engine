using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Util
{
    internal class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Default Point constructor. Creates a point at (0, 0, 0)
        /// </summary>
        public Point() 
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Point(float x, float y, float z)
        {
            X = x; 
            Y = y; 
            Z = z;
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

        public static float distance(Point p1, Point p2) => (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));

        public Vector3D ToVector3D() => new Vector3D(this);

        public override string ToString() => $"({X}, {Y}, {Z})";

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point operator +(Point p, VectorClass v) => new Point(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        public static Point operator +(VectorClass v, Point p) => p + v;
        public static Vector3D operator -(Point a, Point b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point operator *(float scalair, Point p) => new Point(p.X * scalair, p.Y * scalair, p.Z * scalair);
        public static Point operator *(Point p, float scalair) => scalair * p;
        public static Point operator /(Point p, float scalair) => 1 / scalair * p;

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Point a, Point b) => !(a == b);

    }
}
