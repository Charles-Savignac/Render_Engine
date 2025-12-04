using Render_Engine.Sampling;

namespace Render_Engine.Cameras
{
    internal class ViewPlan
    {
        public int X_res { get; set; }
        public int Y_res { get; set; }
        public double Pixel_size { get; set; }
        public double Gamma { get; set; }
        public Sampler Sampling { get; set; }

        /// <summary>
        /// Default constructor, sets resolution at 512 by 512, pixels size and gamma to 1.
        /// </summary>
        public ViewPlan()
        {
            X_res = 512;
            Y_res = 512;

            Pixel_size = 1;
            Gamma = 1;

            Sampling = new UniformSampler();
        }

        public ViewPlan(int xres, int yres, Sampler sampler, double pixelsize = 1, double gamma = 1)
        {
            X_res = xres;
            Y_res = yres;

            Pixel_size = pixelsize;
            Gamma = gamma;

            Sampling = sampler;
        }
    }
}
