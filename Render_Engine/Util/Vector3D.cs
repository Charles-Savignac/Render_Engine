using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Util
{
    internal class Vector3D : VectorClass
    {
        /// <summary>
        /// Default Vector3D constructor. Creates a null 3D vector [0, 0, 0]
        /// </summary>
        public Vector3D() : base() { }

        public Vector3D(float x, float y, float z) : base(x, y, z) { }

        public Vector3D(Point p) : base(p.X, p.Y, p.Z) { }
    }
}
