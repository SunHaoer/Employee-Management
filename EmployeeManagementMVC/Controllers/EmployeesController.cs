using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.utils;
using Microsoft.AspNetCore.Http;

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
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string orderBy, int? pageIndex, string username)
        {
            /*
            var employee = from m in _context.Employee
                         select m;
            */
            if(IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
                username = HttpContext.Session.GetString("loginUsername");
                IQueryable<Employee> employeeIQ = _context.Employee;
                IQueryable<Employee> result = employeeIQ;
                // 查询
                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.Trim();
                    result = employeeIQ.Where(s => s.Id.ToString().Equals(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
                }
                // 排序
                if(!String.IsNullOrEmpty(orderBy))
                {
                    switch (orderBy)
                    {
                        case "Id": result = result.OrderBy(item => item.Id); break;
                        case "FirstName": result = result.OrderBy(item => item.FirstName); break;
                        case "LastName": result = result.OrderBy(item => item.LastName); break;
                    }
                }
                // 分页
                int pageSize = 2;
                PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(
                    result.AsNoTracking(), pageIndex ?? 1, pageSize);
                EmployeeIndexModel employeeIndexModel = new EmployeeIndexModel(username, pageIndex ?? 1, pageSize, orderBy, searchString, paginatedList);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Gender,Birth,Address,Phone,Email,Department")] Employee employee)
        {
            if (IsNotLogin())
            {
                return View(nameof(Login));
            }
            else
            {
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

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        private bool IsNotLogin()
        {
            return String.IsNullOrEmpty(HttpContext.Session.GetString("loginUsername"));
        }
    }
}
