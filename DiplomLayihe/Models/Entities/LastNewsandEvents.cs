using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class LastNewsandEvents : BaseEntity
    {
        public string EventPhoto { get; set; }
        public string Title { get; set; }
        public string MainText { get; set; }
        public string Explanation { get; set; }
        public virtual ICollection<EventPostTag> EventTagCloud { get; set; }
    }
}
