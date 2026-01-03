using Render_Engine.Acceleration;
using Render_Engine.Materials;
using Render_Engine.Util;

namespace Render_Engine.Shapes
{
    internal class Sphere : Shape
    {
        public double Radius { get; set; }

        public Sphere(Point3D o, Material mat, double radius) : base(o, mat)
        {
            Radius = radius;
            ObjectBoundingBox = new BoundingBox(new Point3D(-radius, -radius, -radius), new Point3D(radius, radius, radius));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = 4 * Math.PI * Radius * Radius;
        }


        public override bool Intersects(Ray worldRay, ref double t)
        {
            Ray r = ApplyInvTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                double t0;
                double t1;

                double a = Math.Pow(r.Direction.X, 2) + Math.Pow(r.Direction.Y, 2) + Math.Pow(r.Direction.Z, 2);
                double b = 2 * (r.Direction.X * r.Origin.X + r.Direction.Y * r.Origin.Y + r.Direction.Z * r.Origin.Z);
                double c = Math.Pow(r.Origin.X, 2) + Math.Pow(r.Origin.Y, 2) + Math.Pow(r.Origin.Z, 2) - Math.Pow(Radius, 2);

                double delta = Math.Pow(b, 2) - 4 * a * c;

                if (delta < 0)
                    return false;

                t0 = (-b - Math.Sqrt(delta)) / (2 * a);
                t1 = (-b + Math.Sqrt(delta)) / (2 * a);

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
            // Step 1: Transform ray into object space
            Ray objectRay = ApplyInvTransformationOnRay(worldRay);

            // Step 2: Compute hit point in object space
            Point3D objectHit = objectRay.Origin + t * objectRay.Direction;

            // Step 3: Normal in object space = hit point (sphere is centered at origin)
            Vector3D localNormal = new Vector3D(objectHit);
            localNormal.Normalize();

            // Step 4: Transform normal back to world space
            return ApplyTransformationOnNormal(new Normal(localNormal));
        }

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Sphere\n" +
                   $"   Radius: {Radius}\n" +
                   base.ToString();
        }
    }
}
