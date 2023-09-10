using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class BlogPostComments : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
        public int ParentId { get; set; }
        public int? ReplyId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }



        public virtual BlogPostComments Parent { get; set; }
        public virtual ICollection<BlogPostComments> Children { get; set; }
    }
}
