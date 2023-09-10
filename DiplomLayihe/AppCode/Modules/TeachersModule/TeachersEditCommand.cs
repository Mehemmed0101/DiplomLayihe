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

namespace DiplomLayihe.AppCode.Modules.TeachersModule
{
    public class TeachersEditCommand : IRequest<Teachers>
    {
        public int Id { get; set; }
        public string TeacherPhoto { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public int TeacherNumber { get; set; }
        public string AboutTeacher { get; set; }
        public string TeacherLevel { get; set; }
        public IFormFile file { get; set; }

        public class TeachersEditCommandHandler : IRequestHandler<TeachersEditCommand, Teachers>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public TeachersEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Teachers> Handle(TeachersEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.TeacherPhoto))
                {
                    ctx.AddModelError("TeacherPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    var entity = await db.Teachers
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    string OldFileName = entity.TeacherPhoto;


                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"teachers-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        entity.TeacherPhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }

                    entity.TeacherName = request.TeacherName;
                    entity.TeacherNumber = request.TeacherNumber;
                    entity.TeacherEmail = request.TeacherEmail;
                    entity.TeacherLevel = request.TeacherLevel;
                    entity.AboutTeacher = request.AboutTeacher;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity;
                }

                return null;
            }
        }
    }
}
