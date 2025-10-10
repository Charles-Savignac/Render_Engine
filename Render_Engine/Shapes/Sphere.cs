using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Sphere : Shape
    {
        public float Radius { get; set; }

        public Sphere(Util.Point o, Color c, float radius) : base(o, c)
        {
            Radius = radius;
            ObjectBoundingBox = new BoundingBox(new Util.Point(-radius, -radius, -radius), new Util.Point(radius, radius, radius));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = 4 * MathF.PI * Radius * Radius;
        }


        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                float t0;
                float t1;

                float a = MathF.Pow(r.Direction.X, 2) + MathF.Pow(r.Direction.Y, 2) + MathF.Pow(r.Direction.Z, 2);
                float b = 2 * (r.Direction.X * r.Origin.X + r.Direction.Y * r.Origin.Y + r.Direction.Z * r.Origin.Z);
                float c = MathF.Pow(r.Origin.X, 2) + MathF.Pow(r.Origin.Y, 2) + MathF.Pow(r.Origin.Z, 2) - MathF.Pow(Radius, 2);

                float delta = MathF.Pow(b, 2) - 4 * a * c;

                if (delta < 0)
                    return false;

                t0 = (-b - MathF.Sqrt(delta)) / (2 * a);
                t1 = (-b + MathF.Sqrt(delta)) / (2 * a);

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

        public override Normal GetNormal(Ray worldRay, float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            Normal n = new Normal();
            VectorClass result;

            float z = r.Origin.Z + t * r.Direction.Z;
            float theta = MathF.Acos(r.Origin.Y / Radius);

            float rsinTheta = MathF.Sqrt(MathF.Pow(r.Origin.X, 2) + MathF.Pow(z, 2));
            float cosFi = z / (Radius * MathF.Sin(theta));
            float sinFi = r.Origin.X / (Radius * MathF.Sin(theta));

            Vector3D uPrime = new Vector3D(2 * MathF.PI * z, 0, -2 * MathF.PI * r.Origin.X);
            Vector3D vPrime = new Vector3D(r.Origin.Y * sinFi, -rsinTheta, r.Origin.Y * cosFi);

            vPrime = (Vector3D)(vPrime * MathF.PI);

            result = uPrime.CrossProduct(vPrime);
            result.Normalize();
            n.Assigne(result);

            return ApplyInvTransformationOnNormal(n);
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
