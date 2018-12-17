using EmployeeManagementMVC.utils;
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
        public string OrderBy { get; set; }
        public string SearchString { get; set; }
        public PaginatedList<Employee> EmployeeList { get; set; }
        public string Username { get; set; }

        private readonly EmployeeManagementMVCContext _context;

        public EmployeeIndexModel(EmployeeManagementMVCContext context)
        {
            _context = context;
        }

        public EmployeeIndexModel()
        {
        }

        public EmployeeIndexModel(string username, int PageIndex, int PageSize, string Order, string SearchString, PaginatedList<Employee> Employee) 
        {
            
            this.Username = username;
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            this.OrderBy = Order;
            this.SearchString = SearchString;
            this.EmployeeList = Employee;
        }

    }
}
