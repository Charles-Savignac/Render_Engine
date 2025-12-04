using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Plan : Shape
    {
        public double Witdh { get; private set; }
        public double Depth { get; private set; }
        public Normal Normal { get; set; }

        public Plan(Util.Point3D o, Color c, double witdh = 1.0f, double depth = 1.0f) : base(o, c)
        {
            Witdh = witdh;
            Depth = depth;
            Normal = new Normal(0, 1, 0);

            ObjectBoundingBox = new BoundingBox(new Util.Point3D(-witdh / 2, 0, depth / 2), new Util.Point3D(witdh / 2, 0, -depth / 2));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);


            Surface = witdh * depth;
        }

        public override bool Intersects(Ray worldRay, ref double t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                t = -r.Origin.Y / r.Direction.Y;
                return true;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, double t) => ApplyInvTransformationOnNormal(Normal);

        public override string ToString()
        {
            return $"======Shape_{Id}======\n" +
                   $"   Type: Plan\n" +
                   $"   Witdh: {Witdh}\n" +
                   $"   Depth: {Depth}\n" +
                   base.ToString();
        }
    }
}
