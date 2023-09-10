using Back_End_Layihe.AppCode.InfraStructure;
using DiplomLayihe.AppCode.Extensions;
using DiplomLayihe.Models.DataContext;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.BlogPostModule
{
    public class BlogPostRemoveCommand : IJsonRequest
    {
        public int Id { get; set; }


        public class BlogPostRemoveCommandHandler : IJsonRequestHandler<BlogPostRemoveCommand>
        {
            readonly DiplomDbContext db;
            private readonly IActionContextAccessor ctx;

            public BlogPostRemoveCommandHandler(DiplomDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<CommandJsonResponse> Handle(BlogPostRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.BlogPosts
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null);


                if (entity == null)
                {
                    return new CommandJsonResponse(true, "Tapilmadi");
                }
                entity.DeletedById = ctx.GetPrincipalId();
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);

                await db.SaveChangesAsync(cancellationToken);

                return new CommandJsonResponse(false, "Qeyd silindi");
            }
        }
    }
}
