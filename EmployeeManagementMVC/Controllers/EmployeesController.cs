using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.utils;

namespace EmployeeManagementMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeManagementMVCContext _context;

        public EmployeesController(EmployeeManagementMVCContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string orderBy, int? pageIndex)
        {
            /*
            var employee = from m in _context.Employee
                         select m;
            */
            IQueryable<Employee> employee = _context.Employee;
            IQueryable<Employee> result = employee;
            // 查询
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                result = employee.Where(s => s.Id.ToString().Equals(searchString) || s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
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
            /*
            int pageSize = 3;
            result = result.Skip(0).Take(pageSize);
            //result.AsNoTracking();
            */
            
            int pageSize = 2;
            PaginatedList<Employee> paginatedList = await PaginatedList<Employee>.CreateAsync(
                result.AsNoTracking(), pageIndex ?? 1, pageSize);
            EmployeeIndexModel employeeIndexModel = new EmployeeIndexModel();
            employeeIndexModel.Employee = paginatedList;
            employeeIndexModel.PageIndex = pageIndex ?? 1;
            employeeIndexModel.OrderBy = orderBy;
            employeeIndexModel.SearchString = searchString;
            
            return View(employeeIndexModel);
        }

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

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

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
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Gender,Birth,Address,Phone,Email,Department")] Employee employee)
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

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
