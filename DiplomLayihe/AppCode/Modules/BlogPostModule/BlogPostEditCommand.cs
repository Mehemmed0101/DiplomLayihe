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

namespace DiplomLayihe.AppCode.Modules.BlogPostModule
{
    public class BlogPostEditCommand : IRequest<BlogPosts>
    {
        public int Id { get; set; }
        public string BlogPhoto { get; set; }
        public string Title { get; set; }
        public string p1 { get; set; }
        public string p2 { get; set; }
        public string SpecialText { get; set; }
        public IFormFile file { get; set; }
        public int[] tagIds { get; set; }
        public ICollection<BlogPostTag> TagCloud { get; set; }


        public class BlogPosEditCommandHandler : IRequestHandler<BlogPostEditCommand, BlogPosts>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public BlogPosEditCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }

            public async Task<BlogPosts> Handle(BlogPostEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.BlogPhoto))
                {
                    ctx.AddModelError("BlogPhoto", "Fayl Sechilmeyib!");
                }


                if (ctx.ModelIsValid())
                {
                    var currentEntity = db.BlogPosts
                       .Include(bp=>bp.TagCloud)
                       .FirstOrDefault(bp => bp.Id == request.Id);


                    if (currentEntity == null)
                    {
                        return null;
                    }

                    string OldFileName = currentEntity.BlogPhoto;

                    if (request.file != null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"blog-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }
                        currentEntity.BlogPhoto = name;

                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }


                    currentEntity.Title = request.Title;
                    currentEntity.p1 = request.p1;
                    currentEntity.p2 = request.p2;
                    currentEntity.SpecialText = request.SpecialText;
                    currentEntity.TagCloud = request.TagCloud;

                    await db.SaveChangesAsync(cancellationToken);

                    if (request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            if (db.BlogPostTagCloud.Any(bptc => bptc.PostTagId == item && bptc.BlogPostId == request.Id))
                            {
                                continue;
                            }

                            await db.BlogPostTagCloud.AddAsync(new BlogPostTag
                            {
                                BlogPostId = request.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }
                        await db.SaveChangesAsync(cancellationToken);
                    }

                    return currentEntity;
                }

                return null;
            }
        }
    }
}
