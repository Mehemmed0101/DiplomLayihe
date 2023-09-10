using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class CourseController : Controller
    {
        private readonly DiplomDbContext db;
        public CourseController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = db.CourseCategories
                .Where(cc => cc.DeletedById == null).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["Getsearch"] = search;

            var items = from x in db.CourseCategories.Where(cc => cc.DeletedById == null) select x;

            if (!String.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.CourseName.Contains(search) || i.CourseCategory.Contains(search));
            }

            return View(await items.AsNoTracking().ToListAsync());
        }


        public IActionResult Coursesingle(int id)
        {
            var model = new CourseViewModel();
            model.CourseCategories = db.CourseCategories
               .Include(bp => bp.CourseTagCloud)
               .ThenInclude(bp => bp.PostTag)
               .Include(cc => cc.CourseTeachers)
               .ThenInclude(cc => cc.Teachers)
               .Where(cp => cp.DeletedById == null && cp.Id == id).ToList();

            model.EventGallery = db.EventGallery
               .Where(cp => cp.DeletedById == null && cp.Id == id).ToList();

            //ViewBag.eventgallery = new SelectList(db.EventGallery, "Id", "GalleryPhoto");
            return View(model);
        }
    }
}
