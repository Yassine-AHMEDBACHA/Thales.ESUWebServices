using System.Security.Cryptography.X509Certificates;
using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public DbSet<ProcessingStatus> ProcessingStatus { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Activation> Activations { get; set; }

        public DbSet<Key> ProductKeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("ESU"), sqlOptions => sqlOptions.CommandTimeout(300));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .ToTable("Hosts");

            modelBuilder.Entity<Key>().ToTable("Keys");


            modelBuilder.Entity<Confirmation>().ToTable("Confirmations")
                .HasOne(x => x.License);

            modelBuilder.Entity<ProcessingStatus>().ToView("ProcessingStatus")
                .HasOne(x => x.Host);

            modelBuilder.Entity<License>().ToTable("Licenses")
                .HasOne(x => x.Host);

            modelBuilder.Entity<Status>().ToTable("Status")
                .HasOne(x => x.Host);

            modelBuilder.Entity<Activation>().ToTable("Activations")
                .HasOne(x => x.License);
        }
    }
}
