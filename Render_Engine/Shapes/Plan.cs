using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal class Plan : Shape
    {
        public float Witdh { get; private set; }
        public float Depth { get; private set; }

        public Plan(World w, Util.Point o, Color c, float witdh = 1.0f, float depth = 1.0f) : base(w, o, c)
        {
            Witdh = witdh;
            Depth = depth;
            ObjectBoundingBox = new BoundingBox(new Util.Point(-witdh / 2, 0, depth / 2), new Util.Point(witdh / 2, 0, -depth / 2));
            WorldBoundingBox = new BoundingBox(ObjectBoundingBox);

            Surface = witdh * depth;
        }

        protected override bool Intersects(Ray worldRay, ref float t)
        {
            Ray r = ApplyTransformationOnRay(worldRay);

            if (ObjectBoundingBox.Intersects(r))
            {
                t = -r.Origin.Y / r.Direction.Y;
                return true;
            }
            return false;
        }

        protected override Normal GetNormal(Ray r, float t)
        {
            return ApplyInvTransformationOnNormal(new Normal(0, 1, 0));
        }

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
