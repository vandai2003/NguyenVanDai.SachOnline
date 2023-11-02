using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenVanDai.SachOnline.Models;

namespace NguyenVanDai.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        dbSachOnlineDataContext data = new dbSachOnlineDataContext();
        ///<summary>
        ///LaySachMoi
        ///</summary>
        ///<param name="count">int</param>
        ///<returns>List</returns>
        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a=>a.NgayCapNhat).Take(count).ToList();
        }
        //GET: SachOnline
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }
        //===============================================================
        public ActionResult SachTheoChuDe(int id)
        {
            var sach = from s in data.SACHes where s.MaCD==id select s;
            return View(sach);
        }
        ///<summary>
        ///GetChuDe
        ///</summary>
        ///<returns>ReturnChuDe</returns>
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }
        //===============================================================
        ///<summary>
        ///GetNhaXuatBan
        ///</summary>
        ///<returns>ReturnNhaXuatBan</returns>
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(listNhaXuatBan);
        }
        ///<summary>
        ///GetNav
        ///</summary>
        ///<returns>ReturnNav</returns>
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        ///<summary>
        ///GetFooter
        ///</summary>
        ///<returns>ReturnFooter</returns>
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
         ///<summary>
         ///GetSachBanNhieu
         ///</summary>
         ///<returns>ReturnSachBanNhieu</returns>
         public ActionResult SachBanNhieuPartial()
         {
             return PartialView();
         }

        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in data.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }
    }
}