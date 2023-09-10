using DiplomLayihe.Models.Entities.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.DataContext
{
    public static class DiplomDbSeed
    {

        static internal IApplicationBuilder InitMembership(this IApplicationBuilder app)
        {
            using(IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<DiplomUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<DiplomRole>>();
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                //Superadmin
                var user = userManager.FindByEmailAsync(configuration["membership:email"]).Result;

                if (user == null)
                {
                    user = new DiplomUser
                    {
                        Name = configuration["membership:name"],
                        Surname = configuration["membership:surname"],
                        Email = configuration["membership:email"],
                        UserName = configuration["membership:username"],
                        EmailConfirmed = true
                    };


                    var identityResult = userManager.CreateAsync(user, configuration["membership:password"]).Result;

                    if (!identityResult.Succeeded)
                        return app;

                }



                var roLes = configuration["membership:roles"].Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var roleName in roLes)
                {
                    var role = roleManager.FindByNameAsync(roleName).Result;

                    if (role == null)
                    {
                        role = new DiplomRole
                        {
                            Name = roleName
                        };
                        var roleResult = roleManager.CreateAsync(role).Result;


                        if (roleResult.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, roleName).Wait();
                        }
                    }
                    else if (!userManager.IsInRoleAsync(user, roleName).Result)
                    {
                        userManager.AddToRoleAsync(user, roleName).Wait();
                    }
                }
            }

            return app;
        }

    }
}
