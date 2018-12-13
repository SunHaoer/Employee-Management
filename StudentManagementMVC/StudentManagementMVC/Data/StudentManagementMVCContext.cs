using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementMVC.Models
{
    public class StudentManagementMVCContext : DbContext
    {
        public StudentManagementMVCContext (DbContextOptions<StudentManagementMVCContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManagementMVC.Models.Student> Student { get; set; }
    }
}
