using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizz.Common
{
    public class CheckCredentialAttribute : AuthorizeAttribute
    {
        public string Role_ID { set; get; }
        int checkSession = 0;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            List<string> Admin = new List<string> { "1", "2" };
            List<string> Customer = new List<string> { "2" };
            UserLogin session = (UserLogin)HttpContext.Current.Session["account"];
            RoleLogin role = (RoleLogin)HttpContext.Current.Session["role"];
            if (session == null)
            {
                checkSession = 1;
                return false;
            }
            if (role.Role == 1)
            {
                if (Admin.Contains(this.Role_ID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (role.Role == 0)
            {
                if (Customer.Contains(this.Role_ID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (checkSession == 1)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Auth/Login.cshtml"
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/NotFound/Index.cshtml"
                };
            }

        }
    }
}