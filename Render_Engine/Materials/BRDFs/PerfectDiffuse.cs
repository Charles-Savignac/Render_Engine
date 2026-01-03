using Render_Engine.Util;
using System;
using System.Drawing;
using System.Numerics;

namespace Render_Engine.Materials.BRDFs
{
    internal class PerfectDiffuse : BRDF
    {
        public float K { get; private set; }
        public Color Color { get; private set; }

        public PerfectDiffuse()
        {
            K = 0.0f;
            Color = Color.Black;
        }

        public override Color F(Intersection inter, Vector3D wi, Vector3D wo)
        {
            Vector3D albedo = SrgbToLinear(Color);
            float k = Math.Clamp(K, 0.0f, 1.0f);

            const float invPi = 1.0f / (float)Math.PI;

            Vector3D linearResult = (Vector3D)(albedo * k * invPi);

            return LinearToSrgbClamped(linearResult);
        }

        public void SetCoeff(float k) => K = k;

        public void SetColor(Color c) => Color = c;
    }
}
