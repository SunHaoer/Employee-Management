using System.Linq;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesLoginController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;
        private readonly IQueryable<Employee> EmployeeIQ;
        private readonly EmployeesLoginService EmployeesLoginService;

        public EmployeesLoginController(EmployeeManagementMVCContext context)
        {
            _context = context;
            EmployeeIQ = _context.Employee;
            EmployeesLoginService = new EmployeesLoginService(EmployeeIQ);
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
                if (EmployeesLoginService.IslegalLogin(username, password))
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