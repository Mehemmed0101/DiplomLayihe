using DiplomLayihe.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.ViewModels
{
    public class EventViewModel
    {
        public List<EventSpeakers> EventSpeakers { get; set; }
        public List<EventGallery> EventGallery { get; set; }
        public List<LastNewsandEvents> LastNewsandEvents { get; set; }
    }
}
