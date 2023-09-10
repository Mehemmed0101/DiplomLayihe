using DiplomLayihe.Models.Entities;
using DiplomLayihe.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomLayihe.Models.DataContext
{
    public class DiplomDbContext : IdentityDbContext<DiplomUser, DiplomRole, int, DiplomUserClaim, DiplomUserRole, DiplomUserLogin, DiplomRoleClaim, DiplomUserToken>
    {
        public DiplomDbContext(DbContextOptions options)
            : base(options)
        {

        }



        public DbSet<CourseCategories> CourseCategories { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<BlogPosts> BlogPosts { get; set; }
        public DbSet<LastNewsandEvents> LastNewsandEvents { get; set; }
        public DbSet<EventSpeakers> EventSpeakers { get; set; }
        public DbSet<EventGallery> EventGallery { get; set; }
        public DbSet<PlanAndPricing> PlanAndPricing { get; set; }
        public DbSet<PlanExplanations> PlanExplanations { get; set; }
        public DbSet<CourseCategoriesForHomePage> CourseCategoriesForHomePage { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<BlogPostTag> BlogPostTagCloud { get; set; }
        public DbSet<CoursePostTag> CourseTagCloud { get; set; }
        public DbSet<EventPostTag> EventTagCloud { get; set; }
        public DbSet<CourseTeachers> CourseTeachersCloud { get; set; }
        public DbSet<BlogPostComments> BlogPostComments { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPostTag>(e =>
            {
                e.HasKey(k => new { k.BlogPostId, k.PostTagId });
            });


            modelBuilder.Entity<CoursePostTag>(e =>
            {
                e.HasKey(k => new { k.CoursePostId, k.PostTagId });
            });

            modelBuilder.Entity<EventPostTag>(e =>
            {
                e.HasKey(k => new { k.EventPostId, k.PostTagId });
            });

            modelBuilder.Entity<CourseTeachers>(e =>
            {
                e.HasKey(k => new { k.TeachersId, k.CoursePostId });
            });




            //Membership

            modelBuilder.Entity<DiplomUser>(e =>
            {
                e.ToTable("Users", "Membership"); //Sql de adinin 
            });
            modelBuilder.Entity<DiplomRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            modelBuilder.Entity<DiplomUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            modelBuilder.Entity<DiplomUserToken>(e =>
            {
                e.HasKey(k => new { k.UserId, k.LoginProvider, k.Name });
                e.ToTable("UserTokens", "Membership");
            });
            modelBuilder.Entity<DiplomUserLogin>(e =>
            {
                e.HasKey(k => new { k.UserId, k.LoginProvider, k.ProviderKey });
                e.ToTable("UserLogins", "Membership");
            });
            modelBuilder.Entity<DiplomRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            modelBuilder.Entity<DiplomUserRole>(e =>
            {
                e.HasKey(k => new { k.UserId, k.RoleId });
                e.ToTable("UserRoles", "Membership");
            });
        }

    }
}
