using Microsoft.EntityFrameworkCore;
using XaniAPI.Entities;

namespace XaniAPI.DatabaseContexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
