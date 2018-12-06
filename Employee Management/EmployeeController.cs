using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management
{
    class EmployeeController
    {
        public List<Employee> employees = new List<Employee>();

        /// <summary>
        /// display
        /// </summary>
        public void DisplayEmployees()
        {
            Console.WriteLine("当前employee数：" + employees.Count);
            foreach(Employee employee in employees)
            {
                Console.WriteLine(employee.ToString());
            }
        }

       /// <summary>
       /// add
       /// </summary>
        public void AddEmployee()
        {
            Employee employee = new Employee();
            Console.WriteLine("请输入id");
            employee.Id = Int32.Parse(Console.ReadLine());
            Console.WriteLine("firstname");
            employee.FirstName = Console.ReadLine();
            Console.WriteLine("请输入lastname");
            employee.LastName = Console.ReadLine();
            Console.WriteLine("请输入gender: M/F");
            string gender = Console.ReadLine();
            while(true) {
                if ("M".Equals(gender))
                {
                    employee.Gender = Gender.M;
                    break;
                }
                else if ("F".Equals(gender))
                {
                    employee.Gender = Gender.F;
                    break;
                }
                else
                {
                    Console.WriteLine("请输入M/F");
                    gender = Console.ReadLine();
                }
            }
            Console.WriteLine("请输入birth");
            employee.Birth = new DateTime(long.Parse(Console.ReadLine()));
            Console.WriteLine("请输入address");
            employee.Address = Console.ReadLine();
            Console.WriteLine("请输入phone");
            employee.Phone = long.Parse(Console.ReadLine());
            
            foreach(Employee temp in employees)
            {
                if(employee.Id == temp.Id)
                {
                    Console.WriteLine("该id已存在");
                    return;
                }
            }
            employees.Add(employee);
            Console.WriteLine("add完成");
        }

        /// <summary>
        /// delete
        /// </summary>
        public void DeleteEmployee(int id)
        {
            for(int i = 0; i < employees.Count;)
            {
                if(employees[i].Id == id)
                    employees.Remove(employees[i]);      
                else
                    i++;
            }
            Console.WriteLine("delete完成");
        }

        /// <summary>
        /// 修改employee
        /// </summary>
        /// <param name="id"></param>
        public void UpdateEmployee(int id)
        {
            DeleteEmployee(id);
            AddEmployee();
            Console.WriteLine("update完成");
        }
    }
}
