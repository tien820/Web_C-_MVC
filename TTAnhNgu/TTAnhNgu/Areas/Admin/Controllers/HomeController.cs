using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using TTAnhNgu.App_Start;
using static System.Collections.Specialized.BitVector32;

namespace TTAnhNgu.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLTTAVEntities db = new QLTTAVEntities();
        // GET: Admin/Home
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult Login()
        {

            if (Session["user"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("getListTeacher", "Teacher");
            }            
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Login(string user, string password)
        {
            if (user != null && password != null)
            {
                var account = db.GIAO_VIEN.SingleOrDefault(s => s.GV_EMAIL.ToUpper() == user.ToUpper() && s.GV_MATKHAU == password);
                if (account != null)
                {
                    Session["user"] = account;
                    return RedirectToAction("getListTeacher", "Teacher");
                }
            }                       
            TempData["error"] = "Tài khoản hoặc mật khẩu chưa đúng";
            return View();    
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}