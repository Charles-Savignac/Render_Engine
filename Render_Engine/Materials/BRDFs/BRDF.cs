using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Materials.BRDFs
{
    abstract class BRDF
    {
        public virtual Color F(Intersection inter, Vector3D wi, Vector3D wo) => Color.Black;

        public virtual Color FSample(Intersection inter, Vector3D wi, Vector3D wo) => Color.Black;

        public static Vector3D SrgbToLinear(Color c)
        {
            return new Vector3D(
                SrgbChannelToLinear(c.R / 255.0f),
                SrgbChannelToLinear(c.G / 255.0f),
                SrgbChannelToLinear(c.B / 255.0f)
            );
        }

        public static Color LinearToSrgbClamped(Vector3D linear)
        {
            byte r = FloatToByte(LinearChannelToSrgb((float)linear.X));
            byte g = FloatToByte(LinearChannelToSrgb((float)linear.Y));
            byte b = FloatToByte(LinearChannelToSrgb((float)linear.Z));
            return Color.FromArgb(r, g, b);
        }

        private static float SrgbChannelToLinear(float s)
        {
            s = Math.Clamp(s, 0.0f, 1.0f);
            return s <= 0.04045f
                ? s / 12.92f
                : (float)Math.Pow((s + 0.055f) / 1.055f, 2.4f);
        }

        private static float LinearChannelToSrgb(float l)
        {
            l = Math.Clamp(l, 0.0f, 1.0f);
            return l <= 0.0031308f
                ? 12.92f * l
                : 1.055f * (float)Math.Pow(l, 1.0 / 2.4) - 0.055f;
        }

        private static byte FloatToByte(float s)
        {
            int v = (int)Math.Round(s * 255.0f);
            return (byte)Math.Clamp(v, 0, 255);
        }
    }
}
