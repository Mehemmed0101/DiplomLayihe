using Back_End_Layihe.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.Entities
{
    public class PlanAndPricing : BaseEntity
    {
        public string PlanType { get; set; }
        public int PlanPrice { get; set; }
        public string PlanTime { get; set; }

        public virtual ICollection<PlanExplanations> PlanExplanations { get; set; }
    }
}
