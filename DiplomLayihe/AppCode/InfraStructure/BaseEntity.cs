using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Layihe.AppCode.InfraStructure
{
    public class BaseEntity : HistoryEntity
    {
        public int Id { get; set; }

    }

    public class HistoryEntity
    {

        public int? CreatedById { get; set; }
        // [Column(TypeName = "smalldate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
