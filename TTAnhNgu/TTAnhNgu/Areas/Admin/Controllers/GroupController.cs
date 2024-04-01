using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTAnhNgu.App_Start;
namespace TTAnhNgu.Areas.Admin.Controllers
{

    [AdminAuthentication]
    [AdminAuthorization(Role = "ADMIN")]
    public class GroupController : Controller
    {
        QLTTAVEntities db = new QLTTAVEntities();

        // GET: Admin/Group
        [HttpGet]
        public ActionResult getListGroup()
        {
            //Lay danh sach
            var listGroup = from s in db.NHOM_NGUOI_DUNG select s;

            //Tim kiem
            //Kiem tra xem chuoi co du lieu chua
            //if (!String.IsNullOrEmpty(strSearch))
            //{
            //    listBook = listBook.Where(x => x.AuthorName.ToUpper().Contains(strSearch.ToUpper()) || x.Title.ToUpper().Contains(strSearch.ToUpper())
            //                                || x.Price.ToString().Contains(strSearch) || x.Year.ToString().Contains(strSearch));
            //}
            return View(listGroup);
        }

        //Create Group
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost, ActionName("CreateGroup")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken] //Security help
        public ActionResult CreateGroup([Bind(Include = "NND_MANHOM, NND_TENNHOMND, GIAO_VIEN")] NHOM_NGUOI_DUNG group)
        {
            var groups = db.NHOM_NGUOI_DUNG;
            try
            {
                if (ModelState.IsValid)
                {
                    groups.Add(group);
                    db.SaveChanges();
                }
            }
            catch (RetryLimitExceededException /*dex*/)
            {
                ModelState.AddModelError("", "Error save data");
            }
            return RedirectToAction("getListGroup");
        }

        //Edit Group
        [HttpGet]
        public ActionResult EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHOM_NGUOI_DUNG group = db.NHOM_NGUOI_DUNG.SingleOrDefault(s => s.NND_MANHOM == id);
            return View(group);
        }

        [HttpPost, ActionName("EditGroup")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(int id)
        {
            var groupUpdate = db.NHOM_NGUOI_DUNG.Find(id);
            if (TryUpdateModel(groupUpdate, "", new string[] { "NND_TENNHOMND" }))
            {
                try
                {
                    db.Entry(groupUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }

            return RedirectToAction("getListGroup");
        }

        //Hiển thị chi tiết
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            //Bao gồm tất cả danh sách theo id chỉ định
            var viewModel = db.NHOM_NGUOI_DUNG.Include(s => s.GIAO_VIEN).SingleOrDefault(s => s.NND_MANHOM == id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        //Hiển thị thông tin cần xoá
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            NHOM_NGUOI_DUNG group = db.NHOM_NGUOI_DUNG.SingleOrDefault(n => n.NND_MANHOM == id);
            if (group == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(group);
        }

        //Xác nhận xoá
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            NHOM_NGUOI_DUNG group = db.NHOM_NGUOI_DUNG.SingleOrDefault(n => n.NND_MANHOM == id);
            if (group == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NHOM_NGUOI_DUNG.Remove(group);
            db.SaveChanges();
            return RedirectToAction("getListGroup");
        }
    }
}