using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.Product_Model.ProductDto;
using WALKWAY_ECommerce.Services.ProductServices;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("All")]

        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(new ApiResponses<List<GetProductDto>>(200, "Products fetched Sucessfully", products));

            }catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"Internal server Error",ex.Message));
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if(product == null)
                {
                    return NotFound(new ApiResponses<string>(404, $"product with {id} id not found"));
                }

                return Ok(new ApiResponses<GetProductDto>(200, "Product Successfully Fetched", product));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }

        [HttpGet("GetByCategory")]

        public async Task<IActionResult> GetProductByCategory(string category)
        {
            try
            {
                var products = await _productService.GetProductByCategory(category);

                if (products.Count == 0)
                {
                    return NotFound(new ApiResponses<string>(404, $"product with {category} not found"));
                }
                return Ok(new ApiResponses<List<GetProductDto>>(200, "Successfully fetched the Product", products));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost("Add")]

        public async Task<IActionResult> AddProducts([FromForm] AddProductDto productDto, List<IFormFile> images)
        {
            try
            {
                await _productService.AddProduct(productDto, images);
                return Ok(new ApiResponses<string>(200, "Successfully Added Product"));

            }catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error","data failed to upload"));
            }
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpdateProductById(int productId,[FromForm] AddProductDto productDto,List<IFormFile> images)
        {
            try
            {
               await _productService.UpdateProduct(productId, productDto, images);
                return Ok(new ApiResponses<string>(200, "Product Updated Successfully"));
                
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }

        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var res=await _productService.RemoveProduct(productId);
                if (res == null)
                {
                    return Ok(new ApiResponses<string>(200, $"Product with ProductId {productId} not exist"));

                }
                return Ok(new ApiResponses<string>(200, "Successfully Removed Product"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }

        [HttpGet("search")]

        public async Task<IActionResult> SearchProduct(string searchword)
        {
            try
            {
                var products = await _productService.SearchProduct(searchword);
                return Ok(products);
            }catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }

        [HttpGet("pagination")]

        public async Task<IActionResult> PaginatedProducts([FromQuery] int pageNumber = 1, [FromQuery] int size = 10)
        {
            try
            {
                var products = await _productService.ProductPaginated(pageNumber, size);
                return Ok(products);
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }
    }
}