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

namespace DiplomLayihe.AppCode.Modules.PlanExplanationModule
{
    public class PlanExplanationCreateCommand : IRequest<PlanExplanations>
    {
        public string Text { get; set; }
        public int PlanId { get; set; }

        public class PlanExplanationCreateCommandHandler : IRequestHandler<PlanExplanationCreateCommand, PlanExplanations>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public PlanExplanationCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<PlanExplanations> Handle(PlanExplanationCreateCommand request, CancellationToken cancellationToken)
            {
                var planExplanation = new PlanExplanations();
                planExplanation.Text = request.Text;
                planExplanation.PlanId = request.PlanId;
                planExplanation.CreatedDate = DateTime.UtcNow.AddHours(4);
                planExplanation.CreatedById = ctx.GetPrincipalId();

                await db.PlanExplanations.AddAsync(planExplanation, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return planExplanation;
            }
        }
    }
}
