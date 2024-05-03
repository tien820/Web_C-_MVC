using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.Ajax.Utilities;
using TTAnhNgu.App_Start;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace TTAnhNgu.Areas.Admin.Controllers
{
    [AdminAuthentication]
    public class StudentController : Controller
    {
        QLTTAVEntities db = new QLTTAVEntities();

        // GET: Admin/Student
        [HttpGet]
        public ActionResult getListStudent()
        {
            //Lay danh sach
            var listStudent = from s in db.HOC_VIEN select s;

            //Tim kiem
            //Kiem tra xem chuoi co du lieu chua
            //if (!String.IsNullOrEmpty(strSearch))
            //{
            //    listBook = listBook.Where(x => x.AuthorName.ToUpper().Contains(strSearch.ToUpper()) || x.Title.ToUpper().Contains(strSearch.ToUpper())
            //                                || x.Price.ToString().Contains(strSearch) || x.Year.ToString().Contains(strSearch));
            //}

            return View(listStudent);
        }

        private bool CheckEmail(string email)
        {
            return db.HOC_VIEN.Count(gv => gv.HV_EMAIL == email) > 0;
        }

        //Create Student
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost, ActionName("CreateStudent")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken] //Security help
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult CreateStudent([Bind(Include = "HV_MAHV, HV_HOTEN, HV_CCCD, HV_GIOITINH, HV_NGAYSINH, HV_DIACHI, " +
            "HV_SDT, HV_EMAIL, HV_MATKHAU, HV_HINHANH")] HOC_VIEN student, HttpPostedFileBase file)
        {
            var students = db.HOC_VIEN;
            try
            {
                if (ModelState.IsValid)
                {                   
                    if (file != null && !CheckEmail(student.HV_EMAIL))
                    {
                        var newFileName = Guid.NewGuid();
                        var extension = Path.GetExtension(file.FileName);
                        string newName = newFileName + extension;
                        string fileName = Path.GetFileName(newName);
                        string path = Path.Combine(Server.MapPath("~/Content/Assets/Admin/assets/img/"), fileName);
                        file.SaveAs(path);
                        student.HV_HINHANH = fileName;
                    }
                    else
                    {
                        student.HV_HINHANH = null;
                    }
                    if (CheckEmail(student.HV_EMAIL))
                    {
                        ModelState.AddModelError("", "Email đã tồn tại");
                        return View();
                    }
                    else
                    {
                        students.Add(student);
                        db.SaveChanges();
                    }
                    //students.Add(student);
                    //db.SaveChanges();
                }
            }
            catch (RetryLimitExceededException /*dex*/)
            {
                ModelState.AddModelError("", "Error save data");
            }
            return RedirectToAction("getListStudent");
        }

        //Edit student
        [HttpGet]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOC_VIEN student = db.HOC_VIEN.SingleOrDefault(s => s.HV_MAHV == id);
            if (student.HV_SDT != null)
                student.HV_SDT = student.HV_SDT.ToString().Trim();
            if (student.HV_CCCD != null)
                student.HV_CCCD = student.HV_CCCD.ToString().Trim();
            return View(student);
        }

        [HttpPost, ActionName("EditStudent")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult EditStudent(int id, HttpPostedFileBase fileEdit)
        {
            bool checkMail = false;
            var studentUpdate = db.HOC_VIEN.Find(id);
            var check = db.HOC_VIEN.Where(ndk => ndk.HV_MAHV != studentUpdate.HV_MAHV).ToList();

            if (TryUpdateModel(studentUpdate, "", new string[] { "HV_HOTEN", "HV_CCCD", "HV_GIOITINH", "HV_NGAYSINH", "HV_DIACHI",
            "HV_SDT", "HV_EMAIL", "HV_MATKHAU", "HV_HINHANH" }))
            {
                try
                {
                    if (fileEdit != null)
                    {
                        foreach (var item in check)
                        {
                            if (item.HV_EMAIL == studentUpdate.HV_EMAIL)
                            {
                                checkMail = true;
                                break;
                            }
                            else
                            {
                                checkMail = false;
                            }
                        }
                        if (checkMail)
                        {
                            ModelState.AddModelError("", "Email đã tồn tại");
                            return View("EditStudent", studentUpdate);
                        }
                        else
                        {
                            db.SaveChanges();
                        }
                        var newFileName = Guid.NewGuid();
                        var extension = Path.GetExtension(fileEdit.FileName);
                        string newName = newFileName + extension;
                        string fileName = Path.GetFileName(newName);
                        string path = Path.Combine(Server.MapPath("~/Content/Assets/Admin/assets/img/"), fileName);

                        if (Path.GetExtension(studentUpdate.HV_HINHANH) != fileName && studentUpdate.HV_HINHANH != null)
                        {
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/Assets/Admin/assets/img/"), studentUpdate.HV_HINHANH));
                        }
                        studentUpdate.HV_HINHANH = fileName;
                        fileEdit.SaveAs(path);
                        db.Entry(studentUpdate).State = EntityState.Modified;
                        if (studentUpdate.HV_NGAYSINH != null)
                            studentUpdate.HV_NGAYSINH = string.Format("{0:yyyy-MM-dd}", studentUpdate.HV_NGAYSINH).AsDateTime();
                        else
                            studentUpdate.HV_NGAYSINH = null;
                        
                        //db.SaveChanges();
                    }
                    db.Entry(studentUpdate).State = EntityState.Modified;
                    if (studentUpdate.HV_NGAYSINH != null)
                        studentUpdate.HV_NGAYSINH = string.Format("{0:yyyy-MM-dd}", studentUpdate.HV_NGAYSINH).AsDateTime();
                    else
                        studentUpdate.HV_NGAYSINH = null;
                    foreach (var item in check)
                    {
                        if (item.HV_EMAIL == studentUpdate.HV_EMAIL)
                        {
                            checkMail = true;
                            break;
                        }
                        else
                        {
                            checkMail = false;
                        }
                    }
                    if (checkMail)
                    {
                        ModelState.AddModelError("", "Email đã tồn tại");
                        return View("EditStudent", studentUpdate);
                    }
                    else
                    {
                        db.SaveChanges();
                    }
                    //db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Error Save Data");
                }
            }

            return RedirectToAction("getListStudent");
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
            var viewModel = db.HOC_VIEN.Include(s => s.NGAY_DANG_KY).SingleOrDefault(s => s.HV_MAHV == id);
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
            HOC_VIEN student = db.HOC_VIEN.SingleOrDefault(n => n.HV_MAHV == id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(student);
        }

        //Xác nhận xoá
        [HttpPost, ActionName("Delete")]
        [AdminAuthorization(Role = "ADMIN")]
        public ActionResult Delete(int id)
        {
            HOC_VIEN student = db.HOC_VIEN.SingleOrDefault(n => n.HV_MAHV == id);
            var DeleteList = db.NGAY_DANG_KY.Where(ndk => ndk.HV_MAHV == id).ToList();
            if (student.HV_HINHANH != null)
            {
                var path = Path.Combine(Server.MapPath("~/Content/Assets/Admin/assets/img/"), student.HV_HINHANH);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            if (student == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NGAY_DANG_KY.RemoveRange(DeleteList);
            db.HOC_VIEN.Remove(student);
            db.SaveChanges();
            return RedirectToAction("getListStudent");
        }
    }
}