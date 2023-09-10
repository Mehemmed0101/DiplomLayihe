using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using DiplomLayihe.Models.DataContext;
using DiplomLayihe.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using DiplomLayihe.AppCode.Providers;
using System.IO;
using Microsoft.Extensions.Options;
using DiplomLayihe;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews(cfg =>
{

    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


    cfg.Filters.Add(new AuthorizeFilter(policy));

});

builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

var connectionString = builder.Configuration.GetConnectionString("cString");

builder.Services.AddDbContext<DiplomDbContext>(builder =>
{
    builder.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<DiplomUser, DiplomRole>()
    .AddEntityFrameworkStores<DiplomDbContext>();

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<UserManager<DiplomUser>>();
builder.Services.AddScoped<SignInManager<DiplomUser>>();
builder.Services.AddScoped<RoleManager<DiplomRole>>();

builder.Services.Configure<IdentityOptions>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;

    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequiredLength = 3;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 1;

    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 1, 0);
});


builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/signin.html";
    cfg.AccessDeniedPath = "/accessdenied.html";
    cfg.LogoutPath = "/signout.html";

    cfg.Cookie.Name = "diplomapp";
    cfg.Cookie.HttpOnly = true;
    cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);
});



var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.InitMembership();



app.UseRequestLocalization(cfg =>
{
    cfg.AddSupportedUICultures("az", "en", "ru");
    cfg.AddSupportedCultures("az", "en", "ru");
    cfg.RequestCultureProviders.Clear();
    cfg.RequestCultureProviders.Add(new CultureProvider());
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(cfg =>
{
    cfg.MapAreaControllerRoute(
        name: "defaultAdmin-with-lang",
        areaName: "Admin",
        pattern: "{lang}/Admin/{controller=dashboard}/{action=index}/{id?}",
        constraints: new
        {
            lang = "en|az|ru"
        });
    cfg.MapAreaControllerRoute(
        name: "defaultAdmin",
        areaName: "Admin",
        pattern: "Admin/{controller=dashboard}/{action=index}/{id?}"
        );
    cfg.MapControllerRoute("default-with-lang", pattern: "{lang}/{controller=homepage}/{action=index}/{id?}",
        constraints: new
        {
            lang = "en|az|ru"
        });
    cfg.MapControllerRoute("default", pattern: "{controller=homepage}/{action=index}/{id?}");
});

app.Run();
