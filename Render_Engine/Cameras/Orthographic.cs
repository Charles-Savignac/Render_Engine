using Render_Engine.Cameras;
using Render_Engine.Util;
using Render_Engine;
using System.Drawing;

internal class Orthographic : Camera
{

    protected override Vector3D GetRayDirection(double px, double py)
    {
        Vector3D rayDir = (Vector3D)(-W);
        rayDir.Normalize();

        return rayDir;
    }

    protected override Point3D GetRayOrigin(double px, double py)
    {
        return Position + U * px + V * py;
    }
}
