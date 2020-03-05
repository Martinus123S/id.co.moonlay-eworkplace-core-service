using Com.Moonlay.Data.EntityFrameworkCore;
using EWorkplaceCoreService.Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace EWorkplaceCoreService.Lib
{
    public class CoreDbContext : StandardDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
