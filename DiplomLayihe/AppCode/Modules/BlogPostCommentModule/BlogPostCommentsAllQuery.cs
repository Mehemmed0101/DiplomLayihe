using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.BlogPostCommentModule
{
    public class BlogPostCommentsAllQuery : IRequest<IEnumerable<BlogPostComments>>
    {

        public class BlogPostCommentsAllQueryHandler : IRequestHandler<BlogPostCommentsAllQuery, IEnumerable<BlogPostComments>>
        {
            private readonly DiplomDbContext db;

            public BlogPostCommentsAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<BlogPostComments>> Handle(BlogPostCommentsAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.BlogPostComments
                         .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
