using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.Order_Model.OrderDto;
using WALKWAY_ECommerce.Services.Order_Service;
using WALKWAY_ECommerce.Services.User_Service;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService,IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost("CreateOrder")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateOrder(long price)
        {
            try
            {
                if (price <= 0)
                {
                    //throw new Exception("Enter Valid Price");
                    return BadRequest(new ApiResponses<string>(400, "Enter Valid Price"));
                }

                var OrderId = await _orderService.RazorOrderCreate(price);
                return Ok(OrderId);
            }
            catch(DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payment")]
        [Authorize(Roles = "user")]

        public  IActionResult PaymentOrder(PaymentDto paymentDto)
        {
            try
            {
                if (paymentDto == null)
                {
                    return BadRequest("Payment details Cannot be null here");
                }
                var res =  _orderService.RazorPayment(paymentDto);
                return Ok(res);
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("placeOrder")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var userId = _userService.GetUserId();
                var res = await _orderService.CreateOrder(userId, createOrderDto);
                return Ok(res);
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);  
            }
        }

        [HttpGet("getOrderDetails")]
        [Authorize(Roles ="user")]

        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                int userId= _userService.GetUserId();
                var res = await _orderService.GetOrderDetails(userId);
                return Ok(new ApiResponses<List<ViewUserOrderDetailsDto>>(200, " successfully fetched order details", res, null));
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("getAllOrders")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> GetAllOrdersAdmin()
        {
            try
            {
                var res = await _orderService.GetTotalOrders();
                if (res != null)
                {
                    return Ok(new ApiResponses<List<ViewOrderAdminDto>>(200, "Successfully fetched All Orders", res));
                }
                return Ok(new ApiResponses<List<ViewOrderAdminDto>>(200, "Orders are Empty", res));

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetOrderByUserId/{userId}")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> GetOrderByUserId(int userId)
        {
            try
            {
                var res = await _orderService.GetOrdersByUserId(userId);
                if(res == null)
                {
                    return NotFound(new ApiResponses<List<ViewUserOrderDetailsDto>>(404, "There Is No Order With this User",res));
                }
                return Ok(new ApiResponses<List<ViewUserOrderDetailsDto>>(200, " fetched All Orders", res));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("TotalRevenue")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> GetRevenue()
        {
            try
            {
                var revenue = await _orderService.GetTotalRevenue();
                return Ok(revenue);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("TotalProductsPurchased")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> GetPurchasedProducts()
        {
            try
            {
                var res = await _orderService.GetTotalProductPurchased();
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("update-OrderStatus")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> UpdateOrderStatus(string orderId, string orderStatus)
        {
            try
            {
                await _orderService.UpdateOrder(orderId,orderStatus);
                return Ok(new ApiResponses<string>(200, "Order Status Updated Successfully"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
    }
}
