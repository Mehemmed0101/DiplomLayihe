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

namespace DiplomLayihe.AppCode.Modules.TeachersModule
{
    public class TeachersCreateCommand : IRequest<Teachers>
    {
        public string TeacherPhoto { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public int TeacherNumber { get; set; }
        public string AboutTeacher { get; set; }
        public string TeacherLevel { get; set; }
        public IFormFile file { get; set; }



        public class TeachersCreateCommandHandler : IRequestHandler<TeachersCreateCommand, Teachers>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public TeachersCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Teachers> Handle(TeachersCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("TeacherPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"teachers-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }

                    var teachers = new Teachers
                    {
                        TeacherPhoto = name,
                        TeacherName = request.TeacherName,
                        TeacherEmail = request.TeacherEmail,
                        TeacherNumber = request.TeacherNumber,
                        AboutTeacher = request.AboutTeacher,
                        TeacherLevel = request.TeacherLevel,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };


                    await db.Teachers.AddAsync(teachers, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    return teachers;
                }

                return null;
            }
        }
    }
}
