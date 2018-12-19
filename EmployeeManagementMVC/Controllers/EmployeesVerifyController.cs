using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesVerifyController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;

        public EmployeesVerifyController(EmployeeManagementMVCContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 校验邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(int id, string email)
        {
            if (IsEmailExist(id, email))    // create时id为0
            {
                return Json($"Email {email} is already in use.");
            }
            return Json(true);
        }

        /// <summary>
        /// 验证邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsEmailExist(int id, string email)
        {
            IQueryable<Employee> employeeIQ = _context.Employee;
            return employeeIQ.Any(item => item.Email.Equals(email) && !item.Id.Equals(id));
        }
    }
}