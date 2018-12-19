using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using EmployeeManagementMVC.Service;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;
        private IConfiguration Configuration { get; }
        private EmployeesService EmployeesService; 

        public EmployeesController(EmployeeManagementMVCContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            EmployeesService = new EmployeesService(_context, Configuration);
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
                employeeIQ = EmployeesService.SearchEmployee(employeeIQ, searchString.Trim());
            }
            // 排序
            if (!string.IsNullOrEmpty(orderByString))
            {
                employeeIQ = EmployeesService.OrderByEmployee(employeeIQ, orderByString, orderByType);
            }
            EmployeeIndexViewModel employeeIndexModel = await EmployeesService.GetEmployeeIndexModelAsync(employeeIQ, pageIndex, username, orderByString, searchString, orderByType);
            return View(employeeIndexModel);
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
            /*
            var employee = await _context.Employee
                .FirstOrDefaultAsync(item => item.Id == id);
            */
            Employee employee = await EmployeesService.FindEmployeeByIdAsync(id);
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
            return View();
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
                /*
                _context.Add(employee);
                await _context.SaveChangesAsync();
                */
                await EmployeesService.AddEmployeeAsync(employee);
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
            /*
            var employee = await _context.Employee.FindAsync(id);
            */
            Employee employee = await EmployeesService.FindEmployeeByIdAsync(id);
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
                /*
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesService.EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                */
                bool isEmpoyeExist = await EmployeesService.EditEmployeeAsync(employee);
                if (!isEmpoyeExist)
                {
                    return NotFound();
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
            /*
            var employee = await _context.Employee
                .FirstOrDefaultAsync(item => item.Id == id);
            */
            Employee employee = await EmployeesService.FindEmployeeByIdAsync(id);
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
            /*
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            */
            await EmployeesService.DeleteEmployeeByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}
