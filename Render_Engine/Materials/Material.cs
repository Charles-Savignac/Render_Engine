using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Materials
{
    abstract class Material
    {
        public Color MatColor { get; init; }

        public abstract Color Shade(Intersection inter);
    }
}
