using EmployeeManagementMVC.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementMVC.Models
{
    public class EmployeeIndexViewModel 
    {
        public int PageSize { get; set; }
        public string OrderByString { get; set; }
        public string SearchString { get; set; }
        public PaginatedList<Employee> EmployeeList { get; set; }
        public string Username { get; set; }
        public string OrderByType { get; set; }

        public EmployeeIndexViewModel(string username, int pageSize, string orderString, string searchString, string sortType, PaginatedList<Employee> employeeList) 
        {
            
            this.Username = username;
            this.PageSize = pageSize;
            this.OrderByString = orderString;
            this.SearchString = searchString;
            this.OrderByType = sortType;
            this.EmployeeList = employeeList;
        }

    }
}
