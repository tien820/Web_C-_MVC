using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTAnhNgu.App_Start
{
    public class AdminAuthentication : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            GIAO_VIEN teacher = (GIAO_VIEN)HttpContext.Current.Session["user"];
            if (teacher == null)
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            controller = "Home",
                            action = "Login",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
            }
        }
    }
}