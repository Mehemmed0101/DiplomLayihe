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
    public class EventGallerySingleQuery : IRequest<EventGallery>
    {
        public int Id { get; set; }

        public class EventGallerySingleQueryHandler : IRequestHandler<EventGallerySingleQuery, EventGallery>
        {
            private readonly DiplomDbContext db;

            public EventGallerySingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<EventGallery> Handle(EventGallerySingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.EventGallery
                     .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
