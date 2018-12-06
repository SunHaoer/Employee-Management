using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Employee_Management
{
    class EmployeeController
    {
        public const int MAX = 100000;    // 最大employee数
        public List<Employee> employees = new List<Employee>();
        public bool[] employeeIdExist = new bool[MAX];    // 表示该employeeId是否存在

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
            Console.WriteLine("请输入id:");
            employee.Id = InputEmployeeId();   
            Console.WriteLine("请输入firstname:");
            employee.FirstName = InputString();
            Console.WriteLine("请输入lastname");
            employee.LastName = InputString();
            Console.WriteLine("请输入gender: M/F");
            Gender gender = InputGender();
            Console.WriteLine("请输入birth");
            employee.Birth = InputBirth();
            Console.WriteLine("请输入address");
            employee.Address = InputAddress();
            Console.WriteLine("请输入phone");
            employee.Phone = InputPhone();
            
            foreach(Employee temp in employees)
            {
                if(employee.Id == temp.Id)
                {
                    Console.WriteLine("该id已存在");
                    return;
                }
            }
            employeeIdExist[employee.Id] = true;
            employees.Add(employee);
            Console.WriteLine("add完成");
        }

        private int InputEmployeeId()
        {
            int id;
            bool isFirst = true;
            do
            {
                string input;
                do
                {
                    if(!isFirst)
                    {
                        Console.WriteLine("输入不合法或该id已存在, 请重新输入0-100000内整数");
                    }
                    isFirst = false;
                    input = Console.ReadLine();
                } while (!JudgeNumber(input, 1, 5));
                id = Int32.Parse(input);
            } while (employeeIdExist[id]);
            employeeIdExist[id] = true;
            return id;
        }

        /// <summary>
        /// 判断输入是否合法 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool JudgeNumber(string input, int minlength, int maxLength)      
        {
            if (input.Length < minlength || input.Length > maxLength)
            {
                return false;    // 长度超限
            }
            else
            {
                for(int i = 0; i < input.Length; i++)
                {
                    if(!char.IsDigit(input[i]))
                    {
                        return false;    // 存在不合法字符
                    }
                }
                return true;
            }
        }

        private string InputString()
        {
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// 输入Gender
        /// </summary>
        /// <returns></returns>
        private Gender InputGender()
        {
            string input;
            bool isFirst = true;
            do
            {
                if(!isFirst)
                {
                    Console.WriteLine("请输入F/M:");
                }
                input = Console.ReadLine();
                isFirst = false;
            } while (!("F".Equals(input) || "M".Equals(input)));
            if ("F".Equals(input))
            {
                return Gender.F;
            }
            else
            {
                return Gender.M;
            }
        }

        /// <summary>
        /// 输入Birth
        /// </summary>
        /// <returns></returns>
        private DateTime InputBirth()
        {
            Console.Write("请输入year:");
            int year = InputNumber(1, 4);
            Console.Write("请输入month:");
            int month = InputNumber(1, 2);
            Console.Write("请输入day:");
            int day = InputNumber(1, 2);
            return new DateTime(year, month, day);
            
        }

        /// <summary>
        /// 判断输入是否为合法数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private int InputNumber(int minLength, int maxLength)
        {
            bool isFirst = true;
            string input;
            do
            {
                if(!isFirst)
                {
                    Console.WriteLine("输入不合法，请重新输入");
                }
                isFirst = false;
                input = Console.ReadLine();
            } while (!JudgeNumber(input, minLength, maxLength));
            int number = Int32.Parse(input);
            return number;
        }

        /// <summary>
        /// 输入address
        /// </summary>
        /// <returns></returns>
        private string InputAddress()
        {
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// 输入phone
        /// </summary>
        /// <returns></returns>
        private long InputPhone()
        {
            string input;
            bool isFirst = true;
            do
            {
                if(!isFirst)
                {
                    Console.WriteLine("输入不合法，请重新输入");
                }
                isFirst = false;
                input = Console.ReadLine();
            } while (!JudgeNumber(input, 11, 11));
            long number = long.Parse(input);
            return number;
        }

        /// <summary>
        /// delete
        /// </summary>
        public void DeleteEmployee()
        {
            Console.WriteLine("请输入需要删除的id：");
            int id = InputNumber(1, 5);
            if(!employeeIdExist[id])
            {
                Console.WriteLine("抱歉，该id不存在");
                DeleteEmployee();
            }
            else
            {
                for(int i = 0; i < employees.Count;)
                {
                    if(employees[i].Id == id)
                    {
                        employees.Remove(employees[i]);     
                    } 
                    else
                    {
                        i++;
                    }   
                }
                employeeIdExist[id] = false;
                Console.WriteLine("delete完成");
            }
        }

        /// <summary>
        /// 修改employee
        /// </summary>
        /// <param name="id"></param>
        public void UpdateEmployee()
        {
            Console.WriteLine("先删除再添加");
            DeleteEmployee();
            AddEmployee();
            Console.WriteLine("update完成");
        }
    }
}
