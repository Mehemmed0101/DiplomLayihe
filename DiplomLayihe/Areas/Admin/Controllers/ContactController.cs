using DiplomLayihe.AppCode.Modules.ContactPostModule;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using DiplomLayihe.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class ContactController : Controller
    {
        
        private readonly DiplomDbContext db;
        private readonly UserManager<DiplomUser> userManager;
        private readonly IMediator mediator;

        public ContactController(IMediator mediator, DiplomDbContext db,
            UserManager<DiplomUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.mediator = mediator;
        }


        public async Task<IActionResult> Index()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var data = await mediator.Send(new ContactAllQuery());
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            ViewData["Getsearch"] = search;

            var items = from x in db.ContactUs.Where(cc => cc.DeletedById == null) select x;

            if (!String.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Name.Contains(search) || i.Email.Contains(search) || i.Subject.Contains(search) || i.Comment.Contains(search));
            }

            return View(await items.AsNoTracking().ToListAsync());
        }




        public async Task<IActionResult> Answered()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var data = await mediator.Send(new ContactAnsweredQuery());
            return View(data);
        }


        public async Task<IActionResult> NonAnswered()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var data = await mediator.Send(new ContactNonAnsweredQuery());
            return View(data);
        }



        public async Task<IActionResult> Create(string name, string email, string comment, string subject)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            ContactUs newContact = new ContactUs
            {
                Name = name,
                Email = email,
                Comment = comment,
                Subject = subject,
                EmailSended = false

            };
            await db.ContactUs.AddAsync(newContact);
            await db.SaveChangesAsync();


            return Json(new { status = 200 });
        }



        public async Task<IActionResult> Details(ContactSingleQuery query)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var entity = await mediator.Send(query);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }



        public async Task<IActionResult> ToAnswer(ContactSingleQuery query)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }

            var command = new ContactAnswerCommand();
            command.Name = entity.Name;
            command.Email = entity.Email;
            command.Comment = entity.Comment;
            command.Answer = entity.Answer;
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ToAnswer(int id, ContactAnswerCommand command)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            if (id != command.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }



        public async Task<IActionResult> Subscribe(int id, ContactSendEmailCommand command)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            return Json(response);
        }


        [HttpPost]

        public async Task<IActionResult> Delete(ContactRemoveCommand command)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool ContactUsExists(int id)
        {
            return db.ContactUs.Any(e => e.Id == id);
        }


    }
}
