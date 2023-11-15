using BanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BanDienThoai.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminControllers : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [Route("")]
        [Route("index")]
        public ActionResult index()
        {
            return View();
        }
        [Route("DanhMucSanPham")]
        public ActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 14;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        #region Thêm Sản PHẩm mới
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public ActionResult ThemSanPhamMoi()
        {
            
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();

        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSanPhamMoi(TDanhMucSp sanPham )
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
        }
        #endregion
        #region Chỉnh sửa sản phẩm
        [Route("SuaSanPham")]
        [HttpGet]
        public ActionResult SuaSanPham(String maSanPham)
        {

            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham = db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);

        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham", "HomeAdminControllers");
            }
            return View(sanPham);
        }
        #endregion
        #region xóa sản phẩm
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var chiTietSanPhams = db.TChiTietSanPhams.Where(x => x.MaSp == maSanPham).ToList();
            if (chiTietSanPhams.Count() > 0)
            {
                TempData["Message"] = "không xóa được sản phẩm này";
                    return RedirectToAction("DanhMucSanPham", "HomeAdminControllers");
            }
            var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == maSanPham);
            if (anhSanPhams.Any()) db.RemoveRange(anhSanPhams);
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMucSanPham", "HomeAdminControllers");
        }
            #endregion
            #region QuanLyDonHang
            [Route("QuanLyDonHang")]
        public ActionResult QuanLyDonHang(int? page)
        {
            int pageSize = 14;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = db.THoaDonBans.AsNoTracking().OrderBy(x => x.MaHoaDon);
            PagedList<THoaDonBan> lst = new PagedList<THoaDonBan>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        #endregion
        #region QuanLyLoaiSP
        [Route("QuanLyLoaiSP")]
        public ActionResult QuanLyLoaiSP(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = db.TLoaiSps.AsNoTracking().OrderBy(x => x.MaLoai);
            PagedList<TLoaiSp> lst = new PagedList<TLoaiSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        #endregion
        #region Thêm Loại Sản PHẩm Mới
        [Route ("ThemLoaiSPMoi")]
        [HttpGet]
        public ActionResult ThemLoaiSPMoi()
        {
            return View();
        }
        [Route("ThemLoaiSPMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemLoaiSPMoi(TLoaiSp LoaiSP)
        {
            if (ModelState.IsValid)
            {
                db.TLoaiSps.Add(LoaiSP);
                db.SaveChanges();
                return RedirectToAction("QuanLyLoaiSP");
            }
            return View(LoaiSP);
        }
        #endregion
        #region xóa loại SP
        [Route("XoaLoaiSP")]
        [HttpGet]
        public IActionResult XoaLoaiSP(string LoaiSP)
        {
            TempData["Message"] = "";
           
            db.Remove(db.TLoaiSps.Find(LoaiSP));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("QuanLyLoaiSP", "HomeAdminControllers");
        }
        #endregion
    }


}