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
    public class ContactSingleQuery : IRequest<ContactUs>
    {
        public int Id { get; set; }


        public class ContactSingleQueryHandler : IRequestHandler<ContactSingleQuery, ContactUs>
        {
            private readonly DiplomDbContext db;

            public ContactSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<ContactUs> Handle(ContactSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.ContactUs
                          .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
