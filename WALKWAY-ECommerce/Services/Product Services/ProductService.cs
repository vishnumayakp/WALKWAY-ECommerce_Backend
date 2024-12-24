using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.Product_Model.ProductDto;
using WALKWAY_ECommerce.Services.Coudinary_Service;

namespace WALKWAY_ECommerce.Services.ProductServices
{
    public class ProductService: IProductService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<ProductService> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

       public ProductService(ICloudinaryService cloudinaryService, ILogger<ProductService> logger, AppDbContext context, IMapper mapper)
        {
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<GetProductDto>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products.Include(x => x.Category).ToListAsync();
                foreach (var product in products)
                {
                    _logger.LogInformation($"Product: {product.ProductName}, ImageUrls: {product.ImageUrls}");
                }


                if (products.Count > 0)
                {
                    var productWithCategory = products.Select(p => new GetProductDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductBrand = p.ProductBrand,
                        ProductDescription = p.ProductDescription,
                        Sizes=p.Sizes,
                        ProductPrice = p.ProductPrice,
                        Category=p.Category.CategoryName,
                        ImageUrls= p.ImageUrls,
                        MRP = p.MRP,
                        Stock = p.Stock,

                    }).ToList();
                    _logger.LogInformation("Image URLs: {@ImageUrls}", productWithCategory.Select(p => p.ImageUrls));
                    return productWithCategory;

                }

                return new List<GetProductDto>();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<GetProductDto> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(p => p.ProductId == id);
                if(product == null)
                {
                    return null;
                }
                return new GetProductDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductBrand = product.ProductBrand,
                    ProductDescription = product.ProductDescription,
                    Sizes=product.Sizes,
                    ProductPrice = product.ProductPrice,
                    MRP = product.MRP,
                    Category = product.Category.CategoryName,
                    ImageUrls = product.ImageUrls,
                    Stock = product.Stock,
                };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetProductDto>> GetProductByCategory(string category)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category)
                .Where(p => p.Category.CategoryName==category)
                .Select(p => new GetProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductBrand = p.ProductBrand,
                    ProductDescription = p.ProductDescription,
                    Sizes=p.Sizes,
                    ProductPrice = p.ProductPrice,
                    Category = p.Category.CategoryName,
                    ImageUrls = p.ImageUrls,
                    MRP = p.MRP,
                    Stock = p.Stock
                }).ToListAsync();
                return products;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task AddProduct(AddProductDto productDto, List<IFormFile> images)
        {
            try
            {

                
                if (productDto == null)
                {
                    throw new Exception("Product cannot be null");
                }

                if (productDto.ProductPrice > productDto.MRP)
                {
                    throw new Exception("Product price must not be greater than MRP");
                }

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == productDto.CategoryId);

                if (category == null)
                {
                    throw new Exception("Category with this Id does not Exist");
                    _logger.LogInformation("Uploading image at {Time}", DateTime.UtcNow);
                }

                var validSizes = new List<int> { 16,17,18,19,20,21,22,23,34,35,36,27,28,29,30,38,40,42,44,46,48 };

                if(productDto.Sizes==null || productDto.Sizes.Count == 0)
                {
                    throw new Exception("At least one Size must be selected.");
                }

                foreach (var size in productDto.Sizes)
                {
                    if (!validSizes.Contains(size))
                    {
                        throw new ArgumentException($"Invalid size selected: {size}");
                    }
                }

                if (images==null || images.Count == 0)
                {
                    throw new Exception("No images Provided For Upload");
                }

                var imageUrls = await _cloudinaryService.uploadImageAsync(images);
                

                if (imageUrls==null || imageUrls.Count==0)
                {
                    throw new Exception("Image Upload failed");
                }
                _logger.LogInformation("Image upload completed at {Time}, URL: {ImageUrl}", DateTime.UtcNow, string.Join(", ", imageUrls));


                _logger.LogInformation("Inserting product into database at {Time}", DateTime.UtcNow);
                var product = _mapper.Map<Product>(productDto);
                product.ImageUrls = imageUrls;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product added to database at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while adding the product.");
            }
        }
      public async Task UpdateProduct(int productId, AddProductDto productDto, List<IFormFile> images)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == productDto.CategoryId);

                if (categoryExist == null)
                {
                    throw new Exception("Category Not Exist");
                }

                if (product != null)
                {
                    product.ProductName = productDto.ProductName;
                    product.ProductBrand = productDto.ProductBrand;
                    product.ProductDescription = productDto.ProductDescription;
                    product.Sizes = productDto.Sizes;
                    product.ProductPrice = productDto.ProductPrice;
                    product.CategoryId = productDto.CategoryId;
                    product.MRP = productDto.MRP;
                    product.Stock = productDto.Stock;

                    if(images!=null && images.Count > 0)
                    {
                        var imageUrls = await _cloudinaryService.uploadImageAsync(images);
                        product.ImageUrls = imageUrls;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Product with id {productId} Not found");
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (product == null)
                {
                    //return false;
                    throw new Exception($"Product with id {productId} Not Found");
                }

                 _context.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    public async Task<List<GetProductDto>>  SearchProduct(string searchword)
        {
            if (string.IsNullOrEmpty(searchword))
            {
                return new List<GetProductDto>();
            }

            var product= await _context.Products.Include(x=>x.Category)
                .Where(x=>x.ProductName.ToLower().Contains(searchword.ToLower()))
                .ToListAsync();

            return product.Select(x=> new GetProductDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductBrand = x.ProductBrand,
                ProductDescription = x.ProductDescription,
                Sizes=x.Sizes,
                ProductPrice = x.ProductPrice,
                Category = x.Category.CategoryName,
                ImageUrls = x.ImageUrls,
                MRP = x.MRP,
                Stock = x.Stock
            }).ToList();
        }

        public async Task<List<GetProductDto>>  ProductPaginated(int page = 1, int pageSize = 10)
        {
            try
            {
                var product = await _context.Products
                    .Include(x => x.Category)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return product.Select(x => new GetProductDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductBrand=x.ProductBrand,
                    ProductDescription = x.ProductDescription,
                    Sizes=x.Sizes,
                    ProductPrice = x.ProductPrice,
                    Category = x.Category.CategoryName,
                    ImageUrls = x.ImageUrls,
                    MRP = x.MRP,
                    Stock = x.Stock
                }).ToList();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
