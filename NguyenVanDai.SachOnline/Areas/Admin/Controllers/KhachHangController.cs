using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenVanDai.SachOnline.Models;
using PagedList;

namespace NguyenVanDai.SachOnline.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var sach = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
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
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                return View(kh);
            }
            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.DeleteAllOnSubmit(vietsach);
                db.SubmitChanges();

            }
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG kh, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                kh.HoTen = f["MaKH"];
                    kh.HoTen = f["hoten"];
                    kh.TaiKhoan = f["taikhoan"];
                    kh.MatKhau = f["matkhau"];
                    kh.Email = f["email"];
                    kh.DiaChi = f["DiaChi"];
                    kh.DienThoai = f["Dienthoai"];
                    kh.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);
                    db.KHACHHANGs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(kh);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == int.Parse(f["MaKh"]));
            if (ModelState.IsValid)
            {
                kh.HoTen = f["MaKH"];
                kh.HoTen = f["hoten"];
                kh.TaiKhoan = f["taikhoan"];
                kh.MatKhau = f["matkhau"];
                kh.Email = f["email"];
                kh.DiaChi = f["diachi"];
                kh.DienThoai = f["dienthoai"];
                kh.NgaySinh = Convert.ToDateTime(f["dNgaySinh"]);
                db.SubmitChanges();
                return RedirectToAction("Index");

            }
            return View(kh);
        }

    }
}