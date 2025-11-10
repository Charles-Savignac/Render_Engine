using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Disk : Shape
    {
        public float RadiusIn { get; private set; }
        public float RadiusOut { get; private set; }
        public Normal Normal { get; init; }

        public Disk(Util.Point p, Color c, float radiusIn, float radiusOut) : base(p, c)
        {
            RadiusIn = radiusIn;
            RadiusOut = radiusOut;
            Normal = new Normal(0, 1, 0);

            ObjectBoundingBox = new BoundingBox(new Util.Point(-radiusOut, 0, -radiusOut), new Util.Point(radiusOut, 0, radiusOut));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = MathF.PI * (radiusOut * radiusOut - radiusIn * radiusIn);
        }

        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                Util.Point intersectionPoint;
                float distanceSquared;

                t = -r.Origin.Y / r.Direction.Y;

                intersectionPoint = new Util.Point(r.Origin.X + t * r.Direction.X, 0, r.Origin.Z + t * r.Direction.Z);
                distanceSquared = MathF.Pow(intersectionPoint.X, 2) + MathF.Pow(intersectionPoint.Z, 2);

                if (distanceSquared >= MathF.Pow(RadiusIn, 2) && distanceSquared <= MathF.Pow(RadiusOut, 2))
                    return true;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, float t) => ApplyInvTransformationOnNormal(Normal);

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Disk\n" +
                   $"   RadiusIn: {RadiusIn}\n" +
                   $"   RadiusOut: {RadiusOut}\n" +
                   base.ToString();
        }
    }
}
