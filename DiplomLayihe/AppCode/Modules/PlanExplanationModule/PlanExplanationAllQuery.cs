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
    public class PlanExplanationAllQuery : IRequest<IEnumerable<PlanExplanations>>
    {

        public class PlanExplanationAllQueryHandler : IRequestHandler<PlanExplanationAllQuery, IEnumerable<PlanExplanations>>
        {
            private readonly DiplomDbContext db;

            public PlanExplanationAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<PlanExplanations>> Handle(PlanExplanationAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.PlanExplanations
                    .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
