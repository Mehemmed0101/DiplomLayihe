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
    public class AboutUsController : Controller
    {
        private readonly DiplomDbContext db;
        public AboutUsController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = db.Teachers
                .Where(t => t.DeletedById == null).ToList();

            return View(model);
        }
    }
}
