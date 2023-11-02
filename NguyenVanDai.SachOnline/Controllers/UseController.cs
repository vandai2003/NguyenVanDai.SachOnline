    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.Security;
    using NguyenVanDai.SachOnline.Models;

    namespace NguyenVanDai.SachOnline.Controllers
    {
        public class UseController : Controller
        {
            dbSachOnlineDataContext db = new dbSachOnlineDataContext();
            // GET: Use
            public ActionResult Index()
            {
                return View();
            }


            [HttpGet]
            public ActionResult Dangky() {
                return View();
            }

            [HttpPost]
            public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
            {
                var sHoTen = collection["HoTen"];
                var sTenDN = collection["tenDN"];
                var sMatKhau = collection["MatKhau"];
                var SMatKhauNhapLai = collection["MatKhauNL"];
                var sDiaChi = collection["DiaChi"];
                var sEmail = collection["Email"];
                var sDienThoai = collection["DienThoai"];
                var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
                if (String.IsNullOrEmpty(sHoTen))
                {
                    ViewData["err1"] = "Họ và tên không được rỗng";
                }
                else if (String.IsNullOrEmpty(sTenDN))
                {
                    ViewData["err2"] = "Tên đăng nhập không được rỗng";
                }
                else if (String.IsNullOrEmpty(sMatKhau))
                {
                    ViewData["err3"] = "Phải nhập mật khâu";
                }
                else if (String.IsNullOrEmpty(SMatKhauNhapLai))
                {
                    ViewData["err4"] = "Phải nhập lại mật khâu";
                }
                else if (sMatKhau != SMatKhauNhapLai)
                {
                    ViewData["err4"] = "Nhập lại mật khâu";
                }
                else if (String.IsNullOrEmpty(sEmail))
                {
                    ViewData["err5"] = "Email không được rỗng";
                }
                else if (String.IsNullOrEmpty(sDienThoai))
                {
                    ViewData["err6"] = "Số điện thoại không được rỗng";
                }
                else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
                {
                    ViewBag.ThongBao = "tên đăng nhập đã tồn tại";
                }
                else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
                {
                    ViewBag.ThongBao = "Email đã sử dụng";
                }
                else
                {
                    kh.HoTen = sHoTen;
                    kh.TaiKhoan = sTenDN;
                    kh.MatKhau = sMatKhau;
                    kh.Email = sEmail;
                    kh.DiaChi = sDiaChi;
                    kh.DienThoai = sDienThoai;
                    kh.NgaySinh = DateTime.Parse(dNgaySinh);
                    db.KHACHHANGs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
                return this.Dangky();
            }

            [HttpGet]
            public ActionResult DangNhap()
            {
                return View();
            }
            [HttpPost]
            public ActionResult DangNhap(FormCollection collection, string returnUrl)
            {
                var sTenDn = collection["TenDN"];
                var sMatKhau = collection["MatKhau"];
                if (String.IsNullOrEmpty(sTenDn))
                {
                    ViewData["Err1"] = "bạn chưa nhập tên đăng nhập";   
                }
                else if(String.IsNullOrEmpty(sMatKhau))
                {
                    ViewData["Err2"] = "phải nhập mật khẩu";
                }
                else
                {
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDn && n.MatKhau == sMatKhau);
                    if(kh != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        Session["TaiKhoan"] = kh;

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("Index", "SachOnline"); 
                    }

                    return Redirect(returnUrl);
                }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
                return View();
            }
        }
    }
