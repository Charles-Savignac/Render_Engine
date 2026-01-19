using Render_Engine.Materials.BRDFs;
using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Materials
{
    class PerfectSpecular : BRDF
    {
        public float Kr { get; set; } = 1.0f;          // reflection coefficient
        public Color Cr { get; set; } = Color.White;   // mirror color (tint)

        public PerfectSpecular() { }

        public PerfectSpecular(float kr, Color cr)
        {
            Kr = kr;
            Cr = cr;
        }

        public override Color FSample(Intersection a_inter, Vector3D a_wi, Vector3D a_wo)
        {
            var n = a_inter.Normal;
            n.Normalize();

            var wo = a_wo;
            wo.Normalize();

            Vector3D crLinear = SrgbToLinear(Cr);
            Vector3D value = (Vector3D)(crLinear * Kr);

            return LinearToSrgbClamped(value);
        }
    }
}
