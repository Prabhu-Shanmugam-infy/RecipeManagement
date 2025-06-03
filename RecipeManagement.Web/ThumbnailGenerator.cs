using System.Drawing;
using System.IO;

namespace RecipeManagement.Web
{  

    public class ThumbnailGenerator
    {
        public static void GenerateThumbnail(string sourceImagePath, int targetWidth, int targetHeight, string thumbnailPath)
        {
            try
            {
                using (Image originalImage = Image.FromFile(sourceImagePath))
                {
                    using (Bitmap thumbnailImage = new Bitmap(targetWidth, targetHeight))
                    using (Graphics g = Graphics.FromImage(thumbnailImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; // Improved quality
                        g.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
                        thumbnailImage.Save(thumbnailPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
