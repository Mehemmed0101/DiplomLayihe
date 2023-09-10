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

namespace DiplomLayihe.AppCode.Modules.LastNewsAndEventsModule
{
    public class LastNewsAndEventsEditCommand : IRequest<LastNewsandEvents>
    {
        public int Id { get; set; }
        public string EventPhoto { get; set; }
        public string Title { get; set; }
        public string MainText { get; set; }
        public string Explanation { get; set; }
        public IFormFile file { get; set; }
        public int[] tagIds { get; set; }
        public ICollection<EventPostTag> EventTagCloud { get; set; }


        public class LastNewsAndEventsEditCommandHandler : IRequestHandler<LastNewsAndEventsEditCommand, LastNewsandEvents>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public LastNewsAndEventsEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<LastNewsandEvents> Handle(LastNewsAndEventsEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.EventPhoto))
                {
                    ctx.AddModelError("EventPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    var entity = await db.LastNewsandEvents
                       .Include(bp => bp.EventTagCloud)
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    string OldFileName = entity.EventPhoto;


                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"event-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        entity.EventPhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }

                    entity.Title = request.Title;
                    entity.MainText = request.MainText;
                    entity.Explanation = request.Explanation;
                    entity.EventTagCloud = request.EventTagCloud;
                    await db.SaveChangesAsync(cancellationToken);

                    if (request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            if (db.EventTagCloud.Any(bptc => bptc.PostTagId == item && bptc.EventPostId == request.Id))
                            {
                                continue;
                            }

                            await db.EventTagCloud.AddAsync(new EventPostTag
                            {
                                EventPostId = request.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }
                        await db.SaveChangesAsync(cancellationToken);
                    }

                    return entity;
                }

                return null;
            }
        }
    }
}
