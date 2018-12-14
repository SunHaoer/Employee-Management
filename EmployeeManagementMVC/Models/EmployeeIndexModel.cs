using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementMVC.Models
{
    public class EmployeeIndexModel 
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        private readonly EmployeeManagementMVCContext _context;

        public EmployeeIndexModel(EmployeeManagementMVCContext context)
        {
            _context = context;
        }

        public EmployeeIndexModel()
        {
        }

        public List<Employee> EmployeeIQ;
    }
}
