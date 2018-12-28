using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TodoApi.Models;

#region EmployeeController
namespace EmployeeApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;
        #endregion

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
            /*
            if (_context.EmployeeItems.Count() == 0)
            {
                //_context.EmployeeItems.Add(new EmployeeItem { FirstName = "Dillon", LastName = "Sun", Birth = new System.DateTime(1999, 06, 09), Department = "Develop", Email = "sunhao1109@outlook.com", Gender = "M"});
                //_context.EmployeeItems.Add(new EmployeeItem { FirstName = "Dillon", LastName = "Sun", Birth = new System.DateTime(1996, 08, 09), Department = "Develop", Email = "sunhao1109@outlook.com", Gender = "M"});
                _context.SaveChanges();
            }
            */
        }

        #region snippet_GetAll
        [HttpGet]
        public ActionResult<List<EmployeeItem>> GetAll()
        {
            return _context.EmployeeItems.ToList();
        }

        #region snippet_GetByID
        [HttpGet("{id}", Name = "GetEmployee")]
        public ActionResult<EmployeeItem> GetById(long id)
        {
            var item = _context.EmployeeItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        #endregion
        #endregion

        #region snippet_Create
        [HttpPost]
        public ActionResult Create(EmployeeItem item)
        {
            if(!IsLegal(item))
            {
                return CreatedAtRoute("GetEmployee", 1);
            }
            _context.EmployeeItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetEmployee", new { id = item.Id }, item);
        }
        #endregion

        private bool IsLegal(EmployeeItem item)
        {
            return ValidatePhone(item.Phone) && ValidateEmail(item.Email) && !IsEmailExist(item);
        }


        #region snippet_Update
        [HttpPut("{id}")]
        public ActionResult Update(long id, EmployeeItem item)
        {
            var Employee = _context.EmployeeItems.Find(id);
            if (Employee == null)
            {
                return NotFound();
            }

            Employee.Id = item.Id;
            Employee.FirstName = item.FirstName;
            Employee.LastName = item.LastName;
            Employee.Gender = item.Gender;
            Employee.Email = item.Email;
            Employee.Birth = item.Birth;
            Employee.Address = item.Address;
            Employee.Phone = item.Phone;
            Employee.Department = item.Department;

            _context.EmployeeItems.Update(Employee);
            _context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region snippet_Delete
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            var Employee = _context.EmployeeItems.Find(id);
            if (Employee == null)
            {
                return NotFound();
            }

            _context.EmployeeItems.Remove(Employee);
            _context.SaveChanges();
            return NoContent();
        }
        #endregion

        private bool ValidatePhone(long phone)
        {
            string regex = @"^[1]\d{10,10}$";
            return Regex.IsMatch(phone.ToString(), regex);
        }

        private bool ValidateEmail(string email)
        {
            string regex = "^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$";
            return Regex.IsMatch(email, regex);
        }

        private bool IsEmailExist(EmployeeItem item)
        {
            return _context.EmployeeItems.Any(e => e.Email.Equals(item.Email) && e.Id != item.Id);
        }
    }
}