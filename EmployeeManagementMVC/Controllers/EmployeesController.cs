using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.utils;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Linq.Expressions;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;
        public IConfiguration Configuration { get; }

        public EmployeesController(EmployeeManagementMVCContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        /// <summary>
        /// 查询排序分页
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="orderByString"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string orderByString, int? pageIndex, string username, string orderByType)
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
                employeeIQ = OrderByEmployee(employeeIQ, orderByString, orderByType);
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
            /*
            int pageSize = Configuration.GetSection("Constant").GetValue<int>("PageSize");
            PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(employeeIQ, pageIndex ?? 1, pageSize);
            EmployeeIndexModel employeeIndexModel = new EmployeeIndexModel(username, pageSize, orderByString, searchString, sortType, paginatedList);
            */
            EmployeeIndexViewModel employeeIndexModel = await GetEmployeeIndexModelAsync(employeeIQ, pageIndex, username, orderByString, searchString, orderByType);
            return View(employeeIndexModel);

        }

        private async Task<EmployeeIndexViewModel> GetEmployeeIndexModelAsync(IQueryable<Employee> employeeIQ, int? pageIndex, string username, string orderByString, string searchString, string orderByType)
        {
            int pageSize = Configuration.GetSection("Constant").GetValue<int>("PageSize");
            PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(employeeIQ, pageIndex ?? 1, pageSize);
            EmployeeIndexViewModel employeeIndexModel = new EmployeeIndexViewModel(username, pageSize, orderByString, searchString, orderByType, paginatedList);
            return employeeIndexModel;
        }

        /// <summary>
        /// 查看详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
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

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        // GET: Employees/Create
        public IActionResult Create()
        {
            if (IsNotLogin())
            {
                return RedirectToAction("Login", "EmployeesLogin");
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
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                return RedirectToAction("Index");
            }
            return View(employee);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

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
        private IQueryable<Employee> OrderByEmployee(IQueryable<Employee> employeeIQ, string orderByString, string orderByType)
        {
            Expression<Func<Employee, object>> sortExpression;
            switch (orderByString)
            {
                // var sortExpression = item.id
                // if desc orderbydesc asc orderbyasc 
                case "Id": sortExpression = item => item.Id;  break;
                case "FirstName": sortExpression = item => item.FirstName; break;
                case "LastName": sortExpression = item => item.LastName; break;
                case "Gender": sortExpression = item => item.Gender; break;
                case "Birth": sortExpression = item => item.Birth; break;
                case "Address": sortExpression = item => item.Address; break;
                case "Phone": sortExpression = item => item.Phone; break;
                case "Email": sortExpression = item => item.Email; break;
                case "Department": sortExpression = item => item.Department; break;
                default: sortExpression = item => item.Id; break;
            }
            if("reversed".Equals(orderByType))
            {
                employeeIQ = employeeIQ.OrderByDescending(sortExpression);
            }
            else
            {
                employeeIQ = employeeIQ.OrderBy(sortExpression);
            }
            return employeeIQ;
        }
    }
}
