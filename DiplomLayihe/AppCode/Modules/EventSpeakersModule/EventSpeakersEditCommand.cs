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

namespace DiplomLayihe.AppCode.Modules.EventSpeakersModule
{
    public class EventSpeakersEditCommand : IRequest<EventSpeakers>
    {
        public int Id { get; set; }
        public string SpeakersPhoto { get; set; }
        public string SpeakersName { get; set; }
        public string SpeakersWork { get; set; }
        public IFormFile file { get; set; }

        public class EventSpeakersEditCommandHandler : IRequestHandler<EventSpeakersEditCommand, EventSpeakers>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public EventSpeakersEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<EventSpeakers> Handle(EventSpeakersEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.SpeakersPhoto))
                {
                    ctx.AddModelError("SpeakersPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    var entity = await db.EventSpeakers
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    string OldFileName = entity.SpeakersPhoto;


                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"speakers-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        entity.SpeakersPhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }

                    entity.SpeakersName = request.SpeakersName;
                    entity.SpeakersWork = request.SpeakersWork;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity;
                }

                return null;
            }
        }
    }
}
