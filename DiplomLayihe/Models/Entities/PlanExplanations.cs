using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class PlanExplanations : BaseEntity
    {
        public string Text { get; set; }
        [ForeignKey("ParentId")]
        public int PlanId { get; set; }
        public virtual ICollection<PlanAndPricing> PlanAndPricing { get; set; }
    }
}
