using Microsoft.EntityFrameworkCore;
using XaniAPI.Entities;

namespace XaniAPI.DatabaseContexts
{
    public class RepostDbContext : DbContext
    {
        public DbSet<Repost> Repost { get; set; }

        public RepostDbContext()
        {
        }

        public RepostDbContext(DbContextOptions<RepostDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
