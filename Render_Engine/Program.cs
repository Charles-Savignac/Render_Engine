using Render_Engine.Util;

namespace Render_Engine
{
    class Programe
    {
        public static void Main(string[] args)
        {
            Point p1 = new Point(1,2,3);
            Point p2 = new Point(2,3,4);
            Vector3D v1 = new Vector3D(1,2,3);
            Vector3D v2 = new Vector3D(2,3,4);

            Normal n = v1.CrossProduct(v2);
            
        }
    }
}

