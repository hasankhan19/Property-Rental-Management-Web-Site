using System.ComponentModel.DataAnnotations;

namespace RentalManagementSyetem.Models
{
    public class DTO_ClsBuilding
    {
        [Key]
        public int BuildingId { get; set; }
        public string? BuildingName { get; set; }
        public int PropertymangerId { get; set; }
        public string? status { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
