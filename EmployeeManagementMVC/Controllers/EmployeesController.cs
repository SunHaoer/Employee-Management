using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;

        public EmployeesController(EmployeeManagementMVCContext context)
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
            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                IQueryable<Employee> employeeIQ = _context.Employee;
                bool isLegal = employeeIQ.Any(item => item.FirstName.Equals(username) && item.LastName.Equals(password));
                if(isLegal)
                {    // 登录成功
                    HttpContext.Session.SetString("loginUsername", username);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(nameof(Login));
                }
            }
            else
            {
                return View(nameof(Login));
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
            return View(nameof(Login));
        }

        /// <summary>
        /// 查询排序分页
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="orderByString"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string orderByString, int? pageIndex, string username, string sortType)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                username = HttpContext.Session.GetString("loginUsername");
                IQueryable<Employee> employeeIQ = _context.Employee;
                // 查询
                if (!string.IsNullOrEmpty(searchString))
                {
                    employeeIQ = SearchEmployee(employeeIQ, searchString.Trim());
                }
                // 排序
                if (!string.IsNullOrEmpty(orderByString))
                {
                    employeeIQ = OrderByEmployee(employeeIQ, orderByString);
                }
                /*
                List<Employee> resultList = employeeIQ.ToList();
                // 倒序
                if ("reversed".Equals(sortType))
                {
                    resultList.Reverse();
                    //employeeIQ = employeeIQ.Reverse();
                }
                */
                // 分页
                int pageSize = 2;
                /*
                PaginatedList<Employee> paginatedList = PaginatedList<Employee>.Create(
                   resultList, pageIndex ?? 1, pageSize);
                */
                PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(employeeIQ, pageIndex ?? 1, pageSize);
                EmployeeIndexModel employeeIndexModel = new EmployeeIndexModel(username, pageSize, orderByString, searchString, sortType, paginatedList);
                return View(employeeIndexModel);
            }
        }

        /// <summary>
        /// 查看详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        // GET: Employees/Create
        public IActionResult Create()
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Gender,Birth,Address,Phone,Email,Department")] Employee employee)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }

        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,Birth,Address,Phone,Email,Department")] Employee employee)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                //int a = employee.Id;
                Employee tempEmployee = employee;
                if (id != employee.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                var employee = await _context.Employee.FindAsync(id);
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// 远程校验邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult Verify(int id, string email)
        {
            if (IsEmailExist(id, email))    // create时id为0
            {
                return Json($"Email {email} is already in use.");
            }
            return Json(true);
        }

        /// <summary>
        /// 判断该employee是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <returns></returns>
        private bool IsNotLogin()
        {
            return String.IsNullOrEmpty(HttpContext.Session.GetString("loginUsername"));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="employeeIQ"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private IQueryable<Employee> SearchEmployee(IQueryable<Employee> employeeIQ, string searchString)
        {
            return employeeIQ.Where(item => item.Id.ToString().Equals(searchString) || item.FirstName.Contains(searchString) || item.LastName.Contains(searchString));
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="employeeIQ"></param>
        /// <param name="orderByString"></param>
        /// <returns></returns>
        private IQueryable<Employee> OrderByEmployee(IQueryable<Employee> employeeIQ, string orderByString)
        {
            switch (orderByString)
            {
                // var sortExpression = item.id
                // if desc orderbydesc asc orderbyasc 
                case "Id": employeeIQ = employeeIQ.OrderBy(item => item.Id); break;
                case "FirstName": employeeIQ = employeeIQ.OrderBy(item => item.FirstName).ThenBy(item => item.Id); break;
                case "LastName": employeeIQ = employeeIQ.OrderBy(item => item.LastName).ThenBy(item => item.Id); break;
                case "Gender": employeeIQ = employeeIQ.OrderBy(item => item.Gender).ThenBy(item => item.Id); break;
                case "Birth": employeeIQ = employeeIQ.OrderBy(item => item.Birth).ThenBy(item => item.Id); break;
                case "Address": employeeIQ = employeeIQ.OrderBy(item => item.Address).ThenBy(item => item.Id); break;
                case "Phone": employeeIQ = employeeIQ.OrderBy(item => item.Phone).ThenBy(item => item.Id); break;
                case "Email": employeeIQ = employeeIQ.OrderBy(item => item.Email).ThenBy(item => item.Id); break;
                case "Department": employeeIQ = employeeIQ.OrderBy(item => item.Department).ThenBy(item => item.Id); break;
            }
            return employeeIQ;
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
