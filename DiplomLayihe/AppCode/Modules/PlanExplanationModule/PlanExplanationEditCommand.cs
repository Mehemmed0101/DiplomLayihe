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
    public class PlanExplanationEditCommand : IRequest<PlanExplanations>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PlanId { get; set; }

        public class PlanExplanationEditCommandHandler : IRequestHandler<PlanExplanationEditCommand, PlanExplanations>
        {
            private readonly DiplomDbContext db;

            public PlanExplanationEditCommandHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<PlanExplanations> Handle(PlanExplanationEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.PlanExplanations
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.Text = request.Text;
                entity.PlanId = request.PlanId;
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
