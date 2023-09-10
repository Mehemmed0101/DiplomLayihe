using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.CourseCategoriesModule
{
    public class CourseCategoriesSingleQuery : IRequest<CourseCategories>
    {
        public int Id { get; set; }

        public class CourseCategoriesSingleQueryHandler : IRequestHandler<CourseCategoriesSingleQuery, CourseCategories>
        {
            private readonly DiplomDbContext db;

            public CourseCategoriesSingleQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<CourseCategories> Handle(CourseCategoriesSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.CourseCategories
                        .Include(cc=>cc.CourseTagCloud)
                        .Include(cc=>cc.CourseTeachers)
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
