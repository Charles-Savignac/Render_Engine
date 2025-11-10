using Render_Engine.Util;

namespace Render_Engine.Acceleration
{
    internal class BoundingBox
    {
        public Point PMax { get; private set; }
        public Point PMin { get; private set; }

        public BoundingBox()
        {
            PMax = new Point(-100, -100, -100);
            PMin = new Point(100, 100, 100);
        }

        public BoundingBox(Point p)
        {
            PMin = p;
            PMax = p;
        }

        public BoundingBox(Point min, Point max)
        {
            PMin = min;
            PMax = max;
        }

        public BoundingBox(BoundingBox bb)
        {
            PMin = bb.PMin;
            PMax = bb.PMax;
        }

        public int MaxAxis()
        {
            float[] AxisLength = {PMax.X - PMin.X,
                                  PMax.Y - PMin.Y,
                                  PMax.Z - PMin.Z };

            return Array.IndexOf(AxisLength, AxisLength.Max());
        }

        public bool Overlaps(BoundingBox box)
        {
            return PMin.X <= box.PMax.X && PMax.X >= box.PMin.X &&
                   PMin.Y <= box.PMax.Y && PMax.Y >= box.PMin.Y &&
                   PMin.Z <= box.PMax.Z && PMax.Z >= box.PMin.Z;
        }

        public bool Contains(Point p)
        {
            return p.X >= PMin.X && p.X <= PMax.X &&
                   p.Y >= PMin.Y && p.Y <= PMax.Y &&
                   p.Z >= PMin.Z && p.Z <= PMax.Z;
        }

        public bool Intersects(Ray r)
        {
            float t0 = (PMin.X - r.Origin.X) / r.Direction.X;
            float t1 = (PMax.X - r.Origin.X) / r.Direction.X;

            if (t0 > t1)
                Swap(ref t0, ref t1);

            float ty0 = (PMin.Y - r.Origin.Y) / r.Direction.Y;
            float ty1 = (PMax.Y - r.Origin.Y) / r.Direction.Y;

            if (ty0 > ty1)
                Swap(ref ty0, ref ty1);

            if (t0 > ty1 || ty0 > t1)
                return false;

            if (ty0 > t0)
                t0 = ty0;
            if (ty1 < t1)
                t1 = ty1;

            float tz0 = (PMin.Z - r.Origin.Z) / r.Direction.Z;
            float tz1 = (PMax.Z - r.Origin.Z) / r.Direction.Z;

            if (tz0 > tz1)
                Swap(ref tz0, ref tz1);

            if (t0 > tz1 || tz0 > t1)
                return false;

            return true;
        }

        private void Swap(ref float a, ref float b)
        {
            float temp = a;
            a = b;
            b = temp;
        }

        public void Combine(BoundingBox box)
        {
            PMin = new Point(
                Math.Min(PMin.X, box.PMin.X),
                Math.Min(PMin.Y, box.PMin.Y),
                Math.Min(PMin.Z, box.PMin.Z)
            );

            PMax = new Point(
                Math.Max(PMax.X, box.PMax.X),
                Math.Max(PMax.Y, box.PMax.Y),
                Math.Max(PMax.Z, box.PMax.Z)
            );
        }


        public void SetPMin(Point p) => PMin = p;

        public void SetPMax(Point p) => PMax = p;

        public override string ToString() => $"Pmin: ({PMin.X}, {PMin.Y}, {PMin.Z}) Pmax: ({PMax.X}, {PMax.Y}, {PMax.Z})";
    }
}
