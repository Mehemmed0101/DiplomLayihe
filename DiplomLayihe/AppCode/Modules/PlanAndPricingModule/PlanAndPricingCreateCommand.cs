using DiplomLayihe.AppCode.Extensions;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.PlanAndPricingModule
{
    public class PlanAndPricingCreateCommand : IRequest<PlanAndPricing>
    {
        public string PlanType { get; set; }
        public int PlanPrice { get; set; }
        public string PlanTime { get; set; }

        public class PlanAndPricingCreateCommandHandler : IRequestHandler<PlanAndPricingCreateCommand, PlanAndPricing>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public PlanAndPricingCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<PlanAndPricing> Handle(PlanAndPricingCreateCommand request, CancellationToken cancellationToken)
            {
                var planAndPricing = new PlanAndPricing();
                planAndPricing.PlanType = request.PlanType;
                planAndPricing.PlanPrice = request.PlanPrice;
                planAndPricing.PlanTime = request.PlanTime;
                planAndPricing.CreatedDate = DateTime.UtcNow.AddHours(4);
                planAndPricing.CreatedById = ctx.GetPrincipalId();


                await db.PlanAndPricing.AddAsync(planAndPricing, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return planAndPricing;
            }
        }
    }
}
