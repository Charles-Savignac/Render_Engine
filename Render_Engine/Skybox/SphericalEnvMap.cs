using Render_Engine.Skybox;
using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Textures
{
    internal class SphericalEnvMap: EnvironmentMap
    {
        public Bitmap Map;

        public SphericalEnvMap(Bitmap bmp)
        {
            Map = bmp;
        }

        public override Color Sample(Vector3D dir)
        {
            dir.Normalize();

            // longitude (theta) and latitude (phi)
            double u = 0.5 + Math.Atan2(dir.Z, dir.X) / (2 * Math.PI);
            double v = 0.5 - Math.Asin(dir.Y) / Math.PI;

            // clamp
            u = Math.Max(0, Math.Min(1, u));
            v = Math.Max(0, Math.Min(1, v));

            int x = (int)(u * (Map.Width - 1));
            int y = (int)(v * (Map.Height - 1));

            return Map.GetPixel(x, y);
        }
    }
}
