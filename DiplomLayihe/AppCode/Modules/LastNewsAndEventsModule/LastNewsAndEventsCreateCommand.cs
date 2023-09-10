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

namespace DiplomLayihe.AppCode.Modules.LastNewsAndEventsModule
{
    public class LastNewsAndEventsCreateCommand : IRequest<LastNewsandEvents>
    {
        public string EventPhoto { get; set; }
        public string Title { get; set; }
        public string MainText { get; set; }
        public string Explanation { get; set; }
        public IFormFile file { get; set; }
        public int[] tagIds { get; set; }

        public class LastNewsAndEventsCreateCommandHandler : IRequestHandler<LastNewsAndEventsCreateCommand, LastNewsandEvents>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public LastNewsAndEventsCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<LastNewsandEvents> Handle(LastNewsAndEventsCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("EventPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"event-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }

                    var lastNewsandEvents = new LastNewsandEvents
                    {
                        EventPhoto = name,
                        Title = request.Title,
                        MainText = request.MainText,
                        Explanation = request.Explanation,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };


                    await db.LastNewsandEvents.AddAsync(lastNewsandEvents, cancellationToken);
                    int affected = await db.SaveChangesAsync(cancellationToken);

                    if (affected > 0 && request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            await db.EventTagCloud.AddAsync(new EventPostTag
                            {
                                EventPostId = lastNewsandEvents.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }

                        await db.SaveChangesAsync(cancellationToken);
                    }

                    return lastNewsandEvents;
                }

                return null;
            }
        }
    }
}
