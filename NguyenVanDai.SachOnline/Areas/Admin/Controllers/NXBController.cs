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
    public class NXBController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/NXB
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Details(int id)
        {
            var sach = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
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
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách nay đang có tong bảng chi tiết đặt hàng <br>" + "Nếu muốn xóa thì phải xóa hết mã sách này trong bảng chi tiết dặt hàng";
                return View(nxb);
            }
            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.DeleteAllOnSubmit(vietsach);
                db.SubmitChanges();
            }
            db.NHAXUATBANs.DeleteOnSubmit(nxb);
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
        public ActionResult Create(NHAXUATBAN nxb, FormCollection f)
        {
            if (ModelState.IsValid)
                {
                nxb.TenNXB = f["sTenNXB"];
                nxb.DiaChi = f["sDiaChi"];
                nxb.DienThoai = f["DienThoai"];
                    db.NHAXUATBANs.InsertOnSubmit(nxb);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nxb);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == int.Parse(f["MaNXB"]));
            if (ModelState.IsValid)
            {
                nxb.TenNXB = f["sTenNXB"];
                nxb.DiaChi = f["sDiaChi"];
                nxb.DienThoai = f["DienThoai"];
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

    }
}