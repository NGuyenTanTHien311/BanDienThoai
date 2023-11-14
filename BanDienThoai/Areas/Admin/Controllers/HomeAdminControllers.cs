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
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();

        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        public ActionResult ThemSanPhamMoi(TDanhMucSp sanPham, IFormFile AnhDaiDien)
        {
            if (ModelState.IsValid)
            {
                if (AnhDaiDien != null && AnhDaiDien.Length > 0)
                {
                    // Lưu trữ ảnh vào thư mục trên máy chủ hoặc lưu trữ trong cơ sở dữ liệu (tùy chọn của bạn)
                    var imagePath = "/uploads/" + AnhDaiDien.FileName;
                    using (var stream = new FileStream("wwwroot" + imagePath, FileMode.Create))
                    {
                        AnhDaiDien.CopyTo(stream);
                    }
                    // Lưu đường dẫn ảnh vào đối tượng sản phẩm
                    sanPham.AnhDaiDien = imagePath;
                }

                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
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
    }


}