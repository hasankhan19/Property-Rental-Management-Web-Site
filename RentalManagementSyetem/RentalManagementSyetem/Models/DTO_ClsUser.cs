using System.ComponentModel.DataAnnotations;
#nullable disable

namespace RentalManagementSyetem.Models
{
    public class DTO_ClsUser
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Pasword { get; set; }
        public string status { get; set; }
        public string Role { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
