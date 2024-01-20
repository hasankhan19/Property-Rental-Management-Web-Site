using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;
using System.Security.Claims;

namespace RentalManagementSyetem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportsController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult ReportsIndex()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LookUpItemReport()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsAppointment> appointments = (from c in _context.TblAppointment
                                                         where c.status == "Request"
                                                         select new DTO_ClsAppointment
                                                         {
                                                             AppId = c.AppId,
                                                             status = c.status,
                                                             sdate = c.SuggestedDateTime.ToString("MM/dd/yyyy"),
                                                             fdate = c.FromDateTime.ToString("MM/dd/yyyy")
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
                    appointments = appointments.Where(x => x.status.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = appointments.Count();
                var data = appointments.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        public IActionResult IncommingReportsIndex()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LookUpIncommingReport()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsAppointment> appointments = (from c in _context.TblAppointment
                                                         where c.status == "Unread"
                                                         select new DTO_ClsAppointment
                                                         {
                                                             AppId = c.AppId,
                                                             status = c.status,
                                                             sdate = c.SuggestedDateTime.ToString("MM/dd/yyyy"),
                                                             fdate = c.FromDateTime.ToString("MM/dd/yyyy")
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
                    appointments = appointments.Where(x => x.status.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = appointments.Count();
                var data = appointments.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        //update the status
        [HttpGet]
        public IActionResult UPdateStatus(int id)
        {
            try
            {
                if (id != 0)
                {
                    var existingmsg = _context.TblAppointment.Where(x => x.AppId == id).FirstOrDefault();
                    if (existingmsg != null)
                    {
                        existingmsg.status = "Unread";
                        _context.Entry(existingmsg).State = EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction(nameof(ReportsIndex));
                    }
                    else
                    {
                        return RedirectToAction(nameof(ReportsIndex));

                    }
                }
                else
                {
                    return RedirectToAction(nameof(ReportsIndex));
                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
        //update the status
        [HttpGet]
        public IActionResult ApprovedRequest(int id)
        {
            try
            {
                if (id != 0)
                {
                    var existingmsg = _context.TblAppointment.Where(x => x.AppId == id).FirstOrDefault();
                    if (existingmsg != null)
                    {
                        existingmsg.status = "Approved";
                        _context.Entry(existingmsg).State = EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction(nameof(ReportsIndex));
                    }
                    else
                    {
                        return RedirectToAction(nameof(ReportsIndex));

                    }
                }
                else
                {
                    return RedirectToAction(nameof(ReportsIndex));
                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
        public IActionResult CompletedRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LookUpICompletedRequest()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsAppointment> appointments = (from c in _context.TblAppointment
                                                         where c.status == "Approved"
                                                         select new DTO_ClsAppointment
                                                         {
                                                             AppId = c.AppId,
                                                             status = c.status,
                                                             sdate =System.DateTime.Now.ToString("MM/dd/yyyy"),
                                                             fdate = System.DateTime.Now.ToString("MM/dd/yyyy")
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
                    appointments = appointments.Where(x => x.status.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = appointments.Count();
                var data = appointments.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        public IActionResult TenatsReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LookUpTenatsReport()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);

                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsAppointment> appointments = (from c in _context.TblAppointment
                                                         join a in _context.TblApartment on c.ApartmentId equals a.ApartmentId
                                                         join b in _context.TblBuilding on a.BuildingId equals b.BuildingId
                                                         where c.status == "Approved" && c.TenantId == userid
                                                         select new DTO_ClsAppointment
                                                         {
                                                             AppId = c.AppId,
                                                             status = c.status,
                                                             BuildingName=b.BuildingName,
                                                             sdate = System.DateTime.Now.ToString("MM/dd/yyyy"),
                                                             fdate = System.DateTime.Now.ToString("MM/dd/yyyy")
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
                    appointments = appointments.Where(x => x.status.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = appointments.Count();
                var data = appointments.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
    }
}

