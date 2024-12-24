using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Models.Cart_Model;

namespace WALKWAY_ECommerce.Services.Cart_Services
{
    public interface ICartService
    {
        Task<CartResDto> GetCartItems(int userId);
        Task<ApiResponses<CartResDto>> AddToCart(int userId, int productId);
        Task<bool> RemoveFromCart(int userId, int productId);
        Task<ApiResponses<CartItem>> IncreaseQty(int userId, int productId);
        Task<bool> DecreaseQty(int userId, int productId);
        Task<bool> RemoveAllItems(int userId);
    }
}
