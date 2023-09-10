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
    public class PlanAndPricingAllQuery : IRequest<IEnumerable<PlanAndPricing>>
    {
        public class PlanAndPricingAllQueryHandler : IRequestHandler<PlanAndPricingAllQuery, IEnumerable<PlanAndPricing>>
        {
            private readonly DiplomDbContext db;

            public PlanAndPricingAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<PlanAndPricing>> Handle(PlanAndPricingAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.PlanAndPricing
                            .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
