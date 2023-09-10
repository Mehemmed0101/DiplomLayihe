using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.EventSpeakersModule
{
    public class EventSpeakersAllQuery : IRequest<IEnumerable<EventSpeakers>>
    {

        public class EventSpeakersAllQueryHandler : IRequestHandler<EventSpeakersAllQuery, IEnumerable<EventSpeakers>>
        {
            private readonly DiplomDbContext db;

            public EventSpeakersAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<EventSpeakers>> Handle(EventSpeakersAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.EventSpeakers
                            .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
