using Render_Engine.Shapes;
using Render_Engine.Util;
using System.Drawing;
using System.Numerics;

namespace Render_Engine.Illumination
{
    internal class DirectIlluminationRayTracer : RayTracer
    {
        public DirectIlluminationRayTracer(World w) : base(w) { }

        private bool IsInShadow(Ray shadowRay, double maxDistance, Shape current)
        {
            Intersection inter = new Intersection();
            inter.Ray = shadowRay;

            if (!PresentWorld.AccelerationTec.Intersects(ref inter))
                return false;

            if (inter.HitShape == current)
                return false;

            return inter.t < maxDistance;
        }

        public override Color TraceRay(Ray ray, int depth = 1)
        {
            Color pixelColor = PresentWorld.Background_color;

            Intersection inter = new Intersection();
            inter.World = PresentWorld;
            inter.Ray = ray;

            if (!PresentWorld.AccelerationTec.Intersects(ref inter))
                return pixelColor;

            inter.HitPoint = ray.Origin + inter.t * ray.Direction;
            inter.Normal = inter.HitShape.GetNormal(ray, inter.t);

            pixelColor = inter.HitShape.Material.Shade(inter);

            //Color objectColor = inter.HitShape.ShapeColor;

            //foreach (Light light in Lights)
            //{
            //    Vector3D lightDir = light.GetDirection(inter);
            //    lightDir.Normalize();
            //    double lightDist = light.GetDistance(inter);

            //    Ray shadowRay = new Ray
            //    {
            //        Origin = inter.HitPoint + inter.Normal * 0.001f,
            //        Direction = lightDir,
            //        T_min = 0.001f,
            //        T_max = lightDist
            //    };

            //    if (!IsInShadow(shadowRay, lightDist, inter.HitShape))
            //    {
            //        double NdL = Math.Max(0f, inter.Normal * lightDir);

            //        Color lightColor = light.GetRadiance();

            //        double r = pixelColor.R;
            //        double g = pixelColor.G;
            //        double b = pixelColor.B;

            //        r += objectColor.R * lightColor.R * NdL / 255f;
            //        g += objectColor.G * lightColor.G * NdL / 255f;
            //        b += objectColor.B * lightColor.B * NdL / 255f;

            //        r = Math.Min(255f, r);
            //        g = Math.Min(255f, g);
            //        b = Math.Min(255f, b);

            //        pixelColor = Color.FromArgb((int)r, (int)g, (int)b);
            //    }
            //}

            return pixelColor;
        }
    }
}
