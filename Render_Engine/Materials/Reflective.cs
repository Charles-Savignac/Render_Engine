using Render_Engine.Materials.BRDFs;
using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Materials
{
    internal class Reflective : Material
    {
        private PerfectDiffuse DiffuseBRDF;
        private PerfectSpecular ReflectBRDF;
        private GlossySpecular SpecularBRDF;

        public Reflective(Color c)
        {
            DiffuseBRDF = new PerfectDiffuse();
            ReflectBRDF = new PerfectSpecular();
            SpecularBRDF = new GlossySpecular();
            MatColor = c;

            DiffuseBRDF.SetCoeff(1.0f);
            DiffuseBRDF.SetColor(c);

            ReflectBRDF.Kr = 0.025f;
            ReflectBRDF.Cr = Color.White;

            SpecularBRDF.Ks = 0.5f;
            SpecularBRDF.Exp = 50.0f;
            SpecularBRDF.Cs = Color.White;
        }

        public override Color Shade(Intersection inter)
        {
            Vector3D wo = (Vector3D)(-inter.Ray.Direction);
            wo.Normalize();

            Normal n = inter.Normal;

            double r = 0, g = 0, b = 0;

            // ===== DIFFUSE DIRECT LIGHTING =====
            foreach (var light in inter.World.Tracer.Lights)
            {
                Vector3D wi = light.GetDirection(inter);
                wi.Normalize();

                double cosNI = wi.Dot(n);
                if (cosNI <= 0.0)
                    continue;

                // shadow ray
                Point3D origin = inter.HitPoint + n * 0.001f;

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
                Color gloss = SpecularBRDF.F(inter, wi, wo);

                Vector3D fLinear = BRDF.SrgbToLinear(f);
                Vector3D glossLinear = BRDF.SrgbToLinear(gloss);

                Vector3D Li = BRDF.SrgbToLinear(light.GetRadiance());

                double scale = light.g() * cosNI / light.pdf();

                r += (fLinear.X + glossLinear.X) * Li.X * scale;
                g += (fLinear.Y + glossLinear.Y) * Li.Y * scale;
                b += (fLinear.Z + glossLinear.Z) * Li.Z * scale;
            }

            Vector3D Ld = new Vector3D(r, g, b);

            // ===== REFLECTION =====

            double ndotwo = n * wo;
            Vector3D wr = (Vector3D)(-wo + 2.0 * ndotwo * n);

            Ray reflectRay = new Ray
            {
                Origin = inter.HitPoint + n * 0.001f,
                Direction = wr,
                T_min = 0.001f,
                T_max = double.MaxValue
            };

            Color reflected = inter.World.Tracer.TraceRay(reflectRay, inter.Depth + 1);
            Vector3D reflLinear = BRDF.SrgbToLinear(reflected);

            Color fSpec = ReflectBRDF.FSample(inter, wr, wo);
            Vector3D fSpecLinear = BRDF.SrgbToLinear(fSpec);

            Vector3D result =
                (Vector3D)(Ld +
                new Vector3D(
                    fSpecLinear.X * reflLinear.X,
                    fSpecLinear.Y * reflLinear.Y,
                    fSpecLinear.Z * reflLinear.Z
                ));

            return BRDF.LinearToSrgbClamped(result);
        }

    }
}
