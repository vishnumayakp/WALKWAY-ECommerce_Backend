using AutoMapper;
using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.Address_Model.AddressDto;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Models.Category_Model.CategoryDto;
using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.Product_Model.ProductDto;
using WALKWAY_ECommerce.Models.User_Model;
using WALKWAY_ECommerce.Models.User_Model.UserDTO;
using WALKWAY_ECommerce.Models.WishList_Model;
using WALKWAY_ECommerce.Models.WishList_Model.WishListDto;

namespace WALKWAY_ECommerce.Mapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User,UserRegistrationDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Product,AddProductDto>().ReverseMap();
            CreateMap<Product,GetProductDto>().ReverseMap();
            CreateMap<WishList,WishListDto>().ReverseMap();
            //CreateMap<Address,AddressDto>().ReverseMap();

        }
    }
}
