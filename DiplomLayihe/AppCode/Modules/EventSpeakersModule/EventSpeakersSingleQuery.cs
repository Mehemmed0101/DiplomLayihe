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
    public class EventSpeakersSingleQuery : IRequest<EventSpeakers>
    {
        public int Id { get; set; }

        public class EventSpeakersSingleQueryHandler : IRequestHandler<EventSpeakersSingleQuery, EventSpeakers>
        {
            private readonly DiplomDbContext db;

            public EventSpeakersSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<EventSpeakers> Handle(EventSpeakersSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.EventSpeakers
                      .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
