using Microsoft.EntityFrameworkCore;
using XaniAPI.Entites;

namespace XaniAPI.DatabaseContexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Like { get; set; }

        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<LikeDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
