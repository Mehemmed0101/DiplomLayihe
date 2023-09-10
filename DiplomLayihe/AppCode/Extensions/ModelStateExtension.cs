using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCodee.Extensions
{
    public static partial class Extension
    {
        public static IActionContextAccessor AddModelError(this IActionContextAccessor ctx, string key, string message)
        {
            ctx.ActionContext.ModelState.AddModelError(key, message);
            return ctx;
        }
        public static bool ModelIsValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }

    }
}
