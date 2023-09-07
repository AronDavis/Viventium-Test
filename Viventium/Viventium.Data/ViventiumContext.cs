using Microsoft.EntityFrameworkCore;
using Viventium.Data.Models;

namespace Viventium.Data
{
    public class ViventiumContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public ViventiumContext(DbContextOptions<ViventiumContext> options) : base(options)
        {

        }
    }
}
