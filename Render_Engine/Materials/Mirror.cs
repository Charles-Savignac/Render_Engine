using Render_Engine.Materials.BRDFs;
using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Materials
{
    internal class Mirror : Material
    {
        private PerfectSpecular ReflectBRDF;

        // max recursion to avoid infinite loops
        public int MaxDepth { get; set; } = 5;

        public Mirror()
        {
            ReflectBRDF = new PerfectSpecular
            {
                Kr = 1.0f,
                Cr = Color.White
            };
        }

        public Mirror(float kr, Color tint)
        {
            ReflectBRDF = new PerfectSpecular
            {
                Kr = kr,
                Cr = tint
            };
        }

        public override Color Shade(Intersection inter)
        {
            // outgoing direction toward camera
            Vector3D wo = (Vector3D)(-inter.Ray.Direction);
            wo.Normalize();

            Normal n = inter.Normal;

            double ndotwo = n * wo;
            if (ndotwo <= 0.0)
                return Color.Black;

            // --- reflection direction ---
            Vector3D wi = (Vector3D)(-wo + 2.0 * ndotwo * n);
            wi.Normalize();

            // --- build reflection ray ---
            Point3D origin = inter.HitPoint + n * 0.001f;

            Ray reflectRay = new Ray
            {
                Origin = origin,
                Direction = wi,
                T_min = 0.001f,
                T_max = double.MaxValue
            };

            // trace reflected ray
            Color reflected = inter.World.Tracer.TraceRay(reflectRay, inter.Depth + 1);

            // BRDF contribution (tint + strength)
            Color f = ReflectBRDF.FSample(inter, wi, wo);

            var fLinear = BRDF.SrgbToLinear(f);
            var reflLinear = BRDF.SrgbToLinear(reflected);

            Vector3D result = new Vector3D(
                fLinear.X * reflLinear.X,
                fLinear.Y * reflLinear.Y,
                fLinear.Z * reflLinear.Z
            );

            return BRDF.LinearToSrgbClamped(result);
        }
    }
}
