using Render_Engine.Acceleration;
using Render_Engine.Materials;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Cylinder : Shape
    {
        public double Radius { get; private set; }
        public double MaxY { get; private set; }
        public double MinY { get; private set; }

        public Cylinder(Point3D o, Material mat, double radius = 1.0f, double minY = -1.0f, double maxY = 1.0f) : base(o, mat)
        {
            Radius = radius;
            MaxY = maxY;
            MinY = minY;

            ObjectBoundingBox = new BoundingBox(new Point3D(-radius, minY, -radius), new Point3D(radius, maxY, radius));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = 2 * Math.PI * Radius * (MaxY - MinY);
        }

        public override bool Intersects(Ray worldRay, ref double t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                double t0;
                double t1;

                double a = Math.Pow(r.Direction.X, 2) + Math.Pow(r.Direction.Z, 2);
                double b = 2 * (r.Direction.X * r.Origin.X + r.Direction.Z * r.Origin.Z);
                double c = Math.Pow(r.Origin.X, 2) + Math.Pow(r.Origin.Z, 2) - Math.Pow(Radius, 2);

                double discriminant = Math.Pow(b, 2) - 4 * a * c;

                if (discriminant < 0)
                    return false;

                double sqrtDiscriminant = Math.Sqrt(discriminant);

                t0 = (-b - sqrtDiscriminant) / (2 * a);
                t1 = (-b + sqrtDiscriminant) / (2 * a);

                if (t0 < r.T_min || t0 > r.T_max)
                    if (t1 < r.T_min || t1 > r.T_max)
                        return false;

                if (r.T_min <= t0 && t0 <= r.T_max)
                {
                    t = t0;
                    return true;
                }
                if (r.T_min <= t1 && t1 <= r.T_max)
                {
                    t = t1;
                    return true;
                }
            }

            return false;
        }

        public override Normal GetNormal(Ray worldRay, double t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            Normal n = new Normal();
            VectorClass result;
            double z = r.Origin.Z + t * r.Direction.Z;

            Vector3D uPrime = new Vector3D(2 * Math.PI * z, 0, -2 * Math.PI * (r.Origin.X));
            Vector3D VPrime = new Vector3D(0, MaxY - MinY, 0);

            result = uPrime.CrossProduct(VPrime);
            result.Normalize();
            n.Assigne(result);

            return ApplyInvTransformationOnNormal(n);
        }

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Cylinder\n" +
                   $"   Radius: {Radius}\n" +
                   $"   MinY: {MinY}\n" +
                   $"   MaxY: {MaxY}\n" +
                   base.ToString();
        }
    }
}
