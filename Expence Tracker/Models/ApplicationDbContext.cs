using Microsoft.EntityFrameworkCore;

namespace Expence_Tracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
