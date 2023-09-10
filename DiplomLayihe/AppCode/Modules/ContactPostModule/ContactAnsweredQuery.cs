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
    public class ContactAnsweredQuery : IRequest<IEnumerable<ContactUs>>
    {

        public class ContactAnsweredQueryHandler : IRequestHandler<ContactAnsweredQuery, IEnumerable<ContactUs>>
        {
            private readonly DiplomDbContext db;

            public ContactAnsweredQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<ContactUs>> Handle(ContactAnsweredQuery request, CancellationToken cancellationToken)
            {
                var model = await db.ContactUs
                            .Where(ah => ah.DeletedById == null && ah.AnswerById != null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
