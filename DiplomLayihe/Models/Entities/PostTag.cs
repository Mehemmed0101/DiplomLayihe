using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class PostTag : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<BlogPostTag> TagCloud { get; set; }
        public virtual ICollection<CoursePostTag> CourseTagCloud { get; set; }
        public virtual ICollection<EventPostTag> EventTagCloud { get; set; }
        
    }


    public class BlogPostTag
    {

        public int BlogPostId { get; set; }
        public virtual BlogPosts BlogPost { get; set; }
        public int PostTagId { get; set; }
        public virtual PostTag PostTag { get; set; }
    }

    public class CoursePostTag
    {

        public int CoursePostId { get; set; }
        public virtual CourseCategories CoursePost { get; set; }
        public int PostTagId { get; set; }
        public virtual PostTag PostTag { get; set; }
    }

    public class EventPostTag
    {

        public int EventPostId { get; set; }
        public virtual LastNewsandEvents EventPost { get; set; }
        public int PostTagId { get; set; }
        public virtual PostTag PostTag { get; set; }
    }
}
