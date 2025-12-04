using System.Numerics;

namespace Render_Engine.Util
{
    internal class GeometricTransform
    {
        public Matrix4x4 MatrixP { get; private set; }
        public Matrix4x4 InvMatrix { get; private set; }

        public GeometricTransform()
        {
            MatrixP = new Matrix4x4(1, 0, 0, 0,
                                    0, 1, 0, 0,
                                    0, 0, 1, 0,
                                    0, 0, 0, 1);
            InvMatrix = MatrixP;
        }

        public GeometricTransform(GeometricTransform gt)
        {
            MatrixP = gt.MatrixP;
            InvMatrix = gt.InvMatrix;
        }

        public GeometricTransform(Matrix4x4 aM)
        {
            MatrixP = aM;
            if (!Matrix4x4.Invert(MatrixP, out Matrix4x4 result))
            {
                throw new InvalidOperationException("Matrix is non-invertible.");
            }
            InvMatrix = result;
        }

        public GeometricTransform(Matrix4x4 aM, Matrix4x4 iAM)
        {
            MatrixP = aM;
            InvMatrix = iAM;
        }

        public bool IsIdentity() => MatrixP.IsIdentity;

        public void Multiply(GeometricTransform gt)
        {
            MatrixP = MatrixP * gt.MatrixP;
            InvMatrix = InvMatrix * gt.InvMatrix;
        }

        public static GeometricTransform Translate(double x = 0, double y = 0, double z = 0) => Translate(new Vector3D(x, y, z));

        public static GeometricTransform Translate(Vector3D v)
        {
            Matrix4x4 m = new Matrix4x4(1, 0, 0, (float)v.X,
                                        0, 1, 0, (float)v.Y,
                                        0, 0, 1, (float)v.Z,
                                        0, 0, 0, 1);

            Matrix4x4 Im = new Matrix4x4(1, 0, 0, (float)-v.X,
                                         0, 1, 0, (float)-v.Y,
                                         0, 0, 1, (float)-v.Z,
                                         0, 0, 0, 1);

            return new GeometricTransform(m, Im);
        }

        public static GeometricTransform Scale(double x = 1, double y = 1, double z = 1) => Scale(new Vector3D(x, y, z));

        public static GeometricTransform Scale(Vector3D v)
        {
            Matrix4x4 m = new Matrix4x4((float)v.X, 0, 0, 0,
                                        0, (float)v.Y, 0, 0,
                                        0, 0, (float)v.Z, 0,
                                        0, 0, 0, 1);

            Matrix4x4 Im = new Matrix4x4(1 / (float)v.X, 0, 0, 0,
                                         0, 1 / (float)v.Y, 0, 0,
                                         0, 0, 1 / (float)v.Z, 0,
                                         0, 0, 0, 1);

            return new GeometricTransform(m, Im);
        }

        public static GeometricTransform RotateX(double x)
        {
            double radX = Math.PI / 180 * x;

            Matrix4x4 m = new Matrix4x4(1, 0, 0, 0,
                                           0, (float)Math.Cos(radX), (float)-Math.Sin(radX), 0,
                                           0, (float)Math.Sin(radX), (float)Math.Cos(radX), 0,
                                           0, 0, 0, 1);

            Matrix4x4 Im = Matrix4x4.Transpose(m);

            return new GeometricTransform(m, Im);
        }

        public static GeometricTransform RotateY(double y)
        {
            double radY = Math.PI / 180 * y;

            Matrix4x4 m = new Matrix4x4((float)Math.Cos(radY), 0, (float)Math.Sin(radY), 0,
                                           0, 1, 0, 0,
                                           -(float)Math.Sin(radY), 0, (float)Math.Cos(radY), 0,
                                           0, 0, 0, 1);

            Matrix4x4 Im = Matrix4x4.Transpose(m);

            return new GeometricTransform(m, Im);
        }

        public static GeometricTransform RotateZ(double z)
        {
            double radZ = Math.PI / 180 * z;

            Matrix4x4 m = new Matrix4x4((float)Math.Cos(radZ), -(float)Math.Sin(radZ), 0, 0,
                                           (float)Math.Sin(radZ), (float)Math.Cos(radZ), 0, 0,
                                           0, 0, 1, 0,
                                           0, 0, 0, 1);

            Matrix4x4 Im = Matrix4x4.Transpose(m);

            return new GeometricTransform(m, Im);
        }

        public override string ToString()
        {
            string format = "{0,10:F2}";
            return $"\n" +
                   string.Format(format, MatrixP.M11) + string.Format(format, MatrixP.M12) + string.Format(format, MatrixP.M13) + string.Format(format, MatrixP.M14) + "\n" +
                   string.Format(format, MatrixP.M21) + string.Format(format, MatrixP.M22) + string.Format(format, MatrixP.M23) + string.Format(format, MatrixP.M24) + "\n" +
                   string.Format(format, MatrixP.M31) + string.Format(format, MatrixP.M32) + string.Format(format, MatrixP.M33) + string.Format(format, MatrixP.M34) + "\n" +
                   string.Format(format, MatrixP.M41) + string.Format(format, MatrixP.M42) + string.Format(format, MatrixP.M43) + string.Format(format, MatrixP.M44);
        }

    }
}
