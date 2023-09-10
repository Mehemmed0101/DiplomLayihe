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

namespace DiplomLayihe.AppCode.Modules.EventSpeakersModule
{
    public class EventSpeakersCreateCommand : IRequest<EventSpeakers>
    {
        public string SpeakersPhoto { get; set; }
        public string SpeakersName { get; set; }
        public string SpeakersWork { get; set; }
        public IFormFile file { get; set; }

        public class EventSpeakersCreateCommandHandler : IRequestHandler<EventSpeakersCreateCommand, EventSpeakers>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public EventSpeakersCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<EventSpeakers> Handle(EventSpeakersCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("SpeakersPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"speakers-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }

                    var eventSpeakers = new EventSpeakers
                    {
                        SpeakersPhoto = name,
                        SpeakersName = request.SpeakersName,
                        SpeakersWork = request.SpeakersWork,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };


                    await db.EventSpeakers.AddAsync(eventSpeakers, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    return eventSpeakers;
                }

                return null;
            }
        }
    }
}
