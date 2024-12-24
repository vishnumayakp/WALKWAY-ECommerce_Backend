using WALKWAY_ECommerce.Models.WishList_Model.WishListDto;

namespace WALKWAY_ECommerce.Services.WishList_Services
{
    public interface IWishListService
    {
        Task<string> AddOrRemove(int userId, int productId);

        Task<List<WishListResponseDto>> GetWishLists(int userId);
    }
}
