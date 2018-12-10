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
            Console.WriteLine("查看所有Employee请按0\n添加Employee请按1\n删除Employee请按2\n修改Employee请按3\n查看单个Employee请按4\n退出请按#");
        }

        /// <summary>
        /// 执行主要功能
        /// </summary>
        private static void Action()
        {
            string key = Console.ReadLine();
            Manage manage = new Manage();
            while (!"q".Equals(key))
            {
                switch (key)
                {
                    case "0": manage.DisplayEmployees(); break;
                    case "1": manage.AddEmployee(); break;
                    case "2": manage.DeleteEmployee(); break;
                    case "3": manage.UpdateEmployee(); break;
                    case "4": manage.DisplayOneEmployee(); break;
                    default: Console.WriteLine("请合法输入"); break;
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
