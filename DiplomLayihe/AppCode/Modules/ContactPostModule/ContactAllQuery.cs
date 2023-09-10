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
    public class ContactAllQuery : IRequest<IEnumerable<ContactUs>>
    {
        public class ContactAllQueryHandler : IRequestHandler<ContactAllQuery, IEnumerable<ContactUs>>
        {
            private readonly DiplomDbContext db;

            public ContactAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<ContactUs>> Handle(ContactAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.ContactUs
                         .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
