using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace WALKWAY_ECommerce.Services.Coudinary_Service
{
    public class CloudinaryService:ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;


        public CloudinaryService(IConfiguration configuration,  ILogger<CloudinaryService> logger)
        {
            _logger = logger;
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];


            if(string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("Cloudinary configuration Missing or Incomplete");
            }

            var account= new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<List<String>> uploadImageAsync(List<IFormFile> files)
        {
            if(files == null || files.Count==0)
            {
                //return null;
                throw new Exception("File Is Null or Empty");
               
            }

            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);


                        if (uploadResult.Error != null)
                        {
                            _logger.LogError($"Cloudinary upload error: {uploadResult.Error.Message}");
                            throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
                        }

                        _logger.LogInformation($"Cloudinary upload successful. URL: {uploadResult.SecureUrl}");
                        imageUrls.Add(uploadResult.SecureUrl?.ToString());
                    }

                }
            }
            return imageUrls;
        }
    }
}