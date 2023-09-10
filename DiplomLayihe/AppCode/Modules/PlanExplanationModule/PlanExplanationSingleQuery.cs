using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.PlanExplanationModule
{
    public class PlanExplanationSingleQuery : IRequest<PlanExplanations>
    {
        public int Id { get; set; }

        public class PlanExplanationSingleQueryHandler : IRequestHandler<PlanExplanationSingleQuery, PlanExplanations>
        {
            private readonly DiplomDbContext db;

            public PlanExplanationSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<PlanExplanations> Handle(PlanExplanationSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.PlanExplanations
                      .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
