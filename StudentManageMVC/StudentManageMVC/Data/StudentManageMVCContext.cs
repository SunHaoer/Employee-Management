using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentManageMVC.Models
{
    public class StudentManageMVCContext : DbContext
    {
        public StudentManageMVCContext (DbContextOptions<StudentManageMVCContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManageMVC.Models.Student> Student { get; set; }
    }
}
