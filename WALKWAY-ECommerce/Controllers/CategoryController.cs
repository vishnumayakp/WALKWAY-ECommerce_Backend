using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Services.CategoryServices;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategory")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categoryList = await _categoryService.GetAllCategories();

                return Ok(new ApiResponses<List<CategoryDto>>(200, "Succefully got AllCategories", categoryList));
            }catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"Internal server Error",null,ex.Message));
            }
        }

        [HttpGet("GetCategorybyName/{name}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try
            {
                var category = await _categoryService.GetCategoryByName(name);

                if(category == null)
                {
                    return NotFound(new ApiResponses<string>(404,"Not Found" ,$"Category {name} NotFound"));
                }

                return Ok(new ApiResponses<CategoryDto>(200, "categoty Successfully Fetched ",category));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> Addcategory([FromForm] CategoryDto category, IFormFile image)
        {
            try
            {
                var res = await _categoryService.AddCategory(category);

                if (res)
                {
                    return Ok(new ApiResponses<bool>(200, "Successfully Added Category",res));
                }
                return Conflict(new ApiResponses<string>(409, "category Already Exist"));
            }catch( Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }

        [Authorize (Roles ="admin")]
        [HttpDelete("DeleteCategory/{id}")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var res = await _categoryService.RemoveCategory(id);

                if (res)
                {
                    return Ok(new ApiResponses<bool>(200, "Item deleted form Category"));
                }

                return NotFound(new ApiResponses<string>(404, "Category NotFound"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }
    }
}
