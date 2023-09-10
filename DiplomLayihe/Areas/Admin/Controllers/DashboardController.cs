using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin, Admin")]
    public class DashboardController : Controller
    {
        private readonly DiplomDbContext db;
        private readonly UserManager<DiplomUser> userManager;

        public DashboardController(DiplomDbContext db, 
            UserManager<DiplomUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
            
            ViewBag.User = userAbout;

            var obshiUser = await db.Users.ToListAsync();

            ViewBag.Obshiuser = obshiUser;
            var adminUser = await db.Users.ToListAsync();

            ViewBag.AdminUser = adminUser;


            return View();

        }
    }
}
