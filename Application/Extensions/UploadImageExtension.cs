using Application.Utils;
using FileSignatures;
using FileSignatures.Formats;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class UploadImageExtension
    {
        public static bool AddImageToServer(this IFormFile image, string fileName, string originalPath, int? width, int? height, string thumbPath = null, string deleteFileName = null)
        {
            if (image == null)
                return false;

            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(image.OpenReadStream());
            if (format is not Image)
                return false;

            if (!Directory.Exists(originalPath))
                Directory.CreateDirectory(originalPath);

            if (!string.IsNullOrEmpty(deleteFileName))
                DeleteImage(deleteFileName, originalPath, thumbPath);

            var filePath = Path.Combine(originalPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            if (!string.IsNullOrEmpty(thumbPath))
            {
                if (!Directory.Exists(thumbPath))
                    Directory.CreateDirectory(thumbPath);

                var resizer = new ImageOptimizer();
                if (width != null && height != null)
                    resizer.ImageResizer(filePath, Path.Combine(thumbPath, fileName), width, height);
            }

            return true;
        }

        public static void DeleteImage(this string imageName, string OriginPath, string ThumbPath = null)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                if (File.Exists(OriginPath + imageName))
                    File.Delete(OriginPath + imageName);

                if (!string.IsNullOrEmpty(ThumbPath))
                {
                    if (File.Exists(ThumbPath + imageName))
                        File.Delete(ThumbPath + imageName);
                }
            }
        }
    }
}
