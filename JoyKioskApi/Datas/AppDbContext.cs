using JoyKioskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyKioskApi.Datas
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<UserTokenModel> UserTokens { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<LogTxnRequest> LogTxnRequests { get; set; }
        public DbSet<LogTxnResponse> LogTxnResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var conn = Environment.GetEnvironmentVariable("ConnectionString");
            if (conn == null)
            {
                throw new Exception("Connection string not found");
            }

            optionsBuilder.UseNpgsql(conn);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LogTxnRequest>()
            .HasKey(e => new { e.TxnDate, e.TxnType, e.PartnerTxnUid, e.PartnerId });

            builder.Entity<LogTxnResponse>()
            .HasKey(e => new { e.TxnDate, e.TxnType, e.PartnerTxnUid, e.PartnerId });
        }
    }
}
