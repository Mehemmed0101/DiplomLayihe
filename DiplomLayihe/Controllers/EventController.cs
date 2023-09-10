using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Controllers
{
    [AllowAnonymous]
    public class EventController : Controller
    {
        private readonly DiplomDbContext db;

        public EventController(DiplomDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = db.LastNewsandEvents
                .Where(lne => lne.DeletedById == null);
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var model = new EventViewModel();
            model.LastNewsandEvents = db.LastNewsandEvents
                .Where(lna => lna.DeletedById == null && lna.Id == id).ToList();
            model.EventGallery = db.EventGallery
                .Where(eg => eg.DeletedById == null).ToList();
            model.EventSpeakers = db.EventSpeakers
                .Where(es => es.DeletedById == null).ToList();

            return View(model);
        }
    }
}
