using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Util
{
    internal class Ray
    {
        public Point Origin { get; set; }
        public Vector3D Direction { get; set; }
        public float? Tmin { get; set; }
        public float? Tmax { get; set; }
        public int Depth { get; set; }


        public Ray()
        {
            Origin = new Point();
            Direction = new Vector3D();

            Tmin = null;
            Tmax = null;
            Depth = 0;
        }
    }
}
