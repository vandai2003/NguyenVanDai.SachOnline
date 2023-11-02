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
    public class DonHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();
        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
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
            var ddh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (ddh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ddh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var ddh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (ddh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id).ToList();
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách nay đang có trong bảng chi tiết đặt hàng <br>" + "Nếu muốn xóa thì phải xóa hết mã sách này trong bảng chi tiết dặt hàng";
                return View(ddh);
            }
            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.DeleteAllOnSubmit(vietsach);

            }
            try
            {
                db.DONDATHANGs.DeleteOnSubmit(ddh);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.ThongBao = "Lỗi xảy ra khi xoá dơn đặt hàng đang được sử dụng";
                return View(ddh);
            }
        }
        /*
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DONDATHANG ddh, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                ddh.NgayDat = Convert.ToDateTime(f["dNgayDat"]);
                ddh.NgayGiao = Convert.ToDateTime(f["dNgayGiao"]);
                ddh.DaThanhToan = Convert.ToBoolean(f["DaThanhToan"]);
                ddh.TinhTrangGiaoHang = int.Parse(f["TinhTrangGiaoHang"]);
                db.DONDATHANGs.InsertOnSubmit(ddh);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            
            return View();
        }
        */
    }
}