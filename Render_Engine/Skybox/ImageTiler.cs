using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render_Engine.Textures
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class ImageTiler
    {
        public static List<Bitmap> Slice(Bitmap source, int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Width % columns != 0 || source.Height % rows != 0)
                throw new ArgumentException("Image dimensions must be divisible by the number of rows and columns.");

            int tileWidth = source.Width / columns;
            int tileHeight = source.Height / rows;

            var result = new List<Bitmap>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Rectangle rect = new Rectangle(
                        c * tileWidth,
                        r * tileHeight,
                        tileWidth,
                        tileHeight);

                    Bitmap tile = new Bitmap(tileWidth, tileHeight);

                    using (Graphics g = Graphics.FromImage(tile))
                    {
                        g.DrawImage(source, new Rectangle(0, 0, tileWidth, tileHeight), rect, GraphicsUnit.Pixel);
                    }

                    result.Add(tile);
                }
            }

            return result;
        }
    }

}
