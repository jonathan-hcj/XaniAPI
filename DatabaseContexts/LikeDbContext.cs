using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using XaniAPI.Entites;

namespace XaniAPI.DatabaseContexts
{
    public class LikeDbContext : DbContext
    {
        public DbSet<Like> Like { get; set; }

        public LikeDbContext()
        {
        }

        public LikeDbContext(DbContextOptions<LikeDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Like>()
            //.Property(b => b.l_id)
            //.HasColumnType("numeric");
        }
    }
}
