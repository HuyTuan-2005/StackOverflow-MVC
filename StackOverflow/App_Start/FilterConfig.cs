using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using System;
using System.Net;


namespace StackOverflow
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class AdminAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra điều kiện xác thực ở mỗi request
            var session = filterContext.HttpContext.Session;
            if (session == null || session["UserConnStr"] == null)
            {
                // Chuyển hướng sang trang login của Admin hoặc trang thông báo
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "area", "Admin" },
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
                return;
            }
            else
            {
                return;
            }
        }

    }

}