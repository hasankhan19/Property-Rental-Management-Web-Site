using Microsoft.EntityFrameworkCore;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;

namespace RentalManagementSyetem.SeedingData
{
    public class DBSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate();
            if (context.TblUser.Any())
            {
                return;
            }
            //insert data
            context.AddRange(getCitylocationlist());
            context.SaveChanges();
        }
        public static List<DTO_ClsUser> getCitylocationlist()
        {
            var dtoUser = new List<DTO_ClsUser> {
             new DTO_ClsUser{Email="admin@gamil.com" , Pasword="21232F297A57A5A743894A0E4A801FC3", Role="Admin"},
            };
            return dtoUser;
        }
    }
}
  