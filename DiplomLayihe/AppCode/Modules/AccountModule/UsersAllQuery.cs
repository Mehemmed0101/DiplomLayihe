using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.AccountModule
{
    public class UsersAllQuery : IRequest<IEnumerable<DiplomUser>>
    {


        public class UsersAllQueryHandler : IRequestHandler<UsersAllQuery, IEnumerable<DiplomUser>>
        {
            private readonly DiplomDbContext db;

            public UsersAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<DiplomUser>> Handle(UsersAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Users.ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
