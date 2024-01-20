using Microsoft.EntityFrameworkCore;
using RentalManagementSyetem.Models;
#nullable disable

namespace RentalManagementSyetem.AppDbContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
         
        }
        public DbSet<DTO_ClsUser> TblUser { get; set; }
        public DbSet<Dto_ClsApartment> TblApartment { get; set; }
        public DbSet<DTO_ClsBuilding> TblBuilding { get; set; } 
        public DbSet<DTO_ClsAppointment> TblAppointment { get; set; }
    }
}
