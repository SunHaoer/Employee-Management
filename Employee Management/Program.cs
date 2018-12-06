using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management
{
    class Program
    {
        public static Gender M { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("欢迎登录Employee_Management\n查看所有Employee请按0\n添加Employee请按1\n删除Employee请按2\n修改Employee请按3\n退出请按#");
            string key = Console.ReadLine();
            EmployeeController controller = new EmployeeController();
            while (!"#".Equals(key))
            {
                if("0".Equals(key))
                {
                    controller.DisplayEmployees();
                }
                else if("1".Equals(key))
                {
                    controller.AddEmployee();
                }
                else if("2".Equals(key))
                {
                    controller.DisplayEmployees();
                    Console.WriteLine("请输入需要删除的id:");
                    int id = Int32.Parse(Console.ReadLine());
                    controller.DeleteEmployee(id);
                }
                else if ("3".Equals(key))
                {
                    controller.DisplayEmployees();
                    Console.WriteLine("请输入需要更改的id");
                    int id = Int32.Parse(Console.ReadLine());
                    controller.UpdateEmployee(id);
                }  
                else
                {
                    Console.WriteLine("请合法输入");
                }
                Console.WriteLine("查看所有Employee请按0\n添加Employee请按1\n删除Employee请按2\n修改Employee请按3\n退出请按#");
                key = Console.ReadLine();
            }
        }
    }
}
