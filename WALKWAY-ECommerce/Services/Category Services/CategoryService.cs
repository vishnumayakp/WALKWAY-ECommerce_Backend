using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Services.Coudinary_Service;

namespace WALKWAY_ECommerce.Services.CategoryServices
{
    public class CategoryService:ICategoryService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;


        public CategoryService(ICloudinaryService cloudinaryService,AppDbContext appDbContext, IMapper mapper, ILogger<CategoryService> logger)
        {
            _cloudinaryService = cloudinaryService;
            _context = appDbContext;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return _mapper.Map<List<CategoryDto>>(categories);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoryDto> GetCategoryByName(string name)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
                if (category == null)
                {
                    return null;
                }

                return _mapper.Map<CategoryDto>(category);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                var isExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDto.CategoryName);

                if (isExist != null)
                {
                    _logger.LogInformation($"User {categoryDto.CategoryName} Already Exist");
                    return false;
                }


                var add = _mapper.Map<Category>(categoryDto);

                await _context.AddAsync(add);
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveCategory(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c=>c.CategoryId==id);
                if (category == null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(category);
                    await _context.SaveChangesAsync();
                    return true;

                }
                
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }




        }


    }
}
