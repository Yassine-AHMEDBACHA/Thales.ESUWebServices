using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.DashbordWS.Models;
using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESU.DashbordWS.Infrastructures
{
    public class ESUContext : DbContext
    {
        private readonly IConfiguration configuration;

        public ESUContext(DbContextOptions<ESUContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("ESU"), sqlOptions => sqlOptions.CommandTimeout(300));
        }

        public DbSet<Host> Hosts { get; set; }

        public DbSet<Stat> Stats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>().ToTable("Hosts");

            modelBuilder.Entity<Stat>().HasNoKey().ToView(null).Ignore(x => x.Caption);
        }
}
    }