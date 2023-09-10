using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class Teachers : BaseEntity
    {
        public string TeacherPhoto { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public int TeacherNumber { get; set; }
        public string AboutTeacher { get; set; }
        public string TeacherLevel { get; set; }

        public virtual ICollection<CourseTeachers> CourseTeachers { get; set; }
    }

    public class CourseTeachers
    {

        public int CoursePostId { get; set; }
        public virtual CourseCategories CoursePost { get; set; }
        public int TeachersId { get; set; }
        public virtual Teachers Teachers { get; set; }
    }
}
