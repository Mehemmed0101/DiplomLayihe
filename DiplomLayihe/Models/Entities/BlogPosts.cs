using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class BlogPosts : BaseEntity
    {
        public string BlogPhoto { get; set; }
        public string Title { get; set; }
        public string p1 { get; set; }
        public string p2 { get; set; }
        public string SpecialText { get; set; }

        public virtual ICollection<BlogPostComments> Comments { get; set; }


        public virtual ICollection<BlogPostTag> TagCloud { get; set; }
    }
}
