using Render_Engine.Util;
using Render_Engine.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Render_Engine.Acceleration
{
    internal class GridAccelerator : Accelerator
    {
        public BoundingBox ObjectBoundingBox { get; set; }
        public Vector3D CellSize { get; set; }
        public Vector3D InvCellSize { get; set; }
        public List<Cell> Cells { get; set; }
        public int[] CellCount { get; set; }

        public GridAccelerator(List<Shape> shapes) : base(shapes)
        {
            CellCount = new int[3];

            Cells = new List<Cell>();
            ObjectBoundingBox = new BoundingBox();

            GenerateGrid();
            AddShapeToCells();
        }

        private void GenerateGrid()
        {
            // 1. Calcul du bounding box englobant
            bool first = true;
            foreach (Shape s in Shapes)
            {
                if (first)
                {
                    ObjectBoundingBox = new BoundingBox(s.WorldBoundingBox);
                    first = false;
                }
                else
                {
                    ObjectBoundingBox.Combine(s.WorldBoundingBox);
                }
            }

            // 2. Taille totale du volume
            Point diag = ObjectBoundingBox.PMax - ObjectBoundingBox.PMin;

            // Empêche des divisions nulles
            diag.X = Math.Max(diag.X, 1e-4f);
            diag.Y = Math.Max(diag.Y, 1e-4f);
            diag.Z = Math.Max(diag.Z, 1e-4f);

            // 3. Choix du nombre de cellules global
            int nObjects = Math.Max(1, Shapes.Count);
            double cubeRoot = Math.Cbrt(nObjects);

            // Ajuste la résolution selon la taille relative
            double totalVoxels = 8 * cubeRoot; // un facteur empirique
            double scale = Math.Pow(totalVoxels / (diag.X * diag.Y * diag.Z), 1.0 / 3.0);

            CellCount[0] = Math.Max(1, (int)(diag.X * scale));
            CellCount[1] = Math.Max(1, (int)(diag.Y * scale));
            CellCount[2] = Math.Max(1, (int)(diag.Z * scale));

            // 4. Taille de cellule réelle
            CellSize = new Vector3D(
                (float)(diag.X / CellCount[0]),
                (float)(diag.Y / CellCount[1]),
                (float)(diag.Z / CellCount[2])
            );

            InvCellSize = new Vector3D(
                1.0f / CellSize.X,
                1.0f / CellSize.Y,
                1.0f / CellSize.Z
            );

            // 5. Création des cellules
            int totalCells = CellCount[0] * CellCount[1] * CellCount[2];
            Cells = new List<Cell>(totalCells);
            for (int i = 0; i < totalCells; i++)
                Cells.Add(new Cell());
        }


        private void AddShapeToCells()
        {
            foreach (Shape shape in Shapes)
            {
                BoundingBox shapeBB = shape.WorldBoundingBox;

                int minX = SpaceToCell(shapeBB.PMin, 0);
                int minY = SpaceToCell(shapeBB.PMin, 1);
                int minZ = SpaceToCell(shapeBB.PMin, 2);

                int maxX = SpaceToCell(shapeBB.PMax, 0);
                int maxY = SpaceToCell(shapeBB.PMax, 1);
                int maxZ = SpaceToCell(shapeBB.PMax, 2);

                for (int x = minX; x <= maxX; x++)
                    for (int y = minY; y <= maxY; y++)
                        for (int z = minZ; z <= maxZ; z++)
                            Cells[CellIndex(x, y, z)].AddObject(shape);
            }
        }

        private int SpaceToCell(Point a_p, int axis)
        {
            int v = 0;
            switch (axis)
            {
                case 0:
                    v = (int)Math.Floor((a_p.X - ObjectBoundingBox.PMin.X) * InvCellSize.X);
                    break;
                case 1:
                    v = (int)Math.Floor((a_p.Y - ObjectBoundingBox.PMin.Y) * InvCellSize.Y);
                    break;
                case 2:
                    v = (int)Math.Floor((a_p.Z - ObjectBoundingBox.PMin.Z) * InvCellSize.Z);
                    break;
                default:
                    throw new ArgumentException($"{axis} is not a valid axis");
            }
            return Math.Max(0, Math.Min(v, CellCount[axis] - 1));
        }

        private int CellIndex(int x, int y, int z)
        {
            return x + CellCount[0] * (y + CellCount[1] * z);
        }

        public override bool Intersects(ref Intersection inter)
        {
            // Ray info
            Point origin = inter.Ray.Origin;
            Vector3D dir = inter.Ray.Direction;

            // Convert origin to starting cell indices
            int[] cell = new int[3];
            cell[0] = SpaceToCell(origin, 0);
            cell[1] = SpaceToCell(origin, 1);
            cell[2] = SpaceToCell(origin, 2);

            // Step along each axis (+1 or -1)
            int[] step = new int[3];
            step[0] = dir.X >= 0 ? 1 : -1;
            step[1] = dir.Y >= 0 ? 1 : -1;
            step[2] = dir.Z >= 0 ? 1 : -1;

            // Compute tMax (distance along ray to next cell boundary)
            float[] tMax = new float[3];
            float[] tDelta = new float[3];

            for (int i = 0; i < 3; i++)
            {
                float cellSizeAxis = i == 0 ? CellSize.X : (i == 1 ? CellSize.Y : CellSize.Z);
                float rayDirAxis = i == 0 ? dir.X : (i == 1 ? dir.Y : dir.Z);
                float cellMin = i == 0 ? ObjectBoundingBox.PMin.X : (i == 1 ? ObjectBoundingBox.PMin.Y : ObjectBoundingBox.PMin.Z);

                if (rayDirAxis != 0)
                {
                    // Distance to first boundary along ray
                    float nextBoundary = cellMin + (cell[i] + (step[i] > 0 ? 1 : 0)) * cellSizeAxis;
                    tMax[i] = (nextBoundary - (i == 0 ? origin.X : i == 1 ? origin.Y : origin.Z)) / rayDirAxis;
                    tDelta[i] = MathF.Abs(cellSizeAxis / rayDirAxis);
                }
                else
                {
                    tMax[i] = float.MaxValue;
                    tDelta[i] = float.MaxValue;
                }
            }

            bool hit = false;
            float mint = (float)inter.Ray.T_max;

            // Traverse the grid until we leave it
            while (cell[0] >= 0 && cell[0] < CellCount[0] &&
                   cell[1] >= 0 && cell[1] < CellCount[1] &&
                   cell[2] >= 0 && cell[2] < CellCount[2])
            {
                int idx = CellIndex(cell[0], cell[1], cell[2]);

                if (Cells[idx].Intersects(ref inter))
                {
                    if (inter.t < mint)
                    {
                        mint = inter.t;
                        hit = true;

                        // Optional early-out: intersection is inside this cell
                        if (mint <= MathF.Min(MathF.Min(tMax[0], tMax[1]), tMax[2]))
                            break;
                    }
                }

                // Step to next cell
                if (tMax[0] < tMax[1])
                {
                    if (tMax[0] < tMax[2])
                    {
                        cell[0] += step[0];
                        tMax[0] += tDelta[0];
                    }
                    else
                    {
                        cell[2] += step[2];
                        tMax[2] += tDelta[2];
                    }
                }
                else
                {
                    if (tMax[1] < tMax[2])
                    {
                        cell[1] += step[1];
                        tMax[1] += tDelta[1];
                    }
                    else
                    {
                        cell[2] += step[2];
                        tMax[2] += tDelta[2];
                    }
                }
            }

            inter.t = mint;
            return hit;
        }

    }

    internal class Cell
    {
        public List<Shape> Shapes { get; private set; }

        public Cell()
        {
            Shapes = new List<Shape>();
        }

        public void AddObject(Shape s)
        {
            Shapes.Add(s);
        }

        public int Size()
        {
            return Shapes.Count;
        }

        public bool Intersects(ref Intersection inter)
        {
            bool hit = false;
            float mint = float.MaxValue;

            foreach (Shape obj in Shapes)
            {
                float tmpt = 0;
                if (obj.Intersects(inter.Ray, ref tmpt) && tmpt < mint)
                {
                    mint = tmpt;
                    inter.t = mint;
                    hit = true;
                    inter.HitShape = obj;
                }
            }

            return hit;
        }
    }
}
