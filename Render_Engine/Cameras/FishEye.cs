using Render_Engine;
using Render_Engine.Cameras;
using Render_Engine.Util;
using System.Drawing;

internal class Fisheye : Camera
{
    public double FOV { get; set; } = 180.0;

    public override Bitmap RenderScene(World world, Bitmap bitmap)
    {
        BuildCCS();

        double halfW = 0.5 * (world.View_plan.X_res - 1);
        double halfH = 0.5 * (world.View_plan.Y_res - 1);

        int numSamples = world.View_plan.Sampling.NbSamples;

        for (int y = 0; y < world.View_plan.Y_res; y++)
        {
            for (int x = 0; x < world.View_plan.X_res; x++)
            {
                double rAcc = 0, gAcc = 0, bAcc = 0;

                for (int s = 0; s < numSamples; s++)
                {
                    Point3D sample = world.View_plan.Sampling.SampleUnitSquare();

                    // --- convert to normalized fisheye coordinates in [-1, 1] ---
                    double nx = ((x + sample.X) - halfW) / halfW;
                    double ny = ((y + sample.Y) - halfH) / halfH;

                    double r = Math.Sqrt(nx * nx + ny * ny);

                    // outside the fisheye circle -> black
                    if (r > 1.0)
                    {
                        continue;
                    }

                    // convert back into the px/py that GetRayDirection expects
                    double px = nx * DistanceToViewPlane;
                    double py = ny * DistanceToViewPlane;

                    Ray ray = new Ray
                    {
                        Origin = GetRayOrigin(px, py),
                        Direction = GetRayDirection(px, py),
                        T_min = 0,
                        T_max = 1000
                    };

                    Color c = world.Tracer.TraceRay(ray, 0);

                    rAcc += c.R;
                    gAcc += c.G;
                    bAcc += c.B;
                }

                // If all samples were outside the lens, just black
                int rOut = Clamp(rAcc / Math.Max(1, numSamples));
                int gOut = Clamp(gAcc / Math.Max(1, numSamples));
                int bOut = Clamp(bAcc / Math.Max(1, numSamples));

                bitmap.SetPixel(x, y, Color.FromArgb(rOut, gOut, bOut));
            }
        }

        return bitmap;
    }


    protected override Vector3D GetRayDirection(double px, double py)
    {
        // --- 1) normalize px/py to [-1, 1] ---

        // px and py are in world units on the view plane,
        // so normalize them by the view plane half-size.
        // DistanceToViewPlane is along -W, so we use that.

        double nx = px / DistanceToViewPlane;
        double ny = py / DistanceToViewPlane;

        double r = Math.Sqrt(nx * nx + ny * ny);

        // Outside the circle → no ray (return a zero vector)
        if (r > 1.0)
            return new Vector3D(0, 0, 0);

        // --- 2) fisheye mapping ---

        double fovRad = Math.PI * (FOV / 180.0);
        double psi = r * (fovRad / 2.0);

        double sinPsi = Math.Sin(psi);
        double cosPsi = Math.Cos(psi);

        double phi = Math.Atan2(ny, nx);

        Vector3D dir = (Vector3D)
            ((U * (sinPsi * Math.Cos(phi))) +
            (V * (sinPsi * Math.Sin(phi))) +
            (-W * cosPsi));

        dir.Normalize();
        return dir;
    }

    protected override Point3D GetRayOrigin(double px, double py)
    {
        return Position;
    }
}
