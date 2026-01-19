using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Skybox
{
    abstract class EnvironmentMap
    {
        public abstract Color Sample(Vector3D dir);
    }
}
