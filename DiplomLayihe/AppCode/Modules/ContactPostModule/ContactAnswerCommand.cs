using DiplomLayihe.AppCode.Extensions;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.ContactPostModule
{
    public class ContactAnswerCommand : IRequest<ContactUs>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Answer { get; set; }


        public class ContactAnswerCommandHandler : IRequestHandler<ContactAnswerCommand, ContactUs>
        {
            private readonly DiplomDbContext db;
            private readonly IActionContextAccessor ctx;

            public ContactAnswerCommandHandler(DiplomDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<ContactUs> Handle(ContactAnswerCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.ContactUs
                          .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.Answer = request.Answer;

                entity.AnswerById = ctx.GetPrincipalId();
                entity.AnswerDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
