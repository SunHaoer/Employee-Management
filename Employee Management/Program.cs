using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Employee_Management
{
    class Program
    {
        public static Gender M { get; private set; }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        private static void Login()
        {
            string username, password;
            do
            {
                Console.WriteLine("请输入任意username和password:");
                username = Console.ReadLine();
                password = Console.ReadLine();
            } while (username == null && password == null);
            Console.WriteLine("欢迎登录" + username);
        }

        /// <summary>
        /// 输出菜单
        /// </summary>
        private static void OutputMenu()
        {
            Console.WriteLine("查看所有Employee请按0\n添加Employee请按1\n删除Employee请按2\n修改Employee请按3\n退出请按#");
        }

        /// <summary>
        /// 执行主要功能
        /// </summary>
        private static void Action()
        {
            string key = Console.ReadLine();
            EmployeeController controller = new EmployeeController();
            while (!"#".Equals(key))
            {
                if ("0".Equals(key))    // 显示全部
                {
                    controller.DisplayEmployees();
                }
                else if ("1".Equals(key))    // 添加
                {
                    controller.AddEmployee();
                }
                else if ("2".Equals(key))    // 删除
                {
                    controller.DisplayEmployees();
                    controller.DeleteEmployee();
                }
                else if ("3".Equals(key))    // 修改
                {
                    controller.DisplayEmployees();
                    controller.UpdateEmployee();
                }
                else
                {
                    Console.WriteLine("请合法输入");
                }
                OutputMenu();
                key = Console.ReadLine();
            }
            Console.WriteLine("感谢使用, 再见");
        }

        static void Main(string[] args)
        {
            Login();
            OutputMenu();
            Action();
        }
    }
}
