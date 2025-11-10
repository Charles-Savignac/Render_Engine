using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Cylinder : Shape
    {
        public float Radius { get; private set; }
        public float MaxY { get; private set; }
        public float MinY { get; private set; }

        public Cylinder(Util.Point o, Color c, float radius = 1.0f, float minY = -1.0f, float maxY = 1.0f) : base(o, c)
        {
            Radius = radius;
            MaxY = maxY;
            MinY = minY;

            ObjectBoundingBox = new BoundingBox(new Util.Point(-radius, minY, -radius), new Util.Point(radius, maxY, radius));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = 2 * MathF.PI * Radius * (MaxY - MinY);
        }

        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                float t0;
                float t1;

                float a = MathF.Pow(r.Direction.X, 2) + MathF.Pow(r.Direction.Z, 2);
                float b = 2 * (r.Direction.X * r.Origin.X + r.Direction.Z * r.Origin.Z);
                float c = MathF.Pow(r.Origin.X, 2) + MathF.Pow(r.Origin.Z, 2) - MathF.Pow(Radius, 2);

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
            //Ray r = ApplyTransformationOnRay(worldRay);

            //Normal n = new Normal();
            //VectorClass result;
            //float z = r.Origin.Z + t * r.Direction.Z;

            //Vector3D uPrime = new Vector3D(2 * MathF.PI * z, 0, -2 * MathF.PI * (r.Origin.X));
            //Vector3D VPrime = new Vector3D(0, MaxY - MinY, 0);

            //result = uPrime.CrossProduct(VPrime);
            //result.Normalize();
            //n.Assigne(result);

            //return ApplyInvTransformationOnNormal(n);

            return ApplyInvTransformationOnNormal(new Normal(0, 1, 0));
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
