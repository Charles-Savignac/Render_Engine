using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Triangle : Shape
    {
        public Util.Point3D PointA { get; private set; }
        public Util.Point3D PointB { get; private set; }
        public Util.Point3D PointC { get; private set; }
        public Normal Normal { get; set; }

        public Triangle(Util.Point3D o, Color c, Util.Point3D a, Util.Point3D b, Util.Point3D pointc) : base(o, c)
        {
            PointA = a;
            PointB = b;
            PointC = pointc;

            Vector3D edge1 = PointB - PointA;
            Vector3D edge2 = PointC - PointA;

            Normal = new Normal();
            Normal.Assigne(edge1.CrossProduct(edge2));
            Normal.Normalize();

            CalculateBoundingBox();

            Vector3D p1 = PointB - PointA;
            Vector3D p2 = PointC - PointA;

            Surface = 1 / 2 * (p1.CrossProduct(p2).Norm());
        }

        private void CalculateBoundingBox()
        {
            double minX = Math.Min(PointA.X, Math.Min(PointB.X, PointC.X));
            double minY = Math.Min(PointA.Y, Math.Min(PointB.Y, PointC.Y));
            double minZ = Math.Min(PointA.Z, Math.Min(PointB.Z, PointC.Z));

            double maxX = Math.Max(PointA.X, Math.Max(PointB.X, PointC.X));
            double maxY = Math.Max(PointA.Y, Math.Max(PointB.Y, PointC.Y));
            double maxZ = Math.Max(PointA.Z, Math.Max(PointB.Z, PointC.Z));

            ObjectBoundingBox = new BoundingBox(new Util.Point3D(minX, minY, minZ), new Util.Point3D(maxX, maxY, maxZ));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);
        }

        public override bool Intersects(Ray worldRay, ref double t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);
            if (ObjectBoundingBox.Intersects(r))
            {
                Vector3D edge1 = PointB - PointA;
                Vector3D edge2 = PointC - PointA;

                VectorClass h = r.Direction.CrossProduct(edge2);
                Vector3D s = r.Origin - PointA;
                VectorClass q = s.CrossProduct(edge1);

                double a = edge1.Dot(h);
                double f = 1.0f / a;
                double u = f * s.Dot(h);
                double v = f * r.Direction.Dot(q);

                t = f * edge2.Dot(q);

                if (u < 0.0f || u > 1.0f)
                    return false;

                if (v < 0.0f || u + v > 1.0f)
                    return false;

                return t > r.T_min && t < r.T_max;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, double t) => ApplyInvTransformationOnNormal(Normal);

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Triangle\n" +
                   $"   Point_1: {PointA}\n" +
                   $"   Point_2: {PointB}\n" +
                   $"   Point_3: {PointC}\n" +
                   base.ToString();
        }
    }
}
