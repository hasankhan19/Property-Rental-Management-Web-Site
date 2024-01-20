using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;
using System.Security.Claims;
#nullable disable

namespace RentalManagementSyetem.Controllers
{
    public class AppointmentController : BaseController
    {
        public AppointmentController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        [HttpGet]
        public IActionResult RequestForAppointment(int id)
        {
            Dto_ClsApartment apartment = new Dto_ClsApartment();
            try
            {
               
                if (id != 0)
                {
                    apartment = (from a in _context.TblApartment
                                 where a.ApartmentId == id
                                 select new Dto_ClsApartment
                                 {
                                     ApartmentId = a.ApartmentId,
                                     Address = a.Address,
                                     Price = a.Price,
                                     Bathrooms = a.Bathrooms,
                                     Bedrooms = a.Bedrooms,
                                 }).FirstOrDefault();
                    ViewBag.prpertyMangerlist = new SelectList(prpertyMangerlist(), "UserID", "FirstName");
                    ViewBag.Appartmentlist = new SelectList(Appartmentlist(), "ApartmentId", "Address");
                    return PartialView("RequestSubmitForAppointment", apartment);

                }
                else
                {
                    return new JsonResult(new { success = false, message = "Error" });
                }

            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = e.Message });
            }
        }
        [HttpPost]
        public JsonResult SendrequestForAppointment(string Appointmentrequest)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);
                DTO_ClsAppointment appointment = JsonConvert.DeserializeObject<DTO_ClsAppointment>(Appointmentrequest);
                var appointmentInfo = new DTO_ClsAppointment
                {
                    ApartmentId = appointment.ApartmentId,
                    TenantId = userid,
                    ManagerId = appointment.ManagerId,
                    SuggestedDateTime = appointment.SuggestedDateTime,
                    FromDateTime = appointment.FromDateTime,
                    status = "Request",
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now

                };
                _context.Add(appointmentInfo);
                _context.SaveChanges();
                return new JsonResult(new { success = true, data = "Data SuccessFully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, data = e.Message });
            }
        }

        #region prpertyMangerlist
        public List<DTO_ClsUser> prpertyMangerlist()
        {
            List<DTO_ClsUser> propertymenagerlist = new List<DTO_ClsUser>();
            propertymenagerlist = (from t in _context.TblUser
                                   where t.Role == "PropertyManager"
                                   select new DTO_ClsUser
                                   {
                                       UserID = t.UserID,
                                       FirstName = t.FirstName,
                                   }).ToList();
            return propertymenagerlist;
        }
        #endregion

        #region Appartmentlist
        public List<Dto_ClsApartment> Appartmentlist()
        {
            List<Dto_ClsApartment> Appartmentlist = new List<Dto_ClsApartment>();
            Appartmentlist = (from t in _context.TblApartment

                              select new Dto_ClsApartment
                              {
                                  ApartmentId = t.ApartmentId,
                                  Address = t.Address,
                              }).ToList();
            return Appartmentlist;
        }
        #endregion
    }
}
