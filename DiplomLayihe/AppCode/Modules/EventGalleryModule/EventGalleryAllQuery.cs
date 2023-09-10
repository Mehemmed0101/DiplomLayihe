using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.EventGalleryModule
{
    public class EventGalleryAllQuery : IRequest<IEnumerable<EventGallery>>
    {

        public class EventGalleryAllQueryHandler : IRequestHandler<EventGalleryAllQuery, IEnumerable<EventGallery>>
        {
            private readonly DiplomDbContext db;

            public EventGalleryAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<EventGallery>> Handle(EventGalleryAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.EventGallery
                            .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
