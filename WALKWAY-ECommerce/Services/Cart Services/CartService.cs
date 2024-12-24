using Microsoft.EntityFrameworkCore;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Models.Product_Model;

namespace WALKWAY_ECommerce.Services.Cart_Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartResDto> GetCartItems(int userId)
        {
            try
            {
                var cart = await _context.Carts.Include(ci => ci.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.UserId == userId);

                if (cart == null)
                {
                    throw new Exception("Cart Not Found");
                }

                decimal cartTotal = cart.CartItems.Sum(items => items.Quantity * items.Product.ProductPrice);

                var cartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.ProductPrice,
                    TotalPrice = item.Quantity * item.Product.ProductPrice,
                    Image = item.Product.ImageUrls.FirstOrDefault()
                }).ToList();
                var TotalItems = cartItems.Count();
                var cartRespponse = new CartResDto
                {
                    TotalItems = TotalItems,
                    GrandTotal = cartTotal,
                    CartItems = cartItems,

                };

                return cartRespponse;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponses<CartResDto>> AddToCart(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new ApiResponses<CartResDto>(404, "User Not found", null);
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null)
                {
                    return new ApiResponses<CartResDto>(404, $"Product with Id {productId} Not Found", null);
                }

                if (product.Stock == 0)
                {
                    return new ApiResponses<CartResDto>(400, "Product is Out of Stock", null);
                }
 

                if (user.Cart == null)
                {
                    user.Cart = new Cart
                    {
                        UserId = userId,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }

                var existCartItem = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);

                if (existCartItem != null)
                {
                    if (existCartItem.Quantity < product.Stock)
                    {
                        existCartItem.Quantity++;
                        await _context.SaveChangesAsync();
                        return new ApiResponses<CartResDto>(200, "Product Quantity Increased");
                    }

                    return new ApiResponses<CartResDto>(400, "Cannot Add more Items Stock Limit reached", null);
                }

                if(existCartItem != null)
                {
                    return new ApiResponses<CartResDto>(409, "product already esist in the cart", null);
                }

                var newCartItem = new CartItem
                {
                    CartId = user.Cart.CartId,
                    ProductId = productId,
                    Quantity = 1,
                };
                user.Cart.CartItems.Add(newCartItem);
                await _context.SaveChangesAsync();

                return new ApiResponses<CartResDto>(200, "Product Added to Cart Successfully");
            } catch (Exception ex)
            {
                return new ApiResponses<CartResDto>(500, "Internal server Error", null, ex.Message);
            }
        }

        public async Task<bool> RemoveAllItems(int userId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(u => u.CartItems).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User Not Found");
                    return false;
                }

                user.Cart.CartItems.Clear();
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<bool> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
                .ThenInclude(u => u.CartItems)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (product == null)
                {
                    throw new Exception("Product Not Found");
                }

                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(u => u.ProductId == productId);
                    if (item != null)
                    {
                        user.Cart.CartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<ApiResponses<CartItem>> IncreaseQty(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(ci => ci.CartItems)
                    .ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.Id == userId);

                if(user == null)
                {
                    return new ApiResponses<CartItem>(404, "User Not Found");
                }

                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

                if(product == null)
                {
                    return new ApiResponses<CartItem>(404, "Product Not Found");
                }

                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);

                if(item == null)
                {
                    return new ApiResponses<CartItem>(404, "Item Not Found");
                }

                if (item.Quantity >= 10)
                {
                    return new ApiResponses<CartItem>(400, "Maximum Quantity reached (10 Items)");
                }

                if (product.Stock < item.Quantity)
                {
                    return new ApiResponses<CartItem>(400, "Out of Stock");
                }

                item.Quantity++;
                await _context.SaveChangesAsync();
                return new ApiResponses<CartItem>(200, "Quantity Increased Successfully");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DecreaseQty(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(ci => ci.CartItems).FirstOrDefaultAsync(p => p.Id == userId);
                if(user == null)
                {
                    throw new Exception("User Not Found");
                }

                var produdct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if(user != null && produdct != null  )
                {
                    var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId==productId);

                    if(item != null)
                    {
                        if (item.Quantity > 1)
                        {
                            item.Quantity--;
                        }
                        else if (item.Quantity == 1)
                        {
                            user.Cart.CartItems.Remove(item);
                        }
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
