using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.ContactPostModule
{
    public class ContactNonAnsweredQuery : IRequest<IEnumerable<ContactUs>>
    {
        public class ContactNonAnsweredQueryHandler : IRequestHandler<ContactNonAnsweredQuery, IEnumerable<ContactUs>>
        {
            private readonly DiplomDbContext db;

            public ContactNonAnsweredQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<ContactUs>> Handle(ContactNonAnsweredQuery request, CancellationToken cancellationToken)
            {
                var model = await db.ContactUs
                               .Where(ah => ah.DeletedById == null && ah.AnswerById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
