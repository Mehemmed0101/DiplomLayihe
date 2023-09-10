using DiplomLayihe.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.ViewModels
{
    public class CourseViewModel
    {
        public List<CourseCategories> CourseCategories { get; set; }
        public List<EventGallery> EventGallery { get; set; }
    }
}
