using DiplomLayihe.AppCode.Modules.AccountModule;
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

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly DiplomDbContext db;
        private readonly SignInManager<DiplomUser> signInManager;
        private readonly UserManager<DiplomUser> userManager;

        public AccountController(IMediator mediator,
            SignInManager<DiplomUser> signInManager,
            UserManager<DiplomUser> userManager,
            DiplomDbContext db)
        {
            this.mediator = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
        }

        [Route("/signin.html")]
        public IActionResult Signin()
        {
            return View();
        }
        

        [HttpPost]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin(SigninCommand command)
        {
            

            var response = await mediator.Send(command);

            if (!ModelState.IsValid)
            {
                return View(command);
            }

            if (User.IsInRole("Superadmin") || User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "admin");
            }



            var redirectUrl = Request.Query["ReturnUrl"];

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                return Redirect(redirectUrl);
            }
            return RedirectToAction("Index", "homepage");
        }



        [Route("/signout.html")]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Homepage");
        }
    }
}
