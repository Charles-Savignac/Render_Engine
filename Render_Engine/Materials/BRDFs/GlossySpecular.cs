using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Materials.BRDFs
{
    class GlossySpecular : BRDF
    {
        public float Ks { get; set; } = 0.5f;          // specular coefficient
        public float Exp { get; set; } = 50.0f;         // shininess exponent
        public Color Cs { get; set; } = Color.White;    // specular color (sRGB)

        public GlossySpecular() { }

        public GlossySpecular(float ks, float exp, Color cs)
        {
            Ks = ks;
            Exp = exp;
            Cs = cs;
        }

        public override Color F(Intersection inter, Vector3D wi, Vector3D wo)
        {
            double ndotwi = inter.Normal * wi;
            if (ndotwi <= 0.0)
                return Color.Black;

            Vector3D r = (Vector3D)(2.0 * ndotwi * inter.Normal - wi);
            r.Normalize();

            double rdotwo = r * wo;
            if (rdotwo <= 0.0)
                return Color.Black;

            double phong = Ks * Math.Pow(rdotwo, Exp);

            Vector3D csLinear = SrgbToLinear(Cs);
            Vector3D result = (Vector3D)(csLinear * phong);

            return LinearToSrgbClamped(result);
        }
    }
}
