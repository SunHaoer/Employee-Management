using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeItem> EmployeeItems { get; set; }
    }
}

