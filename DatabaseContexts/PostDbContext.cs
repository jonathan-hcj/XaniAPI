using Microsoft.EntityFrameworkCore;
using XaniAPI.Entites;

namespace XaniAPI.DatabaseContexts
{
    public class PostDbContext : DbContext
    {
        public DbSet<Post> Post { get; set; }

        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
        }

        public PostDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //  // modelBuilder.Entity<Post>().Ignore(t => t.p_info);
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
