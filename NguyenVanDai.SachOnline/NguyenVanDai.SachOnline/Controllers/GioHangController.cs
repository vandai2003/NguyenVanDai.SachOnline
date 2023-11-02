using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenVanDai.SachOnline.Models;

namespace NguyenVanDai.SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        dbSachOnlineDataContext db = new dbSachOnlineDataContext();

        // GET: GioHang
        public List<GioHang> LaygioHang()
        {
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;

        }
        public ActionResult ThemGiohang(int ms, string url)
        {
            List<GioHang> lstGiohang = LaygioHang();
            GioHang sp = lstGiohang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGiohang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;

            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstgiohang = Session["GioHang"] as List<GioHang>;
            if (lstgiohang != null)
            {
                iTongSoLuong = lstgiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongtien = 0;
            List<GioHang> lstgiohang = Session["GioHang"] as List<GioHang>;
            if (lstgiohang != null)
            {
                dTongtien = lstgiohang.Sum(n => n.dThanhtien);
            }
            return dTongtien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstgiohang = LaygioHang();
            if (lstgiohang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.tongsoluong = TongSoLuong();
            ViewBag.tongtien = TongTien();
            return View(lstgiohang);
        }
        public ActionResult GioHangPatial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSpKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LaygioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null) {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if(lstGioHang.Count  == 0) {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f) {
            List<GioHang> lstGioHang = LaygioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LaygioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Use");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LaygioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LaygioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}