using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTAnhNgu.App_Start;

namespace TTAnhNgu.Areas.Admin.Controllers
{
    [AdminAuthentication]
    public class TeacherController : Controller
    {
        QLTTAVEntities db = new QLTTAVEntities();

        // GET: Admin/Teacher
        [HttpGet]
        public ActionResult getListTeacher()
        {
            //Lay danh sach
            var listTeacher = from s in db.GIAO_VIEN select s;

            //Tim kiem
            //Kiem tra xem chuoi co du lieu chua
            //if (!String.IsNullOrEmpty(strSearch))
            //{
            //    listBook = listBook.Where(x => x.AuthorName.ToUpper().Contains(strSearch.ToUpper()) || x.Title.ToUpper().Contains(strSearch.ToUpper())
            //                                || x.Price.ToString().Contains(strSearch) || x.Year.ToString().Contains(strSearch));
            //}

            return View(listTeacher);
        }

        //Create Teacher
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult CreateTeacher()
        {
            return View();
        }

        [HttpPost, ActionName("CreateTeacher")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken] //Security help
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult CreateTeacher([Bind(Include = "GV_MAGV, NND_MANHOM, GV_HOTEN, GV_CCCD, GV_GIOITINH, GV_NGAYSINH, GV_DIACHI, " +
            "GV_SDT, GV_EMAIL, GV_MATKHAU, GV_TRINHDOTA")] GIAO_VIEN teacher)
        {
            var teachers = db.GIAO_VIEN;
            try
            {
                if (ModelState.IsValid)
                {
                    teachers.Add(teacher);
                    db.SaveChanges();
                }
            }
            catch (RetryLimitExceededException /*dex*/)
            {
                ModelState.AddModelError("", "Error save data");
            }
            return RedirectToAction("getListTeacher");
        }

        //Edit Teacher
        [HttpGet]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult EditTeacher(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIAO_VIEN teacher = db.GIAO_VIEN.SingleOrDefault(s => s.GV_MAGV == id);
            return View(teacher);
        }

        [HttpPost, ActionName("EditTeacher")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult EditTeacher(int id)
        {
            var teacherUpdate = db.GIAO_VIEN.Find(id);
            if (TryUpdateModel(teacherUpdate, "", new string[] { "NND_MANHOM", "GV_HOTEN", "GV_CCCD", "GV_GIOITINH", "GV_NGAYSINH", "GV_DIACHI",
            "GV_SDT", "GV_EMAIL", "GV_MATKHAU", "GV_TRINHDOTA" }))
            {
                try
                {
                    db.Entry(teacherUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }

            return RedirectToAction("getListTeacher");
        }

        //Hiển thị chi tiết
        [HttpGet]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            //Bao gồm tất cả danh sách theo id chỉ định
            var viewModel = db.GIAO_VIEN.Include(s => s.NHOM_NGUOI_DUNG).SingleOrDefault(s => s.GV_MAGV == id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        //Hiển thị thông tin cần xoá
        [HttpGet]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult Delete(int? id)
        {
            GIAO_VIEN teacher = db.GIAO_VIEN.SingleOrDefault(n => n.GV_MAGV == id);
            if (teacher == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(teacher);
        }

        //Xác nhận xoá
        [HttpPost, ActionName("Delete")]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult Delete(int id)
        {
            GIAO_VIEN teacher = db.GIAO_VIEN.SingleOrDefault(n => n.GV_MAGV == id);
            if (teacher == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.GIAO_VIEN.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("getListTeacher");
        }
    }
}