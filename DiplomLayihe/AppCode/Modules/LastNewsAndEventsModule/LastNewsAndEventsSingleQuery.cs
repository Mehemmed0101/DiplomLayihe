using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.LastNewsAndEventsModule
{
    public class LastNewsAndEventsSingleQuery : IRequest<LastNewsandEvents>
    {
        public int Id { get; set; }

        public class LastNewsAndEventsSingleQueryHandler : IRequestHandler<LastNewsAndEventsSingleQuery, LastNewsandEvents>
        {
            private readonly DiplomDbContext db;

            public LastNewsAndEventsSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<LastNewsandEvents> Handle(LastNewsAndEventsSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.LastNewsandEvents
                      .Include(bp => bp.EventTagCloud)
                      .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
