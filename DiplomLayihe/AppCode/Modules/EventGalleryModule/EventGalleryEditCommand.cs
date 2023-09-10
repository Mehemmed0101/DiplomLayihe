using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DiplomLayihe.AppCodee.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.EventGalleryModule
{
    public class EventGalleryEditCommand : IRequest<EventGallery>
    {
        public int Id { get; set; }
        public string GalleryPhoto { get; set; }
        public IFormFile file { get; set; }


        public class EventGalleryEditCommandHandler : IRequestHandler<EventGalleryEditCommand, EventGallery>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public EventGalleryEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<EventGallery> Handle(EventGalleryEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.GalleryPhoto))
                {
                    ctx.AddModelError("GalleryPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    var entity = await db.EventGallery
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    string OldFileName = entity.GalleryPhoto;


                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"gallery-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        entity.GalleryPhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }

                    await db.SaveChangesAsync(cancellationToken);

                    return entity;
                }

                return null;
            }
        }
    }
}
