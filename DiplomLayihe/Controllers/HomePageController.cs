using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class HomePageController : Controller
    {
        private readonly DiplomDbContext db;

        public HomePageController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new HomePageViewModel();
            model.CourseCategoriesForHomePage = db.CourseCategoriesForHomePage
                .Where(cp => cp.DeletedById == null).ToList();
            model.CourseCategories = db.CourseCategories
                .Include(cc=>cc.CourseTeachers)
                .ThenInclude(cc => cc.Teachers)
                .Where(cp => cp.DeletedById == null).ToList();
            model.EventGallery = db.EventGallery
                .Where(cp => cp.DeletedById == null).ToList();
            model.Teachers = db.Teachers
                .Where(cp => cp.DeletedById == null).ToList();
            model.PlanAndPricing = db.PlanAndPricing
                .Where(cp => cp.DeletedById == null).ToList();
            model.PlanExplanations = db.PlanExplanations
                .Where(cp => cp.DeletedById == null).ToList();
            model.LastNewsandEvents = db.LastNewsandEvents
                .Where(cp => cp.DeletedById == null).ToList();
            return View(model);
        }
    }
}
