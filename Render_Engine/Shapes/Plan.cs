using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Plan : Shape
    {
        public float Witdh { get; private set; }
        public float Depth { get; private set; }
        public Normal Normal { get; set; }

        public Plan(Util.Point o, Color c, float witdh = 1.0f, float depth = 1.0f) : base(o, c)
        {
            Witdh = witdh;
            Depth = depth;
            Normal = new Normal(0, 1, 0);

            ObjectBoundingBox = new BoundingBox(new Util.Point(-witdh / 2, 0, depth / 2), new Util.Point(witdh / 2, 0, -depth / 2));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);


            Surface = witdh * depth;
        }

        public override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                t = -r.Origin.Y / r.Direction.Y;
                return true;
            }
            return false;
        }

        public override Normal GetNormal(Ray r, float t) => ApplyInvTransformationOnNormal(Normal);

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
