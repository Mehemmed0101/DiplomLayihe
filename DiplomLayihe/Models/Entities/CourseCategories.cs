using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class CourseCategories : BaseEntity
    {
        public string CoursePhoto { get; set; }
        public string CourseName { get; set; }
        public int TeacherId { get; set; }
        public string CourseDetail { get; set; }
        public string CourseStructure { get; set; }
        public string EntryRequirment { get; set; }
        public string Placements { get; set; }
        public int Price { get; set; }
        public int Lessons { get; set; }
        public int Length { get; set; }
        public string Level { get; set; }
        public string CourseCategory { get; set; }


        public virtual ICollection<CourseTeachers> CourseTeachers { get; set; }
        public virtual ICollection<CoursePostTag> CourseTagCloud { get; set; }
    }   
}
