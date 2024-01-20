using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
#nullable disable

namespace RentalManagementSyetem.Models
{
    public class DTO_ClsAppointment
    {
        [Key]
        public int AppId { get; set; }
        public int TenantId { get; set; }
        public int ManagerId { get; set; }
        public int ApartmentId { get; set; }
        [NotMapped]
        public string BuildingName { get; set; }
        public DateTime  SuggestedDateTime{ get; set; }
        public DateTime FromDateTime { get; set; }
        [NotMapped]
        public string sdate { get; set; }
        [NotMapped]
        public string fdate { get; set; }
        public string status { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
