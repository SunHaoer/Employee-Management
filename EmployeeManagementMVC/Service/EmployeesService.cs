using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagementMVC.Service
{
    public class EmployeesService
    {
        private readonly EmployeeManagementMVCContext _context;
        private IConfiguration Configuration { get; }
        private Dictionary<string, Expression<Func<Employee, object>>> OrderByDictionary;

        public EmployeesService(EmployeeManagementMVCContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            OrderByDictionary = InitOrderByDictionary(OrderByDictionary);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="employeeIQ"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IQueryable<Employee> SearchEmployee(IQueryable<Employee> employeeIQ, string searchString)
        {
            return employeeIQ.Where(item => item.Id.ToString().Equals(searchString) || item.FirstName.Contains(searchString) || item.LastName.Contains(searchString));
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="employeeIQ"></param>
        /// <param name="orderByString"></param>
        /// <returns></returns>
        public IQueryable<Employee> OrderByEmployee(IQueryable<Employee> employeeIQ, string orderByString, string orderByType)
        {
            orderByString = (string.IsNullOrEmpty(orderByString) ? "Id" : orderByString); 
            Expression<Func<Employee, object>> sortExpression = OrderByDictionary.GetValueOrDefault(orderByString);
            if ("reversed".Equals(orderByType))
            {
                employeeIQ = employeeIQ.OrderByDescending(sortExpression);
            }
            else
            {
                employeeIQ = employeeIQ.OrderBy(sortExpression);
            }
            return employeeIQ;
        }

        /// <summary>
        /// 获取EmployeeIndexViewModel
        /// </summary>
        /// <param name="employeeIQ"></param>
        /// <param name="pageIndex"></param>
        /// <param name="username"></param>
        /// <param name="orderByString"></param>
        /// <param name="searchString"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<EmployeeIndexViewModel> GetEmployeeIndexModelAsync(IQueryable<Employee> employeeIQ, int? pageIndex, string username, string orderByString, string searchString, string orderByType)
        {
            int pageSize = Configuration.GetSection("Constant").GetValue<int>("PageSize");
            PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(employeeIQ, pageIndex ?? 1, pageSize);
            EmployeeIndexViewModel employeeIndexModel = new EmployeeIndexViewModel(username, pageSize, orderByString, searchString, orderByType, paginatedList);
            return employeeIndexModel;
        }

        /// <summary>
        /// 根据id查Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Employee> FindEmployeeByIdAsync(int? id)
        {
            return await _context.Employee.FirstOrDefaultAsync(item => item.Id == id);
        }

        /// <summary>
        /// AddEmployee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<EmployeeManagementMVCContext> AddEmployeeAsync(Employee employee)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return _context;
        }

        /// <summary>
        /// EditEmployee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<bool> EditEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// DeleteEmployeeById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeByIdAsync(int id)
        {
            Employee employee = await FindEmployeeByIdAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 初始化OrderByDictionary
        /// </summary>
        /// <param name="OrderByDictionary"></param>
        private Dictionary<string, Expression<Func<Employee, object>>> InitOrderByDictionary(Dictionary<string, Expression<Func<Employee, object>>> OrderByDictionary)
        {
            OrderByDictionary = new Dictionary<string, Expression<Func<Employee, object>>>()
            {
                {"Id", item => item.Id },
                {"FirstName", item => item.FirstName },
                {"LastName", item => item.LastName },
                {"Gender", item => item.Gender },
                {"Birth", item => item.Birth },
                {"Address", item => item.Address },
                {"Phone", item => item.Phone },
                {"Email", item => item.Email },
                {"Department", item => item.Department }
            };
            return OrderByDictionary;
        }

        /// <summary>
        /// 判断该employee是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(item => item.Id == id);
        }
    }
}
