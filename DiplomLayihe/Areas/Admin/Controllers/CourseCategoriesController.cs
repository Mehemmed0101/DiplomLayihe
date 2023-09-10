using DiplomLayihe.AppCode.Modules.CourseCategoriesModule;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using MediatR;
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
    [Authorize(Roles = "Superadmin,Admin")]
    public class CourseCategoriesController : Controller
    {
        private readonly DiplomDbContext db;
        private readonly UserManager<DiplomUser> userManager;
        private readonly IMediator mediator;

        public CourseCategoriesController(IMediator mediator, DiplomDbContext db,
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
            var data = await mediator.Send(new CourseCategoriesAllQuery());
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            ViewData["Getsearch"] = search;

            var items = from x in db.CourseCategories.Where(cc=>cc.DeletedById == null) select x;

            if (!String.IsNullOrEmpty(search))
            {
                items = items.Where(i =>i.CourseName.Contains(search) || i.Level.Contains(search) || i.CourseCategory.Contains(search));
            }

            return View(await items.AsNoTracking().ToListAsync());
        }




        public async Task<IActionResult> Details(CourseCategoriesSingleQuery query)
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



        public async Task<IActionResult> Create()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            ViewBag.Tags = new SelectList(db.PostTags, "Id", "Name");
            ViewBag.Teachers = new SelectList(db.Teachers, "Id", "TeacherName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCategoriesCreateCommand command)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teachers = new SelectList(db.Teachers, "Id", "TeacherName");
            ViewBag.Tags = new SelectList(db.PostTags, "Id", "Name");
            return View(command);
        }



        public async Task<IActionResult> Edit(CourseCategoriesSingleQuery query)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }

            var command = new CourseCategoriesEditCommand();
            command.CoursePhoto = entity.CoursePhoto;
            command.CourseName = entity.CourseName;
            command.CourseDetail = entity.CourseDetail;
            command.CourseStructure = entity.CourseStructure;
            command.EntryRequirment = entity.EntryRequirment;
            command.Placements = entity.Placements;
            command.Price = entity.Price;
            command.Lessons = entity.Lessons;
            command.Length = entity.Length;
            command.Level = entity.Level;
            command.CourseCategory = entity.CourseCategory;
            command.CourseTagCloud = entity.CourseTagCloud;
            command.CourseTeachersCloud = entity.CourseTeachers;


            ViewBag.Teachers = new SelectList(db.Teachers, "Id", "TeacherName");
            ViewBag.Tags = new SelectList(db.PostTags, "Id", "Name");
            return View(command);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CourseCategoriesEditCommand command)
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
            ViewBag.Teachers = new SelectList(db.Teachers, "Id", "TeacherName");
            ViewBag.Tags = new SelectList(db.PostTags, "Id", "Name");
            return View(command);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(CourseCategoriesRemoveCommand command)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool CourseCategoriesExists(int id)
        {
            return db.CourseCategories.Any(e => e.Id == id);
        }
    }
}
