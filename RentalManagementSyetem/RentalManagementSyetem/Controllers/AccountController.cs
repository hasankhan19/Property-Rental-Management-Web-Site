using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagementSyetem.AppDbContext;
using RentalManagementSyetem.Models;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#nullable disable

namespace RentalManagementSyetem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: accoun
        public IActionResult LogIn()
      {
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(DTO_ClsUser clsUser)
        {
            try
            {
                if (clsUser.Email != null)
                {
                    string hashpassword = "";
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
                        var userinfo = (from u in _context.TblUser
                                            where u.Email == clsUser.Email && u.Pasword == hashpassword
                                            select new DTO_ClsUser
                                            {
                                                UserID= u.UserID,
                                                Email=u.Email,
                                                Pasword=u.Pasword,
                                                Role=u.Role,

                                            }).FirstOrDefault();

                        if (userinfo != null)
                        {
                            var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.NameIdentifier,userinfo.UserID.ToString()),
                      new Claim(ClaimValueTypes.Email,userinfo.Email.ToString()),
                    };
                            if (userinfo!=null && userinfo.Role== "Admin")
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            }
                            else if(userinfo!=null && userinfo.Role== "PropertyManager")
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "PropertyManager"));
                            }
                            else
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Tenants"));
                            }
                            var claimidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var authproperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                                IsPersistent = true,
                                IssuedUtc = DateTimeOffset.UtcNow,
                            };
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                 new ClaimsPrincipal(claimidentity), authproperties);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.msg = "Enter Correct UserName or Password";
                            return View("LogIn");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("LogIn", "Home", new { area = "" });
                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
        [Authorize(Roles = "Admin, PropertyManager")]

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,PropertyManager")]

        [HttpPost]
        public IActionResult LookUpTenants()
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
                                              where c.status == "Active" && c.Role == "Tenants"
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
        // GET: accoun/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DTO_ClsUser clsUser)
        {
            if (clsUser != null)
            {
               HttpContext httpContext=_httpContextAccessor.HttpContext;
                var result=httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userid = Convert.ToInt32(result);

                string hashpassword = "";

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
                    Role = "Tenants",
                    Address = clsUser.Address,
                    ContactNo = clsUser.ContactNo,
                    status = clsUser.status,
                    Createdby = userid,
                    CreatedDateTime = System.DateTime.Now,
                    Updatedby = userid,
                    UpdatedDateTime = System.DateTime.Now
                };
                _context.Add(userinfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(clsUser);
            }
        }
        // GET: accoun/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUser == null)
            {
                return NotFound();
            }

            var dTO_ClsUser = await _context.TblUser.FindAsync(id);
            if (dTO_ClsUser == null)
            {
                return NotFound();
            }
            return View(dTO_ClsUser);
        }

        [HttpPost]
        public IActionResult Edit(DTO_ClsUser clsUser)
        {
            if (clsUser != null)
            {
                var existingUser = _context.TblUser.Where(x => x.UserID == clsUser.UserID).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.UserID= clsUser.UserID;
                    existingUser.FirstName = clsUser.FirstName;
                    existingUser.LastName = clsUser.LastName;
                    existingUser.Email = clsUser.Email;
                    existingUser.Address = clsUser.Address;
                    existingUser.ContactNo = clsUser.ContactNo;
                    existingUser.status = clsUser.status;
                    existingUser.Createdby = 1;
                    existingUser.CreatedDateTime=System.DateTime.Now;
                    existingUser.Updatedby= 1;
                    existingUser.UpdatedDateTime=System.DateTime.Now;
                    _context.Update(existingUser);
                    _context.SaveChanges();
                    ViewBag.data = "Update Data Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(clsUser);
                }
            }
            else
            {
                return View(clsUser);
            }
        }
        // GET: accoun/Delete/5
        [Authorize(Roles = "Admin, PropertyManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUser == null)
            {
                return NotFound();
            }

            var dTO_ClsUser = await _context.TblUser
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (dTO_ClsUser == null)
            {
                return NotFound();
            }

            return View(dTO_ClsUser);
        }
        [Authorize(Roles = "Admin, PropertyManager")]
        // POST: accoun/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(DTO_ClsUser clsUser)
        {
            if (_context.TblUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TblUser'  is null.");
            }
            var dTO_ClsUser = await _context.TblUser.FindAsync(clsUser.UserID);
            if (dTO_ClsUser != null)
            {
                _context.TblUser.Remove(dTO_ClsUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DTO_ClsUserExists(int id)
        {
            return (_context.TblUser?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "Account");
        }
    }
}
