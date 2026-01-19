using Render_Engine.Acceleration;
using Render_Engine.Cameras;
using Render_Engine.Illumination;
using Render_Engine.Materials;
using Render_Engine.Sampling;
using Render_Engine.Shapes;
using Render_Engine.Skybox;
using Render_Engine.Textures;
using Render_Engine.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;

using GT = Render_Engine.Util.GeometricTransform;


namespace Render_Engine
{
    internal class World
    {
        public Color Background_color { get; private set; }
        public Accelerator AccelerationTec { get; private set; }
        public ViewPlan View_plan { get; private set; }
        public RayTracer Tracer { get; private set; }
        public EnvironmentMap Skybox { get; private set; }
        public Camera CameraType { get; private set; }

        public World() { }

        public void Build()
        {
            View_plan = new ViewPlan(1080, 720, new UniformSampler(1));
            Tracer = new WhittedRayTracer(this);
            CameraType = new Orthographic();

            CreateShapes();
            CreateSkyBox("none");
            CreateLightSources();

            AccelerationTec = new GridAccelerator(Tracer.Shapes);
        }

        private void CreateSkyBox(string type = "none", string filename = "default.png")
        {
            Background_color = Color.Black;
            switch (type)
            {
                case "sphere":
                    Skybox = new SphericalEnvMap(new Bitmap($"Textures/{filename}"));
                    break;

                case "cube":
                    Bitmap map = new Bitmap($"Textures/{filename}");


                    var tiles = ImageTiler.Slice(map, 3, 4);

                    Bitmap left = tiles[4];
                    Bitmap front = tiles[5];
                    Bitmap right = tiles[6];
                    Bitmap back = tiles[7];
                    Bitmap top = tiles[1];
                    Bitmap bottom = tiles[9];

                    Skybox = new CubeEnvMap
                    {
                        PosX = left,
                        NegX = right,
                        PosY = top,
                        NegY = bottom,
                        PosZ = front,
                        NegZ = back,
                    };
                    break;

                case "none":
                default:
                    Skybox = null;
                    break;
            }
        }

        private void CreateShapes()
        {
            Point3D origin = new Point3D();
            Phong phong = new Phong(Color.Red);
            Matte matte = new Matte(Color.Blue);

            Point3D p1 = new Point3D(-100, -100, 0);
            Point3D p2 = new Point3D(100, -100, 0);
            Point3D p3 = new Point3D(0, 100, 0);

            //fucked
            Cube cu1 = new Cube(new Point3D(), phong, 50);
            Triangle tr1 = new Triangle(origin, phong, p1, p2, p3);

            //good
            Cylinder cy1 = new Cylinder(origin, phong, 150, -200, 200);
            Plan plan = new Plan(origin, matte, 250, 250);
            Disk disk = new Disk(origin, matte, 50, 300);
            Cone co1 = new Cone(origin, matte, 150, 300);
            Sphere sp1 = new Sphere(origin, matte, 250);

            Tracer.AddShapes(tr1);  
        }

        public void CreateLightSources()
        {
            PointLight l1 = new PointLight(new Point3D(0, 0, 500), Color.White);

            Tracer.AddLightSource(l1);
        }

        public Bitmap RenderScene()
        {
            Bitmap bitmap = new Bitmap(View_plan.X_res, View_plan.Y_res);
            bitmap = CameraType.RenderScene(this, bitmap);

            return bitmap;
        }
    }
}
