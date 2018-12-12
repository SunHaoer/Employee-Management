using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementMVC.Models
{
    public class EmployeeManagementMVCContext : DbContext
    {
        public EmployeeManagementMVCContext (DbContextOptions<EmployeeManagementMVCContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeManagementMVC.Models.Employee> Employee { get; set; }
    }
}
