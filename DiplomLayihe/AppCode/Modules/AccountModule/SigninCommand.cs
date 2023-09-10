using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DiplomLayihe.AppCodee.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCode.Modules.AccountModule
{
    public class SigninCommand : IRequest<DiplomUser>
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public class SigninCommandHandler : IRequestHandler<SigninCommand, DiplomUser>
        {
            private readonly UserManager<DiplomUser> userManager;
            private readonly SignInManager<DiplomUser> signInManager;
            private readonly IActionContextAccessor ctx;
            private readonly DiplomDbContext db;

            public SigninCommandHandler(UserManager<DiplomUser> userManager,
                SignInManager<DiplomUser> signInManager,
                IActionContextAccessor ctx,
                DiplomDbContext db)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.ctx = ctx;
                this.db = db;
            }
            public async Task<DiplomUser> Handle(SigninCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.ModelIsValid())
                {
                    return null;
                }

                var user = await userManager.FindByNameAsync(request.Username);

                if (user == null)
                {
                    ctx.AddModelError("UserName", "Istifadeci adi ve ya shifre sehvdir!");
                    return null;
                }


                var signinResult = await signInManager.PasswordSignInAsync(user, request.Password, true, false);

                if (signinResult.IsLockedOut)
                {
                    ctx.AddModelError("UserName", "Hesabiniz muveqqeti olaraq bloklanib");
                    return null;
                }
                else if (signinResult.IsNotAllowed)
                {
                    ctx.AddModelError("UserName", "Girish huququnuz yoxdur!");
                    return null;
                }


                if (!signinResult.Succeeded)
                {
                    ctx.AddModelError("UserName", "Istifadeci adi ve ya shifre yanlishdir!");
                    return null;
                }


                return user;
            }
        }
    }
}
