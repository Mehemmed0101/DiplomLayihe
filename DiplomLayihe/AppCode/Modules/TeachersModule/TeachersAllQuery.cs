using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.TeachersModule
{
    public class TeachersAllQuery : IRequest<IEnumerable<Teachers>>
    {
        public class TeachersAllQueryHandler : IRequestHandler<TeachersAllQuery, IEnumerable<Teachers>>
        {
            private readonly DiplomDbContext db;

            public TeachersAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Teachers>> Handle(TeachersAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Teachers
                             .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
