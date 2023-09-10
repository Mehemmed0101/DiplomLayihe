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

namespace DiplomLayihe.AppCode.Modules.CourseCategoriesModule
{
    public class CourseCategoriesEditCommand : IRequest<CourseCategories>
    {
        public int Id { get; set; }
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
        public ICollection<CoursePostTag> CourseTagCloud { get; set; }
        public int[] teachersIds { get; set; }
        public ICollection<CourseTeachers> CourseTeachersCloud { get; set; }


        public class CourseCategoriesEditCommandHandler : IRequestHandler<CourseCategoriesEditCommand, CourseCategories>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public CourseCategoriesEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<CourseCategories> Handle(CourseCategoriesEditCommand request, CancellationToken cancellationToken)
            {

                if (request.file == null && string.IsNullOrEmpty(request.CoursePhoto))
                {
                    ctx.AddModelError("PhotoPath", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    var entity = await db.CourseCategories
                       .Include(bp => bp.CourseTagCloud)
                       .Include(bp => bp.CourseTeachers)
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    string OldFileName = entity.CoursePhoto;


                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"courseCategories-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        entity.CoursePhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }

                    entity.CourseName = request.CourseName;
                    entity.CourseDetail = request.CourseDetail;
                    entity.CourseStructure = request.CourseStructure;
                    entity.EntryRequirment = request.EntryRequirment;
                    entity.Placements = request.Placements;
                    entity.Price = request.Price;
                    entity.Lessons = request.Lessons;
                    entity.Length = request.Length;
                    entity.Level = request.Level;
                    entity.CourseCategory = request.CourseCategory;
                    entity.CourseTagCloud = request.CourseTagCloud;
                    entity.CourseTeachers = request.CourseTeachersCloud;

                    await db.SaveChangesAsync(cancellationToken);


                    if (request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            if (db.CourseTagCloud.Any(bptc => bptc.PostTagId == item && bptc.CoursePostId == request.Id))
                            {
                                continue;
                            }

                            await db.CourseTagCloud.AddAsync(new CoursePostTag
                            {
                                CoursePostId = request.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }
                        await db.SaveChangesAsync(cancellationToken);
                    }


                    if (request.teachersIds != null && request.teachersIds.Length > 0)
                    {
                        foreach (var item in request.teachersIds)
                        {
                            if (db.CourseTeachersCloud.Any(bptc => bptc.TeachersId == item && bptc.CoursePostId == request.Id))
                            {
                                continue;
                            }

                            await db.CourseTeachersCloud.AddAsync(new CourseTeachers
                            {
                                CoursePostId = request.Id,
                                TeachersId = item
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
