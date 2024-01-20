using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#nullable disable

namespace RentalManagementSyetem.Controllers
{
    public class SetUpController : BaseController
    {
        public SetUpController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        #region Building

        [Authorize(Roles = "Admin, PropertyManager")]
        public IActionResult BuildingIndex()
        {
            return View();
        }
        [Authorize(Roles = "Admin, PropertyManager")]
        [HttpPost]
        public IActionResult LookUpItemBuilding()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsBuilding> buildinglist = (from c in _context.TblBuilding
                                                      where c.status == "Active"
                                                      select new DTO_ClsBuilding
                                                      {
                                                          BuildingId = c.BuildingId,
                                                          BuildingName = c.BuildingName,
                                                          status = c.status
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
                    buildinglist = buildinglist.Where(x => x.BuildingName.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = buildinglist.Count();
                var data = buildinglist.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin, PropertyManager")]
        public IActionResult AddorEditBuilding(int id)
        {
            try
            {
                DTO_ClsBuilding building = new DTO_ClsBuilding();
                if (id == 0)
                {
                    building.BuildingId = 0;
                    building.BuildingName = "";
                    building.status = "";
                }
                else
                {
                    building = (from c in _context.TblBuilding
                                where c.BuildingId == id
                                select new DTO_ClsBuilding
                                {
                                    BuildingId = c.BuildingId,
                                    PropertymangerId=c.PropertymangerId,
                                    BuildingName = c.BuildingName,
                                    status = c.status
                                }).FirstOrDefault();
                }
                ViewBag.SetUp = new SelectList(gtePropertyMangerlist(), "UserID", "FirstName");

                return PartialView("InsertUpadetDeleteBuilding", building);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }

        }
        [Authorize(Roles = "Admin, PropertyManager")]
        [HttpPost]
        public JsonResult InsertUpdateDeleteBuilding(string Buildingobj, string action)
        {
            try
            {
                DTO_ClsBuilding building = JsonConvert.DeserializeObject<DTO_ClsBuilding>(Buildingobj);
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);
                var buildingInfo = new DTO_ClsBuilding
                {
                    BuildingId = building.BuildingId,
                    BuildingName = building.BuildingName,
                    status = building.status,
                    PropertymangerId=building.PropertymangerId,
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now,

                };
                if (buildingInfo != null && action == "Save")
                {
                    _context.Add(buildingInfo);
                }
                else if (buildingInfo != null && action == "Update")
                {
                    var exitingInfo = _context.TblBuilding.FirstOrDefault(x => x.BuildingId == building.BuildingId);
                    if (exitingInfo != null)
                    {
                        exitingInfo.BuildingId = building.BuildingId;
                        exitingInfo.BuildingName = building.BuildingName;
                        exitingInfo.status = building.status;
                        exitingInfo.PropertymangerId=building.PropertymangerId;
                        exitingInfo.Updatedby = userid;
                        exitingInfo.CreatedDateTime = System.DateTime.Now;
                        exitingInfo.Updatedby = userid;
                        exitingInfo.UpdatedDateTime = System.DateTime.Now;
                        _context.Update(exitingInfo);
                    }

                }
                else if (buildingInfo != null && action == "Delete")
                {
                    var exitingInfo = _context.TblBuilding.FirstOrDefault(x => x.BuildingId == building.BuildingId);
                    if (exitingInfo != null)
                    {
                        _context.RemoveRange(exitingInfo);
                    }
                    else
                    {
                        return new JsonResult(new { success = false, data = "Not Found" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, data = "Some Thing Went Wrong" });
                }
                _context.SaveChanges();
                return new JsonResult(new { success = false, data = action + " " + "Data SuccessFully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, data = e.Message });
            }
        }
        #endregion




        #region Apartment
        [Authorize(Roles = "Admin, PropertyManager")]
        public IActionResult ApartmentIndex()
        {
            return View();
        }
        [Authorize(Roles = "Admin, PropertyManager")]
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
                                                            Status = c.Status
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
        [Authorize(Roles = "Admin, PropertyManager")]
        public IActionResult AddorEditApartment(int id)
        {
            try
            {
                Dto_ClsApartment apertmentlist = new Dto_ClsApartment();
                if (id == 0)
                {
                    apertmentlist.ApartmentId = 0;
                    apertmentlist.BuildingId = 0;
                    apertmentlist.Price = 0;
                    apertmentlist.Bedrooms = 0;
                    apertmentlist.Bathrooms = 0;
                    apertmentlist.Status = "";
                }
                else
                {
                    apertmentlist = (from c in _context.TblApartment
                                     where c.ApartmentId == id
                                     select new Dto_ClsApartment
                                     {
                                         ApartmentId = c.ApartmentId,
                                         BuildingId = c.BuildingId,
                                         Address = c.Address,
                                         Price = c.Price,
                                         Bedrooms = c.Bedrooms,
                                         Bathrooms = c.Bathrooms,
                                         Status = c.Status
                                     }).FirstOrDefault();
                }
                ViewBag.building = new SelectList(buildinglist(), "BuildingId", "BuildingName");
                return PartialView("InsertUpadetDeleteApartment", apertmentlist);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }

        }
        [Authorize(Roles = "Admin, PropertyManager")]
        [HttpPost]
        public JsonResult InsertUpdateDeleteApartment(string aprtmentobj, string action)
        {
            try
            {
                Dto_ClsApartment apartmentlist = JsonConvert.DeserializeObject<Dto_ClsApartment>(aprtmentobj);
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);
                var apertmentinfo = new Dto_ClsApartment
                {
                    ApartmentId = apartmentlist.ApartmentId,
                    BuildingId = apartmentlist.BuildingId,
                    Address = apartmentlist.Address,
                    Price = apartmentlist.Price,
                    Bedrooms = apartmentlist.Bedrooms,
                    Bathrooms = apartmentlist.Bathrooms,
                    Status = apartmentlist.Status,
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now,

                };
                if (apertmentinfo != null && action == "Save")
                {
                    _context.Add(apertmentinfo);
                }
                else if (apertmentinfo != null && action == "Update")
                {
                    var exitingInfo = _context.TblApartment.FirstOrDefault(x => x.ApartmentId == apartmentlist.ApartmentId);
                    if (exitingInfo != null)
                    {
                        exitingInfo.BuildingId = apartmentlist.BuildingId;
                        exitingInfo.ApartmentId = apartmentlist.ApartmentId;
                        exitingInfo.Price = apartmentlist.Price;
                        exitingInfo.Address = apartmentlist.Address;
                        exitingInfo.Bedrooms = apartmentlist.Bedrooms;
                        exitingInfo.Bathrooms = apartmentlist.Bathrooms;
                        exitingInfo.Status = apartmentlist.Status;
                        exitingInfo.Updatedby = userid;
                        exitingInfo.CreatedDateTime = System.DateTime.Now;
                        exitingInfo.Updatedby = userid;
                        exitingInfo.UpdatedDateTime = System.DateTime.Now;
                        _context.Update(exitingInfo);
                    }

                }
                else if (apertmentinfo != null && action == "Delete")
                {
                    var exitingInfo = _context.TblApartment.FirstOrDefault(x => x.ApartmentId == apartmentlist.ApartmentId);
                    if (exitingInfo != null)
                    {
                        _context.RemoveRange(exitingInfo);
                    }
                    else
                    {
                        return new JsonResult(new { success = false, data = "Not Found" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, data = "Some Thing Went Wrong" });
                }
                _context.SaveChanges();
                return new JsonResult(new { success = false, data = action + " " + "Data SuccessFully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, data = e.Message });
            }
        }
        #endregion

        #region Admin
        [Authorize(Roles ="Admin")]
        public IActionResult AdminIndex()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult LookUpAdmin()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsUser> userlist = (from c in _context.TblUser
                                              where c.status == "Active" && c.Role== "Admin"
                                              select new DTO_ClsUser
                                              {
                                                  UserID=c.UserID,
                                                  FirstName = c.FirstName,
                                                  LastName = c.LastName,
                                                  Email = c.Email,
                                                  ContactNo = c.ContactNo,
                                                  Address = c.Address,
                                                  status = c.status
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
                    userlist = userlist.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = userlist.Count();
                var data = userlist.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddorEditAdmin(int id)
        {
            try
            {
                DTO_ClsUser clsUser = new DTO_ClsUser();
                if (id == 0)
                {
                    clsUser.UserID = 0;
                    clsUser.FirstName = "";
                    clsUser.LastName= "";
                    clsUser.Email = "";
                    clsUser.status = "";
                }
                else
                {
                    clsUser = (from c in _context.TblUser
                                where c.UserID == id && c.Role=="Admin"
                                select new DTO_ClsUser  
                                {
                                   UserID=c.UserID,
                                   FirstName=c.FirstName,
                                   LastName=c.LastName,
                                   Email=c.LastName,
                                   ContactNo=c.ContactNo,
                                   Address=c.Address,
                                   status = c.status
                                }).FirstOrDefault();
                }
                return PartialView("InsertUpadetDeleteAdmin", clsUser);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult InsertUpdateDeleteAdmin(string adminobj, string action)
        {
            try
            {
                DTO_ClsUser clsUser = JsonConvert.DeserializeObject<DTO_ClsUser>(adminobj);
                string hashpassword = "";

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);


                using (MD5 vmd5 = MD5.Create())
                {
                    byte[] modeldata = Encoding.ASCII.GetBytes(clsUser.Pasword);
                    byte[] bytehash = vmd5.ComputeHash(modeldata);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in bytehash)
                    {
                        stringBuilder.Append(item.ToString("X2"));
                    }
                    hashpassword = stringBuilder.ToString();
                }
                var userinfo = new DTO_ClsUser
                {
                    FirstName = clsUser.FirstName,
                    LastName = clsUser.LastName,
                    Email = clsUser.Email,
                    Pasword = hashpassword,
                    Role = "Admin",
                    Address = clsUser.Address,
                    ContactNo = clsUser.ContactNo,
                    status=clsUser.status,
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now,

                };
                if (clsUser != null && action == "Save")
                {
                    _context.Add(userinfo);
                }
                else if (clsUser != null && action == "Update")
                {
                    var exitingUser = _context.TblUser.FirstOrDefault(x => x.UserID == clsUser.UserID);
                    if (exitingUser != null)
                    {
                        exitingUser.FirstName = clsUser.FirstName;
                        exitingUser.LastName = clsUser.LastName;
                        exitingUser.Email = clsUser.Email;
                        exitingUser.Pasword = hashpassword;
                        exitingUser.Role = "Admin";
                        exitingUser.Address = clsUser.Address;
                        exitingUser.ContactNo = clsUser.ContactNo;
                        exitingUser.Updatedby = 1;
                        exitingUser.CreatedDateTime = System.DateTime.Now;
                        exitingUser.Updatedby = 1;
                        exitingUser.UpdatedDateTime = System.DateTime.Now;
                        _context.Update(exitingUser);
                    }

                }
                else if (clsUser != null && action == "Delete")
                {
                    var exitingUser = _context.TblUser.FirstOrDefault(x => x.UserID == clsUser.UserID);
                    if (exitingUser != null)
                    {
                        _context.RemoveRange(exitingUser);
                    }
                    else
                    {
                        return new JsonResult(new { success = false, data = "User Not Found" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, data = "Some Thing Went Wrong" });
                }
                _context.SaveChanges();
                return new JsonResult(new { success = false, data = action + " " + "Data Successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, data = e.Message });
            }
        }
        #endregion

        #region Property Manager
        [Authorize(Roles = "Admin,PropertyManager")]
        public IActionResult PropertyManagerIndex()
        {
            return View();
        }
        [Authorize(Roles = "Admin, PropertyManager")]
        [HttpPost]
        public IActionResult LookUpPropertyManager()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                List<DTO_ClsUser> userlist = (from c in _context.TblUser
                                              where c.status == "Active" && c.Role == "PropertyManager"
                                              select new DTO_ClsUser
                                              {
                                                  UserID = c.UserID,
                                                  FirstName = c.FirstName,
                                                  LastName = c.LastName,
                                                  Email = c.Email,
                                                  ContactNo = c.ContactNo,
                                                  Address = c.Address,
                                                  status = c.status
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
                    userlist = userlist.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                recordsTotal = userlist.Count();
                var data = userlist.Skip(skip).Take(pageSize).ToList();


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin, PropertyManager")]
        public IActionResult AddorEditPropertyManager(int id)
        {
            try
            {
                DTO_ClsUser clsUser = new DTO_ClsUser();
                if (id == 0)
                {
                    clsUser.UserID = 0;
                    clsUser.FirstName = "";
                    clsUser.LastName = "";
                    clsUser.Email = "";
                    clsUser.status = "";
                }
                else
                {
                    clsUser = (from c in _context.TblUser
                               where c.UserID == id && c.Role == "PropertyManager"
                               select new DTO_ClsUser
                               {
                                   UserID = c.UserID,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   Email = c.LastName,
                                   ContactNo = c.ContactNo,
                                   Address = c.Address,
                                   status = c.status
                               }).FirstOrDefault();
                }
                return PartialView("InsertUpadetDeletePropertyManager", clsUser);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }

        }
        [Authorize(Roles = "Admin, PropertyManager")]
        [HttpPost]
        public JsonResult InsertUpdateDeletePropertyManager(string adminobj, string action)
        {
            try
            {
                DTO_ClsUser clsUser = JsonConvert.DeserializeObject<DTO_ClsUser>(adminobj);
                string hashpassword = "";
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var result = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);
                using (MD5 vmd5 = MD5.Create())
                {
                    byte[] modeldata = Encoding.ASCII.GetBytes(clsUser.Pasword);
                    byte[] bytehash = vmd5.ComputeHash(modeldata);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in bytehash)
                    {
                        stringBuilder.Append(item.ToString("X2"));
                    }
                    hashpassword = stringBuilder.ToString();
                }
                var userinfo = new DTO_ClsUser
                {
                    FirstName = clsUser.FirstName,
                    LastName = clsUser.LastName,
                    Email = clsUser.Email,
                    Pasword = hashpassword,
                    Role = "PropertyManager",
                    Address = clsUser.Address,
                    ContactNo = clsUser.ContactNo,
                    status = clsUser.status,
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now,

                };
                if (clsUser != null && action == "Save")
                {
                    _context.Add(userinfo);
                }
                else if (clsUser != null && action == "Update")
                {
                    var exitingUser = _context.TblUser.FirstOrDefault(x => x.UserID == clsUser.UserID);
                    if (exitingUser != null)
                    {
                        exitingUser.FirstName = clsUser.FirstName;
                        exitingUser.LastName = clsUser.LastName;
                        exitingUser.Email = clsUser.Email;
                        exitingUser.Pasword = hashpassword;
                        exitingUser.Role = "PropertyManager";
                        exitingUser.Address = clsUser.Address;
                        exitingUser.ContactNo = clsUser.ContactNo;
                        exitingUser.Updatedby = userid;
                        exitingUser.CreatedDateTime = System.DateTime.Now;
                        exitingUser.Updatedby = userid;
                        exitingUser.UpdatedDateTime = System.DateTime.Now;
                        _context.Update(exitingUser);
                    }

                }
                else if (clsUser != null && action == "Delete")
                {
                    var exitingUser = _context.TblUser.FirstOrDefault(x => x.UserID == clsUser.UserID);
                    if (exitingUser != null)
                    {
                        _context.RemoveRange(exitingUser);
                    }
                    else
                    {
                        return new JsonResult(new { success = false, data = "User Not Found" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, data = "Some Thing Went Wrong" });
                }
                _context.SaveChanges();
                return new JsonResult(new { success = false, data = action + " " + "Data Successfully" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, data = e.Message });
            }
        }
        #endregion
        #region get list of building
        public List<DTO_ClsBuilding> buildinglist()
        {
            List<DTO_ClsBuilding> building = new List<DTO_ClsBuilding>();
            building = (from b in _context.TblBuilding
                        select new DTO_ClsBuilding
                        {
                            BuildingId = b.BuildingId,
                            BuildingName = b.BuildingName
                        }).ToList();
            return building;
        }
        #endregion

        #region get list of propertyManger
        public List<DTO_ClsUser> gtePropertyMangerlist() { 
         List<DTO_ClsUser> clsUsers=new List<DTO_ClsUser>();
            clsUsers = (from p in _context.TblUser where p.Role== "PropertyManager"
                        select new DTO_ClsUser
                        {
                            UserID=p.UserID,
                            FirstName = p.FirstName,
                        }).ToList();
            return clsUsers;
        }
        #endregion
    }
}
