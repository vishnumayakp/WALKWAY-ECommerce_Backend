using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.Order_Model.OrderDto;

namespace WALKWAY_ECommerce.Services.Order_Service
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(PaymentDto payment);
        Task<bool> CreateOrder(int userId, CreateOrderDto createOrderDTO);
        Task<List<ViewUserOrderDetailsDto>> GetOrderDetails(int userId);
        Task<List<ViewOrderAdminDto>> GetTotalOrders();
        Task<List<ViewUserOrderDetailsDto>> GetOrdersByUserId(int userId);
        Task<Decimal> GetTotalRevenue();
        Task<int> GetTotalProductPurchased();
        Task UpdateOrder(string orderId, string orderStatus);
    }
}
