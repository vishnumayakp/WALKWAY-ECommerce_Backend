using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.WishList_Model;
using WALKWAY_ECommerce.Models.WishList_Model.WishListDto;

namespace WALKWAY_ECommerce.Services.WishList_Services
{
    public class WishListService:IWishListService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public WishListService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<string> AddOrRemove(int userId, int productId)
        {
            try
            {
                var productExist = await _appDbContext.Products.AnyAsync(p => p.ProductId == productId);

                if (!productExist)
                {
                    return "Product Deos Not Exist";
                }

                var existWishList = await _appDbContext.WishLists.Include(x=>x.Product).FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);

                if(existWishList == null)
                {
                    WishListDto newWishListDto = new WishListDto
                    {
                        UserId = userId,
                        ProductId = productId,
                    };
                    var newWishList = _mapper.Map<WishList>(newWishListDto);
                    _appDbContext.WishLists.Add(newWishList);
                    await _appDbContext.SaveChangesAsync();

                    return "Item Added to WishLists";
                }

                _appDbContext.Remove(existWishList);
                await _appDbContext.SaveChangesAsync();
                return "Item Removed from WishLists";
            }catch (Exception ex)
            {
                throw new Exception($"An Error Occured : {ex.Message}");
            }
        }

        public async Task<List<WishListResponseDto>> GetWishLists(int userId)
        {
            try
            {
                var wishListItems = await _appDbContext.WishLists.Include(w => w.Product).ThenInclude(c => c.Category)
                    .Where(x => x.UserId == userId).ToListAsync();
                if (wishListItems != null)
                {
                    var wishListProducts = wishListItems.Select(item => new WishListResponseDto
                    {
                        Id = item.ProductId,
                        ProductId = item.ProductId,
                        ProductBrand=item.Product.ProductBrand,
                        ProductName = item.Product.ProductName,
                        ProductDescription = item.Product.ProductDescription,
                        Image = item.Product.ImageUrls.FirstOrDefault(),
                        Price = item.Product.ProductPrice,
                        Category = item.Product.Category.CategoryName
                    }).ToList();

                    return wishListProducts;
                }
                else
                {
                    return new List<WishListResponseDto>();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
