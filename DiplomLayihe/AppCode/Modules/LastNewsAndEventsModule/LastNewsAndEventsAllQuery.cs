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
    public class LastNewsAndEventsAllQuery : IRequest<IEnumerable<LastNewsandEvents>>
    {
        public class LastNewsAndEventsAllQueryHandler : IRequestHandler<LastNewsAndEventsAllQuery, IEnumerable<LastNewsandEvents>>
        {
            private readonly DiplomDbContext db;

            public LastNewsAndEventsAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<LastNewsandEvents>> Handle(LastNewsAndEventsAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.LastNewsandEvents
                             .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
