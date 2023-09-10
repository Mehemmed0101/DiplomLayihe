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
    public class PlanAndPricingSingleQuery : IRequest<PlanAndPricing>
    {
        public int Id { get; set; }


        public class PlanAndPricingSingleQueryHandler : IRequestHandler<PlanAndPricingSingleQuery, PlanAndPricing>
        {
            private readonly DiplomDbContext db;

            public PlanAndPricingSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<PlanAndPricing> Handle(PlanAndPricingSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.PlanAndPricing
                     .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
