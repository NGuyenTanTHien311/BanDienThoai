using BanDienThoai.Models;
using BanDienThoai.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Diagnostics;
using X.PagedList;

namespace BanDienThoai.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlbanVaLiContext _db;
        
        private const string CARTKEY = "cart";

        public HomeController(QlbanVaLiContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = _db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        #region Search
        public IActionResult Search(string searchString, int? page)
        {
            var products = _db.TDanhMucSps.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                products = products.Where(p => p.TenSp.ToLower().Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = page ?? 1;
            var pagedList = products.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }
        #endregion
        #region San pham theo loai
        public IActionResult SanPhamTheoLoai(string maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = _db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }
        #endregion
        #region CHi tiet san pham
        public IActionResult ChiTietSanPham(string maSp)
        {
            var sanPham = _db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = _db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
            return View(sanPham);
        }
        #endregion
        #region ProductDetail
        public IActionResult ProductDetail(string maSp)
        {
            var sanPham = _db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = _db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            var homeProductDetailViewModel = new HomeProductDetailViewModel
            {
                danhMucSp = sanPham,
                anhSps = anhSanPham
            };
            return View(homeProductDetailViewModel);
        }
        #endregion
        #region Cart
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("addcart/{MaSp}")]
        public IActionResult AddToCart(string MaSp, int quantity)
        {
            var product = _db.TDanhMucSps.SingleOrDefault(x => x.MaSp == MaSp);

            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }

            var AnhDaiDien = _db.TDanhMucSps.Where(x => x.MaSp == MaSp).ToList();

            List<CartItem> cart = GetCartItems();
            var cartItem = cart.Find(p => p.Product.MaSp == MaSp);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.AnhDaiDien = AnhDaiDien;
            }
            else
            {
                cart.Add(new CartItem { Quantity = quantity, Product = product, AnhDaiDien = AnhDaiDien });
            }

            SaveCartSession(cart);

            return RedirectToAction(nameof(Cart));
        }

        [Route("/removecart/{MaSp}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] String MaSp)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.Product.MaSp == MaSp);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            SaveCartSession(cart);
              
            return RedirectToAction(nameof(Cart));
        }
       
        [Route("/Cart", Name = "Cart")]
        public IActionResult Cart()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }
   
                List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        void SaveCartSession(List<CartItem> cartItems)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(cartItems);
            session.SetString(CARTKEY, jsoncart);
        }
        #endregion
        #region Thêm Khách hàng
        [Route("ThemKH")]
        [HttpGet]
        public ActionResult ThemKH()
        {
            return View();
        }
        [Route("ThemKH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemKH(TKhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _db.TKhachHangs.Add(khachHang);
                _db.SaveChanges();
                return RedirectToAction("NhanVien");
            }
            return View(khachHang);
        }
        #endregion




    }
}
