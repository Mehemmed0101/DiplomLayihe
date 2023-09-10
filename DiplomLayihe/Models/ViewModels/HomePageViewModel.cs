using DiplomLayihe.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<CourseCategoriesForHomePage> CourseCategoriesForHomePage { get; set; }
        public List<CourseCategories> CourseCategories { get; set; }
        public List<EventGallery> EventGallery { get; set; }
        public List<Teachers> Teachers { get; set; }
        public List<PlanExplanations> PlanExplanations { get; set; }
        public List<LastNewsandEvents> LastNewsandEvents { get; set; }
        public List<PlanAndPricing> PlanAndPricing { get; set; }

    }
}
