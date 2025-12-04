using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Sampling
{
    internal class UniformSampler : Sampler
    {
        public UniformSampler() : base() { }
        public UniformSampler(int nbSamples) : base(nbSamples) { }

        public override void GenerateSamples()
        {
            int n = NbSamples * NbSamples;

            for (int s = 0; s < NbSets; s++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Samples.Add(new Point3D((double)(i + 0.5 / n), (double)(j + 0.5 / n), 0));
                    }
                }
            }
        }
    }
}
