using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;
using System.Numerics;

namespace Render_Engine.Illumination
{
    internal class DirectIllumination : RayTracer
    {
        public DirectIllumination(World w) : base(w) { }

        private bool IsInShadow(Ray shadowRay, float maxDistance, Shape current)
        {
            Intersection inter = new Intersection();
            inter.Ray = shadowRay;

            // Use the grid accelerator to find intersections
            if (!PresentWorld.AccelerationTec.Intersects(ref inter))
                return false;

            // Don't shadow against the shape we are shading
            if (inter.HitShape == current)
                return false;

            // Hit something before reaching the light?
            return inter.t < maxDistance;
        }


        private Color Blend(Color c1, Color c2)
        {
            int r = Math.Min(255, c1.R + c2.R);
            int g = Math.Min(255, c1.G + c2.G);
            int b = Math.Min(255, c1.B + c2.B);
            return Color.FromArgb(r, g, b);
        }

        public override Color TraceRay(Ray r)
        {
            Color pixelColor = PresentWorld.Background_color;

            Intersection inter = new Intersection();
            inter.Ray = r;

            if (!PresentWorld.AccelerationTec.Intersects(ref inter))
                return pixelColor;

            inter.HitPoint = r.Origin + inter.t * r.Direction;

            inter.Normal = inter.HitShape.GetNormal(r, inter.t);
            inter.Normal.Normalize();

            Color objectColor = inter.HitShape.ShapeColor;

            foreach (Light light in Lights)
            {
                Vector3D lightDir = light.GetDirection(inter);
                lightDir.Normalize();
                float lightDist = light.GetDistance(inter);

                Ray shadowRay = new Ray
                {
                    Origin = inter.HitPoint + inter.Normal * 0.001f,
                    Direction = lightDir,
                    T_min = 0.001f,
                    T_max = lightDist
                };

                if (!IsInShadow(shadowRay, lightDist, inter.HitShape))
                {
                    float NdL = Math.Max(0f, inter.Normal * lightDir);

                    Color lightColor = light.GetRadiance();

                    Color diffuse = Color.FromArgb(
                        (int)(objectColor.R * lightColor.R * NdL / 255f),
                        (int)(objectColor.G * lightColor.G * NdL / 255f),
                        (int)(objectColor.B * lightColor.B * NdL / 255f)
                    );

                    pixelColor = Blend(pixelColor, diffuse);
                }
            }

            return pixelColor;
        }
    }
}
