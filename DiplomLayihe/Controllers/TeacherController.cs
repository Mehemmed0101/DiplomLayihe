using DiplomLayihe.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class TeacherController : Controller
    {
        private readonly DiplomDbContext db;
        public TeacherController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = db.Teachers
                .Where(t => t.DeletedById == null);
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var model = db.Teachers
                .FirstOrDefault(t=>t.Id == id && t.DeletedById == null);
            return View(model);
        }
    }
}
