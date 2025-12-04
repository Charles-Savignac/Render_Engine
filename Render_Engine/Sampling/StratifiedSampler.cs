using Render_Engine.Util;
using System;

namespace Render_Engine.Sampling
{
    internal class StratifiedSampler : Sampler
    {
        private Random rand = new Random();

        public StratifiedSampler() : base() { }
        public StratifiedSampler(int nbSamples) : base(nbSamples) { }
        public StratifiedSampler(int nbSamples, int nbSets) : base(nbSamples, nbSets) { }

        public override void GenerateSamples()
        {
            int n = NbSamples;
            int total = n * n;
            double invN = 1.0 / n;

            for (int s = 0; s < NbSets; s++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double x = (i + rand.NextDouble()) * invN;
                        double y = (j + rand.NextDouble()) * invN;

                        Samples.Add(new Point3D(x, y, 0));
                    }
                }
            }
        }
    }
}
