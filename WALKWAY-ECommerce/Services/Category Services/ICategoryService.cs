using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Models.Category_Model.CategoryDto;

namespace WALKWAY_ECommerce.Services.CategoryServices
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllCategories();
        public Task<CategoryDto> GetCategoryByName(string name);
        public Task<bool> AddCategory(CategoryDto newCategory);
        public Task<bool> RemoveCategory(int id);
    }
}
