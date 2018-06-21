using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.DB;
using System;
using Model.DB.Code;

namespace DAL
{
    public class MainDbContext : IdentityDbContext<User>
    {

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCode> UsersCode { get; set; }
        public DbSet<CodeHistory> CodeHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Messages> Messages { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
