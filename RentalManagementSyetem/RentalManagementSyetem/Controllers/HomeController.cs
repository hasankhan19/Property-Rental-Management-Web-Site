using Microsoft.AspNetCore.Mvc;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;

namespace RentalManagementSyetem.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LookUpApartment()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<Dto_ClsApartment> apartmentlist = (from c in _context.TblApartment
                                                        where c.Status == "Active"  
                                                        select new Dto_ClsApartment
                                                        {
                                                            ApartmentId = c.ApartmentId,
                                                            BuildingId = c.BuildingId,
                                                            Address = c.Address,
                                                            Price = c.Price,
                                                            Bedrooms = c.Bedrooms,
                                                            Bathrooms = c.Bathrooms,
                                                            Status = c.Status,
                                                        }).ToList();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    // Add your sorting logic here
                }
                // Perform filtering based on the search value

                if (!string.IsNullOrEmpty(searchValue))
                {
                    apartmentlist = apartmentlist.Where(x => x.Address.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = apartmentlist.Count();
                var data = apartmentlist.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
    }
}
