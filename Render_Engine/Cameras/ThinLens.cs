using Render_Engine.Util;
using System;
using System.Drawing;
using System.Net;

namespace Render_Engine.Cameras
{
    internal class ThinLens : Camera
    {
        public double LensRadius { get; set; } = 5f;

        private Random rand = new Random();

        protected override Vector3D GetRayDirection(double px, double py)
        {
            Point3D pixelWorld = Position + (-W * DistanceToViewPlane) + U * px + V * py;
            Vector3D rayDir = pixelWorld - GetRayOrigin(px, py);
            rayDir.Normalize();

            return rayDir;
        }

        protected override Point3D GetRayOrigin(double px, double py)
        {

            double r = LensRadius * Math.Sqrt(rand.NextDouble());
            double theta = 2 * Math.PI * rand.NextDouble();
            double lensU = r * Math.Cos(theta);
            double lensV = r * Math.Sin(theta);

            Point3D lensPoint = Position + U * lensU + V * lensV;

            return lensPoint;
        }
    }
}
