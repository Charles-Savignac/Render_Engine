using Render_Engine.Sampling;
using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Cameras
{
    abstract class Camera
    {
        public Point3D Position { get; private set; }
        public Point3D LookingAt { get; private set; }
        public Vector3D Up { get; private set; }
        public Vector3D U { get; private set; }
        public Vector3D V { get; private set; }
        public Vector3D W { get; private set; }
        public double DistanceToViewPlane { get; set; }


        public Camera()
        {
            Position = new Point3D(0, 0, 500);
            LookingAt = new Point3D(0, 0, 0);
            DistanceToViewPlane = 200;
            Up = new Vector3D(0, 1, 0);
            U = new Vector3D(1, 0, 0);
            V = new Vector3D(0, 1, 0);
            W = new Vector3D(0, 0, 1);
        }

        public Camera(Camera a_cam)
        {
            Position = a_cam.Position;
            LookingAt = a_cam.LookingAt;
            Up = a_cam.Up;
            U = a_cam.U;
            V = a_cam.V;
            W = a_cam.W;
        }

        public void SetCenter(Point3D a_p) => Position = a_p;

        public void SetLookAt(Point3D a_p) => LookingAt = a_p;

        public void SetUpVector(Vector3D a_v) => Up = a_v;

        public void BuildCCS()
        {
            W = new Vector3D(Position.X - LookingAt.X, Position.Y - LookingAt.Y, Position.Z - LookingAt.Z);

            W.Normalize();

            U = Vector3D.Cross(Up, W);
            U.Normalize();

            V = Vector3D.Cross(W, U);

            if (Math.Abs(Position.X - LookingAt.X) < double.Epsilon
                && Math.Abs(Position.Z - LookingAt.Z) < double.Epsilon
                && Position.Y > LookingAt.Y)
            {
                U = new Vector3D(0, 0, 1);
                V = new Vector3D(1, 0, 0);
                W = new Vector3D(0, 1, 0);
            }

            if (Math.Abs(Position.X - LookingAt.X) < double.Epsilon
                && Math.Abs(Position.Z - LookingAt.Z) < double.Epsilon
                && Position.Y < LookingAt.Y)
            {
                U = new Vector3D(1, 0, 0);
                V = new Vector3D(0, 0, 1);
                W = new Vector3D(0, -1, 0);
            }
        }

        public Bitmap RenderScene(World world, Bitmap bitmap)
        {
            BuildCCS();

            double halfW = 0.5f * (world.View_plan.X_res - 1);
            double halfH = 0.5f * (world.View_plan.Y_res - 1);

            int numSamples = world.View_plan.Sampling.NbSamples;

            for (int y = 0; y < world.View_plan.Y_res; y++)
            {
                for (int x = 0; x < world.View_plan.X_res; x++)
                {
                    double rAcc = 0, gAcc = 0, bAcc = 0;

                    for (int s = 0; s < numSamples; s++)
                    {
                        Point3D sample = world.View_plan.Sampling.SampleUnitSquare();

                        double px = world.View_plan.Pixel_size * (halfW - (x + sample.X));
                        double py = world.View_plan.Pixel_size * (halfH - (y + sample.Y));

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

                    int r = Clamp(rAcc / numSamples);
                    int g = Clamp(gAcc / numSamples);
                    int b = Clamp(bAcc / numSamples);

                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return bitmap;
        }

        private int Clamp(double v)
        {
            return (int)Math.Max(0, Math.Min(255, v));
        }


        protected abstract Vector3D GetRayDirection(double px, double py);

        protected abstract Point3D GetRayOrigin(double px, double py);
    }
}
