using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class CourseCategoriesForHomePage : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string IconClass { get; set; }
        public string ColorClass { get; set; }
    }
}
