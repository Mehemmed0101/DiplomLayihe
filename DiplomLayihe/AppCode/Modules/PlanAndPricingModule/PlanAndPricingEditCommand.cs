using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.PlanAndPricingModule
{
    public class PlanAndPricingEditCommand : IRequest<PlanAndPricing>
    {
        public int Id { get; set; }
        public string PlanType { get; set; }
        public int PlanPrice { get; set; }
        public string PlanTime { get; set; }

        public class PlanAndPricingEditCommandHandler : IRequestHandler<PlanAndPricingEditCommand, PlanAndPricing>
        {
            private readonly DiplomDbContext db;

            public PlanAndPricingEditCommandHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<PlanAndPricing> Handle(PlanAndPricingEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.PlanAndPricing
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.PlanType = request.PlanType;
                entity.PlanPrice = request.PlanPrice;
                entity.PlanTime = request.PlanTime;
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
