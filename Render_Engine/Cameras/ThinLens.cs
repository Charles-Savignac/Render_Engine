using Render_Engine.Util;
using System;
using System.Drawing;
using System.Net;

namespace Render_Engine.Cameras
{
    internal class ThinLens : Camera
    {
        public double LensRadius { get; set; } = 0;
        public double FocalDistance { get; set; } = 500;

        private Random rand = new Random();

        protected override Vector3D GetRayDirection(double px, double py)
        {
            // Pixel point on view plane
            Point3D pixelWorld =
                Position
                + (-W * DistanceToViewPlane)
                + U * px
                + V * py;

            // Direction from camera center to pixel
            Vector3D centerDir = pixelWorld - Position;
            centerDir.Normalize();

            // Point where this ray intersects the focal plane
            Point3D focalPoint = Position + centerDir * FocalDistance;

            // Now build direction from lens sample to focal point
            Vector3D dir = focalPoint - GetRayOrigin(px, py);
            dir.Normalize();
            return dir;
        }

        protected override Point3D GetRayOrigin(double px, double py)
        {
            double r = LensRadius * Math.Sqrt(rand.NextDouble());
            double theta = 2 * Math.PI * rand.NextDouble();

            double lensU = r * Math.Cos(theta);
            double lensV = r * Math.Sin(theta);

            return Position + U * lensU + V * lensV;
        }
    }
}
