namespace WALKWAY_ECommerce.Services.Coudinary_Service
{
    public interface ICloudinaryService
    {
        Task<List<String>> uploadImageAsync(List<IFormFile> files);
    }
}
