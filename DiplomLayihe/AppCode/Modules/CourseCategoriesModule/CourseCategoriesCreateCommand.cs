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

namespace DiplomLayihe.AppCode.Modules.CourseCategoriesModule
{
    public class CourseCategoriesCreateCommand : IRequest<CourseCategories>
    {
        public string CoursePhoto { get; set; }
        public string CourseName { get; set; }
        public int TeacherId { get; set; }
        public string CourseDetail { get; set; }
        public string CourseStructure { get; set; }
        public string EntryRequirment { get; set; }
        public string Placements { get; set; }
        public int Price { get; set; }
        public int Lessons { get; set; }
        public int Length { get; set; }
        public string Level { get; set; }
        public string CourseCategory { get; set; }
        public IFormFile file { get; set; }
        public int[] tagIds { get; set; }
        public int[] teacherIds { get; set; }

        public class CourseCategoriesCreateCommandHandler : IRequestHandler<CourseCategoriesCreateCommand, CourseCategories>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public CourseCategoriesCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<CourseCategories> Handle(CourseCategoriesCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("CoursePhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"courseCategories-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }

                    var courseCategories = new CourseCategories
                    {
                        CoursePhoto = name,
                        CourseName = request.CourseName,
                        CourseDetail = request.CourseDetail,
                        CourseStructure = request.CourseStructure,
                        EntryRequirment = request.EntryRequirment,
                        Placements = request.Placements,
                        Price = request.Price,
                        Lessons = request.Lessons,
                        Length = request.Length,
                        Level = request.Level,
                        CourseCategory = request.CourseCategory,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };


                    await db.CourseCategories.AddAsync(courseCategories, cancellationToken);
                    int affected = await db.SaveChangesAsync(cancellationToken);

                    if (affected > 0 && request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            await db.CourseTagCloud.AddAsync(new CoursePostTag
                            {
                                CoursePostId = courseCategories.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }

                        await db.SaveChangesAsync(cancellationToken);
                    }

                    if (affected > 0 && request.teacherIds != null && request.teacherIds.Length > 0)
                    {
                        foreach (var item in request.teacherIds)
                        {
                            await db.CourseTeachersCloud.AddAsync(new CourseTeachers
                            {
                                CoursePostId = courseCategories.Id,
                                TeachersId = item
                            }, cancellationToken);
                        }

                        await db.SaveChangesAsync(cancellationToken);
                    }

                    return courseCategories;
                }

                return null;
            }
        }
    }
}
