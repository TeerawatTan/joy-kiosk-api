using JoyKioskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyKioskApi.Datas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserTokenModel> UserTokens { get; set; }
        public DbSet<UserModel> Users { get; set; }


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<UserRoleModel>()
        //    .HasKey(e => new { e.UserId, e.RoleId });
        //}
    }
}
