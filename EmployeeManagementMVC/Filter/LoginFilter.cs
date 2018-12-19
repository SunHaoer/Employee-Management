using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using EmployeeManagementMVC.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementMVC.Filter
{
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.GetString("loginUsername") == null)
            {
                if(context.Controller.GetType() != typeof(EmployeesLoginController))
                {
                    context.Result = new RedirectResult("/EmployeesLogin/Login");
                }
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
