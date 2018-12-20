using EmployeeManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementMVC.Service
{
    public class EmployeesLoginService
    {
        private readonly IQueryable<Employee> EmployeeIQ;

        public EmployeesLoginService(IQueryable<Employee> employeeIQ)
        {
            EmployeeIQ = employeeIQ;
        }

        public bool IslegalLogin(string username, string password)
        {
            return EmployeeIQ.Any(item => item.FirstName.Equals(username) && item.LastName.Equals(password));
        }
    }
}
