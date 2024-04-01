using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace TTAnhNgu.App_Start
{
    public class AdminAuthorization : AuthorizeAttribute
    {
        QLTTAVEntities db = new QLTTAVEntities();

        public string Role { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            GIAO_VIEN teacher = (GIAO_VIEN)HttpContext.Current.Session["user"];
            if (teacher != null)
            {
                var count = db.NHOM_NGUOI_DUNG.Count(s => s.NND_MANHOM == teacher.NND_MANHOM && s.NND_TENNHOMND.ToUpper() == Role);
                if (count != 0)
                {
                    return;
                }
                else
                {
                    var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Error",
                                area = "Admin",
                                returnUrl = returnUrl.ToString()
                            }));
                }
            }
            else
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