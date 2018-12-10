using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Employee_Management
{
    class Manage
    {
        public const int MAX = 100000;    // 最大employee数
        public List<Employee> employees = new List<Employee>();
        public bool[] employeeIdExist = new bool[MAX];    // 表示该employeeId是否存在
        public int maxId = 0;

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
            employee.Id = CreateEmployeeId();  
            Console.WriteLine("自动生成id为" + employee.Id);
            employee = InputInformation(employee);
            employeeIdExist[employee.Id] = true;
            employees.Add(employee);
            Console.WriteLine("add完成");
        }

        /// <summary>
        /// 输入各种信息
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private Employee InputInformation(Employee employee)
        {
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
            return employee;
        }

        /// <summary>
        /// id为最大id+1
        /// </summary>
        /// <returns></returns>
        private int CreateEmployeeId()
        {
            return ++maxId;
        }

        /// <summary>
        /// 输入字符串
        /// </summary>
        /// <returns></returns>
        private string InputString()
        {
            return Console.ReadLine();
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
                    Console.WriteLine("请输入M/F:");
                }
                input = Console.ReadLine();
                isFirst = false;
            } while (!("M".Equals(input) || "F".Equals(input)));
            if ("M".Equals(input))
            {
                return Gender.F;
            }
            else
            {
                return Gender.M;
            }
        }

        /// <summary>
        /// 输入日期
        /// </summary>
        /// <returns></returns>
        private DateTime InputBirth()
        {
            int year, month, day;
            bool isLegal = true;
            do
            {
                if(!isLegal)
                {
                    Console.WriteLine("请按正确格式输入");
                }
                Console.Write("请输入year:");
                year = InputNumber(0, 9999);
                Console.Write("请输入month:");
                month = InputNumber(1, 12);
                Console.Write("请输入day:");
                day = InputNumber(1, 31);
                isLegal = JudgeDate(year, month, day);
            } while (!isLegal);
           return new DateTime(year, month, day);
        }

        /// <summary>
        /// 获取输入的整数
        /// </summary>
        /// <returns></returns>
        private int InputNumber(int minn, int maxn)
        {
            int num;
            bool isLegal = true;
            string input;
            do
            {
                if(!isLegal)
                {
                    Console.WriteLine("请输入整数");
                }
                input = Console.ReadLine();
                isLegal = Int32.TryParse(input, out num);
                isLegal = (num >= minn && num <= maxn);
            } while (!isLegal);
            return num;
        }

        /// <summary>
        /// 判断日期是否合法
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private bool JudgeDate(int year, int month, int day)
        {
            int[,] yearDate = new int[2, 13] { { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }, { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 } };    // 平年和闰年的日历
            int yearType = 0;    // 0表示平年
            if((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
            {
                yearType = 1;    // 1表示闰年
            }
            if(month < 1 || month > 12)
            {
                return false;    // 月份不合法
            }
            if(day > yearDate[yearType, month])
            {
                return false;    // 日期不合法
            }
            return true;
        }

        /// <summary>
        /// 输入address
        /// </summary>
        /// <returns></returns>
        private string InputAddress()
        {
            return InputString();
        }

        /// <summary>
        /// 输入phone
        /// </summary>
        /// <returns></returns>
        private string InputPhone()
        {
            string regex = @"^[1]\d{10,10}$", input;
            bool isLegal = true;
            do
            {
                if (!isLegal)
                {
                    Console.WriteLine("请输入11位手机号码");
                }
                input = Console.ReadLine().Trim();
                isLegal = Regex.IsMatch(input, regex);
            } while (!isLegal);
            return input;

        }

        /// <summary>
        /// delete
        /// </summary>
        public void DeleteEmployee()
        {
            DisplayEmployees();
            Console.WriteLine("请输入需要删除的id：");
            int id = InputNumber(0, MAX);
            if(!employeeIdExist[id])
            {
                Console.WriteLine("抱歉，该id不存在");
            }
            else
            {
                employees.Remove(employees.Find(item => item.Id == id));
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
            DisplayEmployees();
            Console.WriteLine("请输入需要修改的id：");
            int id = InputNumber(0, MAX);
            if (!employeeIdExist[id])
            {
                Console.WriteLine("抱歉，该id不存在");
            }
            else
            {
                Employee employee =  employees.Find(item => item.Id == id);
                Console.WriteLine(employee);
                employee = InputInformation(employee);
            }
        }

        /// <summary>
        /// 显示单个Employee信息
        /// </summary>
        public void DisplayOneEmployee()
        {
            Console.WriteLine("请输入需要查看的id：");
            int id = InputNumber(0, MAX);
            if (!employeeIdExist[id])
            {
                Console.WriteLine("抱歉，该id不存在");
                DisplayOneEmployee();
            }
            else
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    if (employees[i].Id == id)
                    {
                        Console.WriteLine(employees[i].ToString());
                    }
                }
                employeeIdExist[id] = false;
                Console.WriteLine("delete完成");
            }
        }
    }
}
