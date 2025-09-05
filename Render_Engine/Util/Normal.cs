using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Util
{
    internal class Normal : VectorClass
    {
        public Normal() : base() { }

        public Normal(float x, float y, float z) : base(x, y, z) { }

        public Normal(Point p) : base(p.X, p.Y, p.Z) { }
    }
}
