using Render_Engine.Materials.BRDFs;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Materials
{
    internal class Matte : Material
    {
        private PerfectDiffuse DiffuseBRDF;

        public Matte(Color c, float Coefficient = 1.0f)
        {
            DiffuseBRDF = new PerfectDiffuse();

            DiffuseBRDF.SetCoeff(Coefficient);
            DiffuseBRDF.SetColor(c);

            MatColor = c;
        }

        public override Color Shade(Intersection inter)
        {
            Vector3D wo = (Vector3D)(-inter.Ray.Direction);

            double r = 0, g = 0, b = 0;

            foreach (var light in inter.World.Tracer.Lights)
            {
                Vector3D wi = light.GetDirection(inter);
                wi.Normalize();

                double cosNI = wi.Dot(inter.Normal);
                if (cosNI <= 0.0)
                    continue;

                Point3D origin = inter.HitPoint + inter.Normal * 0.001f;

                Ray shadow = new Ray
                {
                    Origin = origin,
                    Direction = wi,
                    T_min = 0.001f,
                    T_max = double.MaxValue
                };

                Intersection shadowHit = new Intersection
                {
                    Ray = shadow,
                    World = inter.World
                };

                if (inter.World.AccelerationTec.Intersects(ref shadowHit))
                    continue;

                Color f = DiffuseBRDF.F(inter, wi, wo);
                Vector3D fLinear = PerfectDiffuse.SrgbToLinear(f);

                Vector3D Li = PerfectDiffuse.SrgbToLinear(light.GetRadiance());

                double scale = light.g() * cosNI / light.pdf();

                r += fLinear.X * Li.X * scale;
                g += fLinear.Y * Li.Y * scale;
                b += fLinear.Z * Li.Z * scale;
            }

            return PerfectDiffuse.LinearToSrgbClamped(new Vector3D(r, g, b));
        }
    }
}
