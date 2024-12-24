using System.ComponentModel.DataAnnotations;

namespace WALKWAY_ECommerce.Models.Address_Model.AddressDto
{
    public class AddressDto
    {

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Pincode { get; set; }

        public string? HouseName { get; set; }

        public string? Place { get; set; }

        public string? PostOffice { get; set; }

        public string? LandMark { get; set; }
    }
}
