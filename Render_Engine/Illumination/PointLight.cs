using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Illumination
{
    internal class PointLight : Light
    {
        public PointLight() : base(new Point3D(0, 0, 0), Color.White, 1.0f) { }

        public PointLight(Point3D position) : base(position, Color.White, 1.0f) { }
        public PointLight(Point3D position, Color color) : base(position, color, 1.0f) { }
        public PointLight(Point3D position, Color color, float intensity) : base(position, color, intensity) { }

        public override Vector3D GetDirection(Intersection t)
        {
            Vector3D v = new Vector3D(Position.X - t.HitPoint.X, Position.Y - t.HitPoint.Y, Position.Z - t.HitPoint.Z);
            v.Normalize();

            return v;
        }

        public override float GetDistance(Intersection t) => (Position - t.HitPoint).Norm();

        public override Color GetRadiance() => Color.FromArgb((int)(LightIntensity * LightColor.R), 
                                                              (int)(LightIntensity * LightColor.G), 
                                                              (int)(LightIntensity * LightColor.B));
    }
}
