using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using NguyenVanDai.SachOnline.Models;
using PagedList;

namespace NguyenVanDai.SachOnline.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/ChuDe
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Details(int id)
        {
            var sach = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id).ToList();
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Chủ đề đang được sử dụng ở liên kết khác <br>" + "Nếu muốn xóa thì phải xóa hết trong bảng chi tiết dặt hàng";
                return View(chude);
            }
            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.DeleteAllOnSubmit(vietsach);
                db.SubmitChanges();
            }
            try
            {
                db.CHUDEs.DeleteOnSubmit(chude);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.ThongBao = "Lỗi xảy ra khi xoá";
                return View(chude);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CHUDE chude, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                chude.TenChuDe = f["TenChuDe"];

                    db.CHUDEs.InsertOnSubmit(chude);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
            }
            return View();
        }

    }
}