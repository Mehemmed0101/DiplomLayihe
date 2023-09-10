using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomLayihe.Models.Entities;
using System.Threading;
using DiplomLayihe.Models.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DiplomLayihe.AppCode.Modules.CourseCategoriesModule
{
    public class CourseCategoriesAllQuery : IRequest<IEnumerable<CourseCategories>>
    {

        public class CourseCategoriesAllQueryHandler : IRequestHandler<CourseCategoriesAllQuery, IEnumerable<CourseCategories>>
        {
            private readonly DiplomDbContext db;

            public CourseCategoriesAllQueryHandler(DiplomDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<CourseCategories>> Handle(CourseCategoriesAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.CourseCategories
                             .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
