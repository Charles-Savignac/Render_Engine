using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Disk : Shape
    {
        public double RadiusIn { get; private set; }
        public double RadiusOut { get; private set; }
        public Normal Normal { get; init; }

        public Disk(Util.Point3D p, Color c, double radiusIn, double radiusOut) : base(p, c)
        {
            RadiusIn = radiusIn;
            RadiusOut = radiusOut;
            Normal = new Normal(0, 1, 0);

            ObjectBoundingBox = new BoundingBox(new Util.Point3D(-radiusOut, 0, -radiusOut), new Util.Point3D(radiusOut, 0, radiusOut));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = Math.PI * (radiusOut * radiusOut - radiusIn * radiusIn);
        }

        public override bool Intersects(Ray worldRay, ref double t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                Util.Point3D intersectionPoint;
                double distanceSquared;

                t = -r.Origin.Y / r.Direction.Y;

                intersectionPoint = new Util.Point3D(r.Origin.X + t * r.Direction.X, 0, r.Origin.Z + t * r.Direction.Z);
                distanceSquared = Math.Pow(intersectionPoint.X, 2) + Math.Pow(intersectionPoint.Z, 2);

                if (distanceSquared >= Math.Pow(RadiusIn, 2) && distanceSquared <= Math.Pow(RadiusOut, 2))
                    return true;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, double t) => ApplyInvTransformationOnNormal(Normal);

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
