using System.Linq;
using EmployeeManagementMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesLoginController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;

        public EmployeesLoginController(EmployeeManagementMVCContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                IQueryable<Employee> employeeIQ = _context.Employee;
                bool isLegal = employeeIQ.Any(item => item.FirstName.Equals(username) && item.LastName.Equals(password));
                if (isLegal)
                {    // 登录成功
                    HttpContext.Session.SetString("loginUsername", username);
                    return RedirectToAction("Index", "Employees");
                }
                else
                {
                    return View("Login");
                }
            }
            else
            {
                return View( "Login");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Logout(string username, string password)
        {
            HttpContext.Session.Remove("loginUsername");
            return RedirectToAction("Login", "EmployeesLogin");
        }
    }
}