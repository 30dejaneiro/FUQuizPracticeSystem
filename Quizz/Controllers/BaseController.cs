using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizz.Controllers
{
    public class BaseController : Controller
    {
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var session = Session["account"];
        //    if (session == null)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Auth", action = "Login" }));
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "success-toast";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "warning-toast";
            }
            if (type == "error")
            {
                TempData["AlertType"] = "error-toast";
            }
        }
    }
}
