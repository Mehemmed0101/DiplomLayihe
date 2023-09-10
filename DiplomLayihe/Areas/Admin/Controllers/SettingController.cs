using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class SettingController : Controller
    {
        private readonly DiplomDbContext db;
        private readonly UserManager<DiplomUser> userManager;
        private readonly IMediator mediator;

        public SettingController(IMediator mediator, DiplomDbContext db,
            UserManager<DiplomUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(int userId)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;

            if (userAbout.Id != userId)
            {

                return View(NotFound());
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SettingEdit(string Name, string Surname, string Email, string PhoneNumber)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;

            userAbout.Name = Name;
            userAbout.Surname = Surname;
            userAbout.Email = Email;
            userAbout.PhoneNumber = PhoneNumber;

            await userManager.UpdateAsync(userAbout);

            return RedirectToAction("Index", "dashboard");
        }


        public async Task<IActionResult> SifreEdit()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SifreEdit(string OldPassword, string NewPassword)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;


            await userManager.ChangePasswordAsync(userAbout, OldPassword, NewPassword);

            return RedirectToAction("Index", "dashboard");
        }
    }
}
