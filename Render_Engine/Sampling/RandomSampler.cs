using Render_Engine.Util;
using System;

namespace Render_Engine.Sampling
{
    internal class RandomSampler : Sampler
    {
        private Random rand = new Random();

        public RandomSampler() : base() { }
        public RandomSampler(int nbSamples) : base(nbSamples) { }
        public RandomSampler(int nbSamples, int nbSets) : base(nbSamples, nbSets) { }


        public override void GenerateSamples()
        {
            int n = NbSamples * NbSamples;

            for (int s = 0; s < NbSets; s++)
            {
                for (int i = 0; i < n; i++)
                {
                    double x = rand.NextDouble();
                    double y = rand.NextDouble();

                    Samples.Add(new Point3D(x, y, 0));
                }
            }
        }
    }
}
