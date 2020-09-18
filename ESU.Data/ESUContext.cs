using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace ESU.Data
{
    public class ESUContext : DbContext
    {
        private readonly IConfiguration configuration;

        public ESUContext(DbContextOptions<ESUContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Host> Hosts { get; set; }

        public DbSet<Confirmation> Confirmations { get; set; }

        public DbSet<License> Licenses { get; set; }

        public DbSet<ActivatedLicense> ActivatedLicenses { get; set; }

        public DbSet<ProcessingStatus> ProcessingStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("ESU"), sqlOptions => sqlOptions.CommandTimeout(300));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .ToTable("Hosts");

            modelBuilder.Entity<Confirmation>().ToTable("Confirmations")
                .HasOne(x => x.License);

            modelBuilder.Entity<ProcessingStatus>().ToTable("ProcessingStatus")
                .HasOne(x => x.Host);

            modelBuilder.Entity<License>().ToTable("Licenses")
                .HasOne(x => x.Host);

            modelBuilder.Entity<ActivatedLicense>().ToTable("ActivatedLicenses")
                .HasOne(x => x.License);
        }
    }
}
