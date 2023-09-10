using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.BlogPostModule
{
    public class BlogPostSingleQuery : IRequest<BlogPosts>
    {

        public int Id { get; set; }


        public class BlogPostSingleQueryHandler : IRequestHandler<BlogPostSingleQuery, BlogPosts>
        {
            private readonly DiplomDbContext db;

            public BlogPostSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<BlogPosts> Handle(BlogPostSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.BlogPosts
               .Include(bp => bp.TagCloud)
               .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
