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
        public float DistanceToViewPlane { get; set; }
        public Vector3D Up { get; private set; }
        public Vector3D U { get; private set; }
        public Vector3D V { get; private set; }
        public Vector3D W { get; private set; }


        public Camera()
        {
            Position = new Point3D(0, 0, 500);
            LookingAt = new Point3D(0, 0, 0);
            DistanceToViewPlane = 100;
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

        protected abstract Vector3D GetRayDirection(float px, float py);

        protected abstract Point3D GetRayOrigin(float px, float py);

        public abstract Bitmap RenderScene(World world, Bitmap bitmap);
    }
}
