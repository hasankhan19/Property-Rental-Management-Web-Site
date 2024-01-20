using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSyetem.Models
{
    public class Dto_ClsApartment
    {
        [Key]
        public int ApartmentId { get; set; }
        public int? BuildingId { get; set; }
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Status { get; set; }
        [NotMapped]
        public string? BuildingName { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
