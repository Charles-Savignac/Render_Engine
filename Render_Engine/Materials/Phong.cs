using Render_Engine.Materials.BRDFs;
using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Materials
{
    internal class Phong : Material
    {
        private PerfectDiffuse DiffuseBRDF;
        private GlossySpecular SpecularBRDF;

        public Phong(Color c)
        {
            DiffuseBRDF = new PerfectDiffuse();
            SpecularBRDF = new GlossySpecular();

            MatColor = c;

            DiffuseBRDF.SetCoeff(1.0f);
            DiffuseBRDF.SetColor(c);

            SpecularBRDF.Ks = 0.5f;
            SpecularBRDF.Exp = 50.0f;
            SpecularBRDF.Cs = Color.White;
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

                // --- shadow ray ---
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

                // --- BRDF evaluations ---
                Color fd = DiffuseBRDF.F(inter, wi, wo);
                Color fs = SpecularBRDF.F(inter, wi, wo);

                Vector3D fdLinear = BRDF.SrgbToLinear(fd);
                Vector3D fsLinear = BRDF.SrgbToLinear(fs);

                Vector3D Li = BRDF.SrgbToLinear(light.GetRadiance());

                double scale = light.g() * cosNI / light.pdf();

                // accumulate
                r += (fdLinear.X + fsLinear.X) * Li.X * scale;
                g += (fdLinear.Y + fsLinear.Y) * Li.Y * scale;
                b += (fdLinear.Z + fsLinear.Z) * Li.Z * scale;
            }

            return BRDF.LinearToSrgbClamped(new Vector3D(r, g, b));
        }
    }
}
