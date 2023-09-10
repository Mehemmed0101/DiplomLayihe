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
    public class BlogPostAllQuery : IRequest<IEnumerable<BlogPosts>>
    {


        public class BlogPostAllQueryHandler : IRequestHandler<BlogPostAllQuery, IEnumerable<BlogPosts>>
        {
            private readonly DiplomDbContext db;

            public BlogPostAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<BlogPosts>> Handle(BlogPostAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.BlogPosts
                             .Where(bp => bp.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
