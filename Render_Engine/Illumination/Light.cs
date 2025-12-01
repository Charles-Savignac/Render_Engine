using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Illumination
{
    abstract class Light
    {
        public Point3D Position { get; private set; }
        public Color LightColor { get; private set; }
        public float LightIntensity { get; private set; }

        protected Light(Point3D position, Color color, float iintensity)
        {
            Position = position;
            LightColor = color;
            LightIntensity = iintensity;
        }

        public abstract Vector3D GetDirection(Intersection t);

        public abstract float GetDistance(Intersection t);

        public abstract Color GetRadiance();

        public virtual float g() => 1;

        public virtual float pdf() => 1;
    }
}

