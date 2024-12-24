using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.Product_Model.ProductDto;

namespace WALKWAY_ECommerce.Services.ProductServices
{
    public interface IProductService
    {
        Task AddProduct(AddProductDto productDto, List<IFormFile> images);
        Task UpdateProduct(int productId, AddProductDto productDto, List<IFormFile> images);
        Task<bool> RemoveProduct(int productId);
       Task<List<GetProductDto>> GetAllProducts();

       Task<GetProductDto> GetProductById(int id);
       Task<List<GetProductDto>> GetProductByCategory(string categoryName);

       Task<List<GetProductDto>> SearchProduct(string search);
       Task<List<GetProductDto>> ProductPaginated( int page=1, int pageSize=10);

    }
}
