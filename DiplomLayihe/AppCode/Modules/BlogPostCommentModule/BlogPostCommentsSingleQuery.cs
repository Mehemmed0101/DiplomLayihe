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
    public class BlogPostCommentsSingleQuery : IRequest<BlogPostComments>
    {
        public int Id { get; set; }


        public class BlogPostCommentsSingleQueryHandler : IRequestHandler<BlogPostCommentsSingleQuery, BlogPostComments>
        {
            private readonly DiplomDbContext db;

            public BlogPostCommentsSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<BlogPostComments> Handle(BlogPostCommentsSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.BlogPostComments
                          .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
