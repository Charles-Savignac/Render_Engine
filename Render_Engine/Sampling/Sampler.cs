using Render_Engine.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Sampling
{
    abstract class Sampler
    {
        public int NbSamples { get; init; }
        public int NbSets { get; init; }
        public int Count { get; protected set; }
        public int Jump { get; protected set; }
        public List<int> ShuffledIndices { get; set; } 
        public List<Point3D> Samples { get; private set; }
        public List<Point3D> CircleSamples { get; protected set; }
        public List<Point3D> HemisphereSamples { get; protected set; }

        public Sampler()
        {
            NbSamples = 1;
            NbSets = 1;
            Count = 0;
            Jump = 0;

            Samples = new List<Point3D>();
            SetupShuffledIndices();
            GenerateSamples();
        }

        public Sampler(int a_nbSamples)
        {
            NbSamples = a_nbSamples;
            NbSets = 1;
            Count = 0;
            Jump = 0;

            Samples = new List<Point3D>();
            SetupShuffledIndices();
            GenerateSamples();
        }

        public Sampler(int a_nbSamples, int a_nbSets)
        {
            NbSamples = a_nbSamples;
            NbSets = a_nbSets;
            Count = 0;
            Jump = 0;

            Samples = new List<Point3D>();
            SetupShuffledIndices();
            GenerateSamples();
        }

        public Point3D SampleUnitSquare()
        {
            if (Count % NbSamples == 0)
            {
                Random rand = new Random();
                Jump = rand.Next(NbSets) * NbSamples;
            }

            return Samples[Jump + ShuffledIndices[(int)(Jump + Count++ % NbSamples)]];
        }

        public Point3D SampleUnitCircle()
        {
            if (Count % NbSamples == 0)
            {
                Random rand = new Random();
                Jump = rand.Next(NbSets) * NbSamples;
            }

            return CircleSamples[Jump + ShuffledIndices[(int)(Jump + Count++ % NbSamples)]];
        }

        public Point3D SampleUnitHemisphere()
        {
            if (Count % NbSamples == 0)
            {
                Random rand = new Random();
                Jump = rand.Next(NbSets) * NbSamples;
            }

            return HemisphereSamples[Jump + ShuffledIndices[(int)(Jump + Count++ % NbSamples)]];
        }

        public void MapSquare2Circle()
        {
            double x = 0;
            double y = 0;
            double cx = 0;
            double cy = 0;
            double r = 0;
            double phi = 0;
            CircleSamples = new List<Point3D>();

            for (int i = 0; i < NbSamples * NbSets; ++i)
            {
                x = Samples[i].X;
                y = Samples[i].Y;

                if (x > y && x > -y)
                {
                    r = x;
                    phi = Math.PI / 4 * (y / x);

                }
                else if (x < y && x > -y)
                {
                    r = y;
                    phi = Math.PI / 4 * (2 - x / y);
                }
                else if (x < y && x < -y)
                {
                    r = -x;
                    phi = Math.PI / 4 * (4 + y / x);
                }
                else if (x > y && x < -y)
                {
                    r = -y;
                    phi = Math.PI / 4 * (6 - x / y);
                }
                cx = r * Math.Cos(phi);
                cy = r * Math.Sin(phi);

                CircleSamples.Add(new Point3D(cx, cy, 0));
            }
        }

        public void MapSquare2Hemisphere(double a_alpha)
        {

            double x = 0;
            double y = 0;
            double theta = 0;
            double phi = 0;
            HemisphereSamples = new List<Point3D>();

            for (int i = 0; i < NbSamples * NbSets; ++i)
            {
                x = Samples[i].X;
                y = Samples[i].Y;

                phi = 2 * Math.PI * x;
                theta = Math.Acos(Math.Pow(1 - y, 1 / (a_alpha + 1)));


            }
        }

        public void SetupShuffledIndices()
        {
            ShuffledIndices = new List<int>(NbSamples * NbSets);
            List<int> indices = new List<int>();

            for (int i = 0; i < NbSamples; ++i)
                indices.Add(i);

            Random rand = new Random();

            for (int j = 0; j < NbSets; ++j)
            {
                for (int i = 0; i < NbSamples; ++i)
                {
                    int swapIndex = rand.Next(i, NbSamples);
                    int temp = indices[i];
                    indices[i] = indices[swapIndex];
                    indices[swapIndex] = temp;
                }

                ShuffledIndices.AddRange(indices);
            }
        }

        public abstract void GenerateSamples();
    }
}
