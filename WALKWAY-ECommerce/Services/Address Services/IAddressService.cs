using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.Address_Model.AddressDto;

namespace WALKWAY_ECommerce.Services.Address_Services
{
    public interface IAddressService
    {
        Task<bool> CreateNewAddress( int userId,AddressDto addressDto);
        public Task<bool> UpdateAddress(int userId, UpdateAddressDto updateAddressDto);

        Task<bool> DeleteAddress(int addressId, int userId);

        Task<List<AddressResDto>> GetAddressById(int userId);
    }
}
