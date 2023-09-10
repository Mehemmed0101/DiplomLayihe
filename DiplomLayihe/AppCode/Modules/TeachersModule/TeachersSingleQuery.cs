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
    public class TeachersSingleQuery : IRequest<Teachers>
    {
        public int Id { get; set; }


        public class TeachersSingleQueryHandler : IRequestHandler<TeachersSingleQuery, Teachers>
        {
            private readonly DiplomDbContext db;

            public TeachersSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<Teachers> Handle(TeachersSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Teachers
                      .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
