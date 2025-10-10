using Render_Engine.Acceleration;
using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Shapes
{
    internal class Triangle : Shape
    {
        public Util.Point PointA { get; private set; }
        public Util.Point PointB { get; private set; }
        public Util.Point PointC { get; private set; }
        public Normal Normal { get; set; }

        public Triangle(Util.Point o, Color c, Util.Point a, Util.Point b, Util.Point pointc) : base(o, c)
        {
            PointA = a;
            PointB = b;
            PointC = pointc;

            Vector3D edge1 = new Vector3D(PointB - PointA);
            Vector3D edge2 = new Vector3D(PointC - PointA);

            Normal = new Normal();
            Normal.Assigne(edge1.CrossProduct(edge2));
            Normal.Normalize();

            CalculateBoundingBox();

            Vector3D p1 = new Vector3D(PointB - PointA);
            Vector3D p2 = new Vector3D(PointC - PointA);

            Surface = 1 / 2 * (p1.CrossProduct(p2).Norm());
        }

        private void CalculateBoundingBox()
        {
            float minX = Math.Min(PointA.X, Math.Min(PointB.X, PointC.X));
            float minY = Math.Min(PointA.Y, Math.Min(PointB.Y, PointC.Y));
            float minZ = Math.Min(PointA.Z, Math.Min(PointB.Z, PointC.Z));

            float maxX = Math.Max(PointA.X, Math.Max(PointB.X, PointC.X));
            float maxY = Math.Max(PointA.Y, Math.Max(PointB.Y, PointC.Y));
            float maxZ = Math.Max(PointA.Z, Math.Max(PointB.Z, PointC.Z));

            ObjectBoundingBox = new BoundingBox(new Util.Point(minX, minY, minZ), new Util.Point(maxX, maxY, maxZ));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);
        }

        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);
            if (ObjectBoundingBox.Intersects(r))
            {
                Vector3D edge1 = new Vector3D(PointB - PointA);
                Vector3D edge2 = new Vector3D(PointC - PointA);

                VectorClass h = r.Direction.CrossProduct(edge2);
                Vector3D s = new Vector3D(r.Origin - PointA);
                VectorClass q = s.CrossProduct(edge1);

                float a = edge1.Dot(h);
                float f = 1.0f / a;
                float u = f * s.Dot(h);
                float v = f * r.Direction.Dot(q);

                t = f * edge2.Dot(q);

                if (u < 0.0f || u > 1.0f)
                    return false;

                if (v < 0.0f || u + v > 1.0f)
                    return false;

                return t > r.T_min && t < r.T_max;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, float t) => ApplyInvTransformationOnNormal(Normal);

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
