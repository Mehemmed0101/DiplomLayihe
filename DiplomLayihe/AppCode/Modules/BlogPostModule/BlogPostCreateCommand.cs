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

namespace DiplomLayihe.AppCode.Modules.BlogPostModule
{
    public class BlogPostCreateCommand : IRequest<BlogPosts>
    {
        public string BlogPhoto { get; set; }
        public string Title { get; set; }
        public string p1 { get; set; }
        public string p2 { get; set; }
        public string SpecialText { get; set; }
        public IFormFile file { get; set; }
        public int[] tagIds { get; set; }


        public class BlogPostCreateCommandHandler : IRequestHandler<BlogPostCreateCommand, BlogPosts>
        {
            private readonly DiplomDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public BlogPostCreateCommandHandler(DiplomDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<BlogPosts> Handle(BlogPostCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.file == null)
                {
                    ctx.AddModelError("BlogPhoto", "Fayl Sechilmeyib!");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.file.FileName);

                    string name = $"blog-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.file.CopyToAsync(fs, cancellationToken);
                    }


                    var blogs = new BlogPosts
                    {
                        BlogPhoto = name,
                        Title = request.Title,
                        p1 = request.p1,
                        p2 = request.p2,
                        SpecialText = request.SpecialText,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };

                    await db.AddAsync(blogs, cancellationToken);
                    int affected = await db.SaveChangesAsync(cancellationToken);


                    if (affected > 0 && request.tagIds != null && request.tagIds.Length > 0)
                    {
                        foreach (var item in request.tagIds)
                        {
                            await db.BlogPostTagCloud.AddAsync(new BlogPostTag
                            {
                                BlogPostId = blogs.Id,
                                PostTagId = item
                            }, cancellationToken);
                        }

                        await db.SaveChangesAsync(cancellationToken);
                    }

                    return blogs;
                }

                return null;
            }
        }
    }
}
