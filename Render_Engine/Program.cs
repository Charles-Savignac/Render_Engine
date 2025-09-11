using Render_Engine.Util;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace Render_Engine
{
    class Programe
    {
        public static void Main(string[] args)
        {
            string fileName = "output.png";

            World world = new World();
            Bitmap bitmap;

            world.Build();
            bitmap = world.RenderScene();

            bitmap.Save(fileName, ImageFormat.Png);
        }
    }
}

