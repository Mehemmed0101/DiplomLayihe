using DiplomLayihe.AppCode.Extensions;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DiplomLayihe.AppCodee.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.EventGalleryModule
{
    public class EventGalleryCreateCommand : IRequest<EventGallery>
    {
        public string GalleryPhoto { get; set; }
        public IFormFile file { get; set; }

        public class EventGalleryCreateCommandHandler : IRequestHandler<EventGalleryCreateCommand, EventGallery>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public EventGalleryCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<EventGallery> Handle(EventGalleryCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("GalleryPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"gallery-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }

                    var eventGallery = new EventGallery
                    {
                        GalleryPhoto = name,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };


                    await db.EventGallery.AddAsync(eventGallery, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    return eventGallery;
                }

                return null;
            }
        }
    }
}
