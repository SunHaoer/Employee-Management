using EmployeeManagementMVC.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementMVC.Models
{
    public class EmployeeIndexModel 
    {
        public int PageSize { get; set; }
        public string OrderByString { get; set; }
        public string SearchString { get; set; }
        public PaginatedList<Employee> EmployeeList { get; set; }
        public string Username { get; set; }
        public string SortType { get; set; }

        public EmployeeIndexModel(string username, int pageSize, string orderString, string searchString, string sortType, PaginatedList<Employee> employeeList) 
        {
            
            this.Username = username;
            this.PageSize = pageSize;
            this.OrderByString = orderString;
            this.SearchString = searchString;
            this.SortType = sortType;
            this.EmployeeList = employeeList;
        }

    }
}
