using Microsoft.EntityFrameworkCore;
using XaniAPI.Entities;

namespace XaniAPI.DatabaseContexts
{
    public class FeedDbContext : DbContext
    {
        public DbSet<Feed> Feed { get; set; }

        public FeedDbContext()
        {
        }

        public FeedDbContext(DbContextOptions<FeedDbContext> options) : base(options)
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
