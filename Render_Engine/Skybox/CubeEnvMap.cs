using Render_Engine.Skybox;
using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Textures
{
    internal class CubeEnvMap : EnvironmentMap
    {
        public Bitmap PosX, NegX, PosY, NegY, PosZ, NegZ;

        public override Color Sample(Vector3D dir)
        {
            dir.Normalize();
            double ax = Math.Abs(dir.X);
            double ay = Math.Abs(dir.Y);
            double az = Math.Abs(dir.Z);

            if (ax >= ay && ax >= az)
            {
                // X face
                if (dir.X > 0) return SampleFace(PosX, dir.Y / ax, -dir.Z / ax);
                else return SampleFace(NegX, dir.Y / ax, dir.Z / ax);
            }
            else if (ay >= ax && ay >= az)
            {
                // Y face
                if (dir.Y > 0) return SampleFace(PosY, -dir.Z / ay, -dir.X / ay);
                else return SampleFace(NegY, dir.Z / ay, -dir.X / ay);
            }
            else
            {
                // Z face
                if (dir.Z > 0) return SampleFace(PosZ, dir.Y / az, dir.X / az);
                else return SampleFace(NegZ, dir.Y / az, -dir.X / az);
            }
        }

        private Color SampleFace(Bitmap bmp, double u, double v)
        {
            double s = (u + 1.0) * 0.5;
            double t = (v + 1.0) * 0.5;

            int x = (int)(s * (bmp.Width - 1));
            int y = (int)((1 - t) * (bmp.Height - 1));

            return bmp.GetPixel(x, y);
        }
    }
}
