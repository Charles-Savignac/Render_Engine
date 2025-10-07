using Render_Engine.Acceleration;
using Render_Engine.Util;
using System.Drawing;

namespace Render_Engine.Shapes
{
    internal abstract class Shape
    {
        public int Id { get; set; }
        public World Present_world { get; init; }
        public Util.Point Origine { get; set; }
        public Color Shape_color { get; set; }
        public GeometricTransform Transformation { get; protected set; }
        public BoundingBox ObjectBoundingBox { get; protected set; }
        public BoundingBox WorldBoundingBox { get; protected set; }
        public float Surface { get; protected set; }

        private static int IdCounter = 0;


        public Shape(World world, Util.Point o, Color color)
        {
            Id = ++IdCounter;

            Present_world = world;
            Origine = o;
            Shape_color = color;

            Transformation = new GeometricTransform();
        }


        protected abstract bool Intersects(Ray r, ref float t);
        protected abstract Normal GetNormal(Ray r, float t);

        public Color TraceRay(Ray r)
        {
            float t = 0;

            if (Intersects(r, ref t))
            {
                Normal n = GetNormal(r, t);
                float blendScalair = MathF.Abs(r.Direction * n);

                return Blend(blendScalair);
            }

            return Present_world.Background_color;
        }

        public Color Blend(float scalar)
        {
            int r = (int)Math.Clamp(Shape_color.R * scalar, 0, 255);
            int g = (int)Math.Clamp(Shape_color.G * scalar, 0, 255);
            int b = (int)Math.Clamp(Shape_color.B * scalar, 0, 255);

            return Color.FromArgb(r, g, b);
        }

        public void AddTransformation(params GeometricTransform[] transformations)
        {
            foreach (GeometricTransform gt in transformations)
                Transformation.Multiply(gt);

            WorldBoundingBox.SetPMin(ApplyTransformationOnPoint(ObjectBoundingBox.PMin));
            WorldBoundingBox.SetPMax(ApplyTransformationOnPoint(ObjectBoundingBox.PMax));
        }

        public Vector3D ApplyTransformationOnVector(Vector3D v)
        {
            return new Vector3D(
            Transformation.MatrixP.M11 * v.X + Transformation.MatrixP.M12 * v.Y + Transformation.MatrixP.M13 * v.Z,
            Transformation.MatrixP.M21 * v.X + Transformation.MatrixP.M22 * v.Y + Transformation.MatrixP.M23 * v.Z,
            Transformation.MatrixP.M31 * v.X + Transformation.MatrixP.M32 * v.Y + Transformation.MatrixP.M33 * v.Z);
        }

        public Vector3D ApplyInvTransformationOnVector(Vector3D v)
        {
            return new Vector3D(
            Transformation.InvMatrix.M11 * v.X + Transformation.InvMatrix.M12 * v.Y + Transformation.InvMatrix.M13 * v.Z,
            Transformation.InvMatrix.M21 * v.X + Transformation.InvMatrix.M22 * v.Y + Transformation.InvMatrix.M23 * v.Z,
            Transformation.InvMatrix.M31 * v.X + Transformation.InvMatrix.M32 * v.Y + Transformation.InvMatrix.M33 * v.Z);
        }

        public Util.Point ApplyTransformationOnPoint(Util.Point p)
        {
            return new Util.Point(
            Transformation.MatrixP.M11 * p.X + Transformation.MatrixP.M12 * p.Y + Transformation.MatrixP.M13 * p.Z + Transformation.MatrixP.M14,
            Transformation.MatrixP.M21 * p.X + Transformation.MatrixP.M22 * p.Y + Transformation.MatrixP.M23 * p.Z + Transformation.MatrixP.M24,
            Transformation.MatrixP.M31 * p.X + Transformation.MatrixP.M32 * p.Y + Transformation.MatrixP.M33 * p.Z + Transformation.MatrixP.M34);
        }

        public Util.Point ApplyInvTransformationOnPoint(Util.Point p)
        {
            return new Util.Point(
            Transformation.InvMatrix.M11 * p.X + Transformation.InvMatrix.M12 * p.Y + Transformation.InvMatrix.M13 * p.Z + Transformation.InvMatrix.M14,
            Transformation.InvMatrix.M21 * p.X + Transformation.InvMatrix.M22 * p.Y + Transformation.InvMatrix.M23 * p.Z + Transformation.InvMatrix.M24,
            Transformation.InvMatrix.M31 * p.X + Transformation.InvMatrix.M32 * p.Y + Transformation.InvMatrix.M33 * p.Z + Transformation.InvMatrix.M34);
        }

        public Normal ApplyTransformationOnNormal(Normal n)
        {
            return new Normal(
            Transformation.MatrixP.M11 * n.X + Transformation.MatrixP.M21 * n.Y + Transformation.MatrixP.M31 * n.Z,
            Transformation.MatrixP.M12 * n.X + Transformation.MatrixP.M22 * n.Y + Transformation.MatrixP.M32 * n.Z,
            Transformation.MatrixP.M13 * n.X + Transformation.MatrixP.M23 * n.Y + Transformation.MatrixP.M33 * n.Z);
        }

        public Normal ApplyInvTransformationOnNormal(Normal n)
        {
            return new Normal(
            Transformation.InvMatrix.M11 * n.X + Transformation.InvMatrix.M21 * n.Y + Transformation.InvMatrix.M31 * n.Z,
            Transformation.InvMatrix.M12 * n.X + Transformation.InvMatrix.M22 * n.Y + Transformation.InvMatrix.M32 * n.Z,
            Transformation.InvMatrix.M13 * n.X + Transformation.InvMatrix.M23 * n.Y + Transformation.InvMatrix.M33 * n.Z);
        }

        public Ray ApplyTransformationOnRay(Ray r)
        {
            Ray localRay = new Ray(r);
            localRay.Direction = (ApplyTransformationOnVector(r.Direction));
            localRay.Origin = (ApplyTransformationOnPoint(r.Origin));
            return localRay;
        }

        public Ray ApplyInvTransformationOnRay(Ray r)
        {
            Ray localRay = new Ray(r);
            localRay.Direction = (ApplyInvTransformationOnVector(r.Direction));
            localRay.Origin = (ApplyInvTransformationOnPoint(r.Origin));
            return localRay;
        }

        protected void Swap(ref float a, ref float b)
        {
            float temp = a;
            a = b;
            b = temp;
        }

        public override string ToString()
        {
            return $"   Color: {Shape_color.Name}\n" +
                   $"   Center: {ApplyTransformationOnPoint(Origine)}\n" +
                   $"   Transformation Matrix: {Transformation}\n\n" +
                   $"   Bounding Box: {WorldBoundingBox}\n";
        }
    }
}
