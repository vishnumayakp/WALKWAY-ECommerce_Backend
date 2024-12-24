using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.Order_Model.OrderDto;

namespace WALKWAY_ECommerce.Services.Order_Service
{
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public OrderService(AppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<string> RazorOrderCreate(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TransactionId = random.Next(0, 1000).ToString();

            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency","INR");
            input.Add("receipt", TransactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);
            Razorpay.Api.Order order = client.Order.Create(input);

            var OrderId = order["id"].ToString();
            return OrderId;
        }

        public bool RazorPayment(PaymentDto payment)
        {
            if (payment == null ||
                string.IsNullOrEmpty(payment.razorpay_payment_id) ||
                string.IsNullOrEmpty(payment.razorpay_orderId) ||
                string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return false;
            }

            try
            {
                RazorpayClient client = new RazorpayClient(
                    _configuration["Razorpay:KeyId"],
                    _configuration["Razorpay:KeySecret"]
                    );
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    {"razorpay_payment_id",payment.razorpay_payment_id },
                    {"razorpay_orderId",payment.razorpay_orderId },
                    {"razorpay_signature",payment.razorpay_signature }
                };
                Utils.verifyPaymentSignature(attributes);
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateOrder(int userId, CreateOrderDto createOrderDTO)
        {
            try
            {
                var cart = await _appDbContext.Carts.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.UserId == userId);

                if (cart == null)
                {
                    throw new Exception("Cart Not Found for the User");
                }

                if (createOrderDTO.Totalamount <= 0)
                {
                    throw new Exception("Total Amount must be greater than Zero");
                }
                var order = new OrderMain
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatus = "pending",
                    AddressId = createOrderDTO.AddressId,
                    TotalAmount = createOrderDTO.Totalamount,
                    OrderString = createOrderDTO.OrderString,
                    TransactionId = createOrderDTO.TransactionId,
                    OrderItems = cart.CartItems.Select(c => new OrderItem
                    {
                        productId = c.ProductId,
                        Quantity = c.Quantity,
                        TotalPrice = c.Quantity * c.Product.ProductPrice,
                    }).ToList()
                };

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);

                    if (product != null)
                        if (product.Stock < cartItem.Quantity)
                        {
                            return false;
                        }
                    product.Stock -= cartItem.Quantity;
                }

                await _appDbContext.Orders.AddAsync(order);
                _appDbContext.Remove(cart);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ViewUserOrderDetailsDto>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _appDbContext.Orders.Include(o => o.OrderItems)
                    .ThenInclude(p => p.Product).Where(u => u.UserId == userId).ToListAsync();

                if(orders==null || !orders.Any())
                {
                    return new List<ViewUserOrderDetailsDto>();
                }

                var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto{
                    OrderId = o.OrderString,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    TransactionId = o.TransactionId,
                    OrderProducts = o.OrderItems.Select(i => new CartItemDto
                    {
                        ProductId = i.productId,
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.Product.ProductPrice,
                        TotalPrice = i.TotalPrice,
                        Image = i.Product.ImageUrls.FirstOrDefault()
                    }).ToList(),
                }).ToList();

                return orderDetails;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ViewOrderAdminDto>> GetTotalOrders()
        {
            try
            {
                var totalOrders = await _appDbContext.Orders.Include(u => u.User)
                    .Include(oi => oi.OrderItems)
                    .Select(g => new ViewOrderAdminDto
                    {
                        UserId=g.UserId,
                        OrderDate = g.OrderDate,
                        orderId = g.OrderString,
                        CustomerName = g.User.UserName,
                        CustomerEmail = g.User.Email,
                        OrderStatus = g.OrderStatus,
                        TransactionId = g.TransactionId
                    }).ToListAsync();
                return totalOrders;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ViewUserOrderDetailsDto>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _appDbContext.Orders.Include(oi => oi.OrderItems).ThenInclude(p => p.Product).Where(u => u.UserId == userId).ToListAsync();
                if(orders==null || !orders.Any())
                {
                    return new List<ViewUserOrderDetailsDto>();
                }

                var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto
                {
                    OrderId = o.OrderString,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    TransactionId = o.TransactionId,
                    OrderProducts = o.OrderItems.Select(i => new CartItemDto
                    {
                        ProductId = i.productId,
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.Product.ProductPrice,
                        TotalPrice = i.TotalPrice,
                        Image = i.Product.ImageUrls.FirstOrDefault()
                    }).ToList(),
                }).ToList();

                return orderDetails;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetTotalRevenue()
        {
            try
            {
                var Total = await _appDbContext.OrderItems.SumAsync(p => p.TotalPrice);
                return Total;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetTotalProductPurchased()
        {
            try
            {
                var TotalProducts = await _appDbContext.OrderItems.SumAsync(p => p.Quantity);
                return TotalProducts;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrder(string orderId, string orderStatus)
        {
            try
            {
                var order = await _appDbContext.Orders.FirstOrDefaultAsync(o => o.OrderString == orderId);
                if (order != null)
                {
                    order.OrderStatus = orderStatus;
                    await _appDbContext.SaveChangesAsync();
                }
                throw new Exception("Order with this OrderId Not Found");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
