using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Cameras
{
    internal class Pinhole : Camera
    {
        protected override Vector3D GetRayDirection(double px, double py)
        {
            Point3D pixelWorld = Position + (-W * DistanceToViewPlane) + U * px + V * py;

            Vector3D rayDir = pixelWorld - Position;
            rayDir.Normalize();

            return rayDir;
        }

        protected override Point3D GetRayOrigin(double px, double py)
        {
            return Position;
        }
    }
}
