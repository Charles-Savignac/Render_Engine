using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Cube : Shape
    {
        public float Size { get; private set; }

        public Cube(World w, Util.Point center, Color c, float size = 1.0f) : base(w, center, c)
        {
            Size = size;

            ObjectBoundingBox = new BoundingBox(
                new Util.Point(-size / 2, -size / 2, -size / 2),
                new Util.Point(size / 2, size / 2, size / 2));
            WorldBoundingBox = ObjectBoundingBox;

            Surface = 6 * size * size;
        }

        protected override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyInvTransformationOnRay(worldRay);

            var min = new Util.Point(Origine.X - Size / 2, Origine.Y - Size / 2, Origine.Z - Size / 2);
            var max = new Util.Point(Origine.X + Size / 2, Origine.Y + Size / 2, Origine.Z + Size / 2);

            float tMin = (min.X - r.Origin.X) / r.Direction.X;
            float tMax = (max.X - r.Origin.X) / r.Direction.X;

            if (tMin > tMax) Swap(ref tMin, ref tMax);

            float tyMin = (min.Y - r.Origin.Y) / r.Direction.Y;
            float tyMax = (max.Y - r.Origin.Y) / r.Direction.Y;

            if (tyMin > tyMax) Swap(ref tyMin, ref tyMax);

            if ((tMin > tyMax) || (tyMin > tMax))
                return false;

            if (tyMin > tMin) tMin = tyMin;
            if (tyMax < tMax) tMax = tyMax;

            float tzMin = (min.Z - r.Origin.Z) / r.Direction.Z;
            float tzMax = (max.Z - r.Origin.Z) / r.Direction.Z;

            if (tzMin > tzMax) Swap(ref tzMin, ref tzMax);

            if ((tMin > tzMax) || (tzMin > tMax))
                return false;

            if (tzMin > tMin) tMin = tzMin;
            if (tzMax < tMax) tMax = tzMax;

            t = tMin;
            return true;
        }

        protected override Normal GetNormal(Ray worldRay, float t)
        {
            Ray r = ApplyInvTransformationOnRay(worldRay);

            var intersectionPoint = new Util.Point(
                r.Origin.X + t * r.Direction.X,
                r.Origin.Y + t * r.Direction.Y,
                r.Origin.Z + t * r.Direction.Z);

            // Get cube boundaries in object space
            var min = new Util.Point(Origine.X - Size / 2, Origine.Y - Size / 2, Origine.Z - Size / 2);
            var max = new Util.Point(Origine.X + Size / 2, Origine.Y + Size / 2, Origine.Z + Size / 2);

            // Choose normal based on which face the intersection is closest to
            const float epsilon = 1e-4f; // much larger than float.Epsilon
            Normal n = new Normal();

            if (Math.Abs(intersectionPoint.X - min.X) < epsilon)
                n = new Normal(-1, 0, 0); // Left
            else if (Math.Abs(intersectionPoint.X - max.X) < epsilon)
                n = new Normal(1, 0, 0);  // Right
            else if (Math.Abs(intersectionPoint.Y - min.Y) < epsilon)
                n = new Normal(0, -1, 0); // Bottom
            else if (Math.Abs(intersectionPoint.Y - max.Y) < epsilon)
                n = new Normal(0, 1, 0);  // Top
            else if (Math.Abs(intersectionPoint.Z - min.Z) < epsilon)
                n = new Normal(0, 0, -1); // Back
            else if (Math.Abs(intersectionPoint.Z - max.Z) < epsilon)
                n = new Normal(0, 0, 1);  // Front
            else
            {
                // Fallback (rare): pick the axis with largest absolute coordinate difference
                var dx = Math.Min(Math.Abs(intersectionPoint.X - min.X), Math.Abs(intersectionPoint.X - max.X));
                var dy = Math.Min(Math.Abs(intersectionPoint.Y - min.Y), Math.Abs(intersectionPoint.Y - max.Y));
                var dz = Math.Min(Math.Abs(intersectionPoint.Z - min.Z), Math.Abs(intersectionPoint.Z - max.Z));
                float m = Math.Min(dx, Math.Min(dy, dz));
                if (m == dx) n = new Normal(Math.Sign(intersectionPoint.X), 0, 0);
                else if (m == dy) n = new Normal(0, Math.Sign(intersectionPoint.Y), 0);
                else n = new Normal(0, 0, Math.Sign(intersectionPoint.Z));
            }

            return ApplyInvTransformationOnNormal(n);
        }

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Cube\n" +
                   $"   Size: {Size}\n" +
                   base.ToString();
        }
    }
}