using Render_Engine.Util;
using System;
using System.Drawing;

namespace Render_Engine.Cameras
{
    internal class Fisheye : Camera
    {
        public double FOV { get; set; } = 180f;

        protected override Vector3D GetRayDirection(double px, double py)
        {
            double r = Math.Sqrt(px * px + py * py);
            if (r > 1) return null;

            double psiPixel = r * FOV;

            double phi = Math.Atan2(py, px);

            double sinPsi = Math.Sin(psiPixel);
            double cosPsi = Math.Cos(psiPixel);

            return (Vector3D)(sinPsi * Math.Cos(phi) * U +
                   sinPsi * Math.Sin(phi) * V -
                   cosPsi * W);
        }

        protected override Point3D GetRayOrigin(double px, double py)
        {
            return Position;
        }
    }
}
