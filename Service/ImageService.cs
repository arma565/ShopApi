using Microsoft.IdentityModel.Tokens;

namespace Shop.Service
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile image);
        FileStream ReadImage(string filePath);
        bool IsValidImage(IFormFile image);
    }

    public class ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor) : IImageService
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        /// <summary>
        /// Use this function to upload profile image to server
        /// </summary>
        /// <param name="image">
        /// image to upload
        /// </param>
        /// <returns></returns>
        public async Task<string> UploadProfileImage(IFormFile image)
        {
            if (image is null)
                return "";

            if (!IsValidImage(image))
                throw new InvalidOperationException("Invalid image file.");

            var webRootPath = _environment.WebRootPath;

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

            if (!Directory.Exists(webRootPath))
                Directory.CreateDirectory(webRootPath); // Recreate wwwroot

            var uploadsFolder = Path.Combine(webRootPath, "images/auth");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream).ConfigureAwait(false);
            }

            return fileName;
        }

        /// <summary>
        /// Use this function to upload profile image to server
        /// </summary>
        /// <param name="image">
        /// image to upload
        /// </param>
        /// <returns></returns>
        public async Task<string> UploadImage(IFormFile image)
        {
            if (!IsValidImage(image))
            {
                throw new InvalidOperationException("Invalid image file.");
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return GetFileUrl(fileName);
        }

        public string GetFullImagePath(string userProfileImageName)
        {

            var environmentPath = GetLocalImagesFullPath("auth");

            // Normalize and sanitize the path
            var fileName = Path.GetFileName(userProfileImageName); // strips any path traversal

            var fullPath = Path.Combine(environmentPath, fileName);

            if (!File.Exists(fullPath))
                return "";

            return fullPath;
        }

        /// <summary>
        /// Use this to download image from server
        /// </summary>
        /// <param name="filePath">
        /// file path of image file
        /// </param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public FileStream ReadImage(string filePath)
        {
            try
            {
                return new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    useAsync: true
                );
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading the file. Error =" + ex.Message);
            }
        }

        /// <summary>
        /// Use this to download image from server
        /// </summary>
        /// <param name="filePath">
        /// file path of image file
        /// </param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public FileStream ReadProfileImage(string filePath)
        {
            try
            {
                return new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    useAsync: true
                );
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading the file. Error =" + ex.Message);
            }
        }

        public bool IsValidImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return false;
            }

            if (image.Length > MaxFileSize)
            {
                return false; // File is too large
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName)?.ToLowerInvariant();

            if (!validExtensions.Contains(extension))
            {
                return false; // Invalid file type
            }

            return true;
        }

        private string GetFileUrl(string fileName)
        {
            var connection = _httpContextAccessor.HttpContext?.Connection;
            if (connection != null)
            {
                var localIpAddress = connection.LocalIpAddress?.ToString();

                if (localIpAddress == "127.0.0.1" || localIpAddress == "::1")
                {
                    return $"http://localhost:5068/images/{fileName}";
                }
                else
                {
                    return $"http://{connection.RemoteIpAddress}:5068/images/{fileName}";
                }
            }
            else
            {
                return $"http://localhost:5068/images/{fileName}";
            }
        }

        public string GetLocalImagesFullPath(string requestedModelPath)
        {

            if (requestedModelPath.IsNullOrEmpty())
                return "";

            var webRootPath = _environment.WebRootPath;

            if (!Directory.Exists(webRootPath))
                return "";

            if (requestedModelPath == "asset")
                return Path.Combine(webRootPath, "images\\asset");
            else
                return Path.Combine(webRootPath, "images\\auth");
        }
    }
}