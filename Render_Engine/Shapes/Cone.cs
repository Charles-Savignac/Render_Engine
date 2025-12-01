using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Cone : Shape
    {
        public float Radius { get; private set; }
        public float Height { get; private set; }

        public Cone(Util.Point3D p, Color c, float radius = 1.0f, float height = 1.0f) : base(p, c)
        {
            Radius = radius;
            Height = height;

            ObjectBoundingBox = new BoundingBox(new Util.Point3D(-radius, 0, -radius), new Util.Point3D(radius, height, radius));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = MathF.PI * radius * MathF.Sqrt(radius * radius + height * height);
        }

        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                float t0;
                float t1;

                float a = MathF.Pow(r.Direction.X, 2) + MathF.Pow(r.Direction.Z, 2) - MathF.Pow(Radius / Height, 2) * MathF.Pow(r.Direction.Y, 2);
                float b = 2 * (r.Origin.X * r.Direction.X + r.Origin.Z * r.Direction.Z - (Radius / Height) * r.Origin.Y * r.Direction.Y);
                float c = MathF.Pow(r.Origin.X, 2) + MathF.Pow(r.Origin.Z, 2) - MathF.Pow(Radius / Height, 2) * MathF.Pow(r.Origin.Y, 2);

                float discriminant = MathF.Pow(b, 2) - 4 * a * c;

                if (discriminant < 0)
                    return false;

                float sqrtDiscriminant = MathF.Sqrt(discriminant);

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

        public override Normal GetNormal(Ray worldRay, float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            Normal n = new Normal();
            VectorClass result;

            float x = r.Origin.X + t * r.Direction.X;
            float z = r.Origin.Z + t * r.Direction.Z;

            float slope = Radius / Height;

            Vector3D uPrime = new Vector3D(2 * MathF.PI * (z), 0, -2 * MathF.PI * r.Origin.X);
            Vector3D vPrime = new Vector3D(0, Height, Radius);

            result = uPrime.CrossProduct(vPrime);
            result.Normalize();

            n.Assigne(result);

            return ApplyInvTransformationOnNormal(n);
        }


        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Cone\n" +
                   $"   Radius: {Radius}\n" +
                   $"   Height: {Height}\n" +
                   base.ToString();
        }

    }
}