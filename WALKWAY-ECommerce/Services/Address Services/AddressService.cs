using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.Address_Model.AddressDto;
using WALKWAY_ECommerce.Models.User_Model;

namespace WALKWAY_ECommerce.Services.Address_Services
{
    public class AddressService:IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNewAddress(int userId, AddressDto addressDto)
        {
            try
            {
                if(addressDto == null)
                {
                    throw new ArgumentNullException(nameof(addressDto),"Address information is Required");
                }
                var user = await _context.Users.FindAsync(userId);
                if(user == null)
                {
                    throw new Exception("User Not Found");
                }
                var addressCount = await _context.Addresses.CountAsync(a => a.UserId == userId);
                if (addressCount >= 4)
                {
                    throw new Exception("Address limit reached. You can only have 4 addresses.");
                }

                var newAdress = new Address
                {
                    UserId = userId,
                    FullName = addressDto.FullName,
                    PhoneNumber = addressDto.PhoneNumber,
                    Pincode = addressDto.Pincode,
                    HouseName = addressDto.HouseName,
                    Place = addressDto.Place,
                    PostOffice = addressDto.PostOffice,
                    LandMark = addressDto.LandMark,
                };

                 _context.Addresses.Add(newAdress);
                await _context.SaveChangesAsync();
                return true;
                

            }catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Error Occured While Creating Address", ex);
            }
        }


        public async Task<List<AddressResDto>> GetAddressById(int userId)
        {
            try
            {
                var address = _context.Addresses.Where(u => u.UserId == userId).ToList();

                if(address == null || address.Count == 0)
                {
                    return new List<AddressResDto>();
                }

                var addressDto = address.Select(a => new AddressResDto
                {
                    AddressId=a.AddressId,
                    FullName = a.FullName,
                    PhoneNumber = a.PhoneNumber,
                    Pincode = a.Pincode,
                    HouseName = a.HouseName,
                    Place = a.Place,
                    PostOffice = a.PostOffice,
                    LandMark = a.LandMark
                }).ToList();

                return addressDto;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAddress(int userId, UpdateAddressDto updateAddressDto)
        {
            try
            {
                if (updateAddressDto == null)
                {
                    throw new ArgumentNullException(nameof(updateAddressDto), "Address information is required");
                }

                var address = await _context.Addresses.Where(u => u.UserId == userId && u.AddressId == updateAddressDto.AddressId).FirstOrDefaultAsync();

                if (address == null)
                {
                    throw new Exception("Address not found for the given user");
                }

                address.FullName = updateAddressDto.FullName;
                address.PhoneNumber = updateAddressDto.PhoneNumber;
                address.Pincode = updateAddressDto.Pincode;
                address.HouseName = updateAddressDto.HouseName;
                address.Place = updateAddressDto.Place;
                address.PostOffice = updateAddressDto.PostOffice;
                address.LandMark = updateAddressDto.LandMark;

                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();

                return true;
            }catch(Exception ex)
            {
                throw new Exception("Error occurred while updating address", ex);
            }
        }

        public async Task<bool> DeleteAddress(int addressId, int userId)
        {
            try
            {
                var address = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);
                if (address == null)
                {
                    return false;
                }

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting address", ex);
            }
        }


    }
}
