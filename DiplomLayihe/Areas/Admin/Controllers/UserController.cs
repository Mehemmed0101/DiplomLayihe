using DiplomLayihe.AppCode.Modules.AccountModule;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using DiplomLayihe.Models.FormModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DiplomLayihe.AppCodee.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class UserController : Controller
    {
        private readonly DiplomDbContext db;
        private readonly IMediator mediator;
        private readonly IWebHostEnvironment env;
        private readonly IActionContextAccessor ctx;
        private readonly UserManager<DiplomUser> userManager;
        private readonly SignInManager<DiplomUser> signInManager;

        public UserController(DiplomDbContext db,
            IMediator mediator,
            IWebHostEnvironment env,
            IActionContextAccessor ctx,
            UserManager<DiplomUser> userManager,
            SignInManager<DiplomUser> signInManager)
        {
            this.db = db;
            this.mediator = mediator;
            this.env = env;
            this.ctx = ctx;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var data = await mediator.Send(new UsersAllQuery());
            return View(data);
        }

        public async Task<IActionResult> Register()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            if (model?.file == null)
            {
                ctx.AddModelError("ProfileImg", "Fayl Sechilmeyib!");
            }


            if (ctx.ModelIsValid())
            {
                string fileExtension = Path.GetExtension(model.file.FileName);

                string name = $"user-{Guid.NewGuid()}{fileExtension}";
                string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                {
                    await model.file.CopyToAsync(fs);
                }


                var user = new DiplomUser
                {
                    ProfileImg = name,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };



                var result = await userManager.CreateAsync(user, model.Password);

                //await userManager.AddToRoleAsync(user, "Telebe");

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            
            
            }

            return View(model);
        }


        public async Task<IActionResult> Details(int id)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);


            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles
             on new { RoleId = r.Id, UserId = user.Id } equals new { ur.RoleId, ur.UserId } into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();


            //ViewBag.Principals = (from p in Program.principals
            //                      join uc in db.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id } equals new { uc.ClaimValue, uc.ClaimType, uc.UserId } into lJoin
            //                      from lj in lJoin.DefaultIfEmpty()
            //                      select Tuple.Create(p, lj != null)).ToList();

            return View(user);
        }




        [HttpPost]
        [Route("/user-set-role")]
        [Authorize("admin.users.setrole")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;

            #region CheckUser and role
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);



            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "user null"
                });
            }
            if (role == null)
            {
                return Json(new
                {
                    error = true,
                    message = "role null"
                });
            }
            #endregion

            if (selected)
            {
                if (await db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                {
                    return Json(new
                    {

                        error = true,
                        message = $"Bu rol zaten var"

                    });
                }
                else
                {
                    db.UserRoles.Add(new DiplomUserRole
                    {

                        UserId = userId,
                        RoleId = roleId

                    });

                    await db.SaveChangesAsync();

                    return Json(new
                    {

                        error = false,
                        message = $"{user.UserName} adli istifadeci {role.Name} adli rola elave edildi"

                    });
                }
            }
            else
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (userRole == null)
                {

                    return Json(new
                    {

                        error = true,
                        message = $"Xeta bash verdi!"

                    });
                }
                else
                {
                    db.UserRoles.Remove(userRole);

                    await db.SaveChangesAsync();

                    return Json(new
                    {

                        error = false,
                        message = $"{user.UserName} adli istifadeci {role.Name} adli roldan cixarildi"

                    });
                }
            }

        }





        //[HttpPost]
        //[Route("/user-set-principal")]
        //[Authorize("admin.users.principal")]
        //public async Task<IActionResult> SetPrincipal(int userId, string principalName, bool selected)
        //{
        //    var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

        //    ViewBag.User = userAbout;

        //    #region CheckUser and principal
        //    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        //    var hasPrincipal = Program.principals.Contains(principalName);



        //    if (user == null)
        //    {
        //        return Json(new
        //        {
        //            error = true,
        //            message = "user null"
        //        });
        //    }
        //    if (hasPrincipal == null)
        //    {
        //        return Json(new
        //        {
        //            error = true,
        //            message = "Principal null"
        //        });
        //    }
        //    #endregion


        //    if (selected)
        //    {
        //        if (await db.UserClaims.AnyAsync(uc => uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1") && uc.UserId == userId))
        //        {
        //            return Json(new
        //            {

        //                error = true,
        //                message = $"{user.Name} adli istifadechi {principalName} adli selahiyyete malikdir"

        //            });
        //        }
        //        else
        //        {
        //            db.UserClaims.Add(new DiplomUserClaim
        //            {

        //                UserId = userId,
        //                ClaimType = principalName,
        //                ClaimValue = "1"

        //            });

        //            await db.SaveChangesAsync();

        //            return Json(new
        //            {

        //                error = false,
        //                message = $"{principalName} adli selahiyyet {user.UserName} adli istifadechiye verildi"

        //            });
        //        }
        //    }
        //    else
        //    {
        //        var userClaim = await db.UserClaims.FirstOrDefaultAsync(uc => uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1") && uc.UserId == userId);

        //        if (userClaim == null)
        //        {
        //            return Json(new
        //            {

        //                error = true,
        //                message = $"{user.UserName} adli istifadechi {principalName} adli selahiyyete malik deyil"

        //            });
        //        }
        //        else
        //        {
        //            db.UserClaims.Remove(userClaim);
        //            await db.SaveChangesAsync();

        //            return Json(new
        //            {

        //                error = true,
        //                message = $"{user.UserName} adli istifadechiden {principalName} adli selahiyyete alindi"

        //            });
        //        }
        //    }
        //}










       
    }
}
