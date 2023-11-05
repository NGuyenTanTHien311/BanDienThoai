using BanDienThoai.Models;
using BanDienThoai.Models.Authentication;
using BanDienThoai.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Globalization;


namespace BanDienThoai.Controllers
{
    public class HomeController : Controller
    {
        QlbanVaLiContext db=new QlbanVaLiContext();
        private List<TDanhMucSp> ShoppingCart = new List<TDanhMucSp>();
        private readonly ILogger<HomeController> _logger;
        
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
         
        }
       // [Authentication]

        public IActionResult Index(int ? page)
        {
            int pageSize = 8;
            int pageNumber=page==null||page<=0?1:page.Value;
            var lstsanpham=db.TDanhMucSps.AsNoTracking().OrderBy(x=>x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult Search(string searchString, int? page)
        {
            var products = db.TDanhMucSps.AsQueryable(); // Lấy danh sách sản phẩm từ database
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                products = products.Where(p => p.TenSp.ToLower().Contains(searchString)); // Tìm sản phẩm theo tên
            }

            int pageSize = 8;
            int pageNumber = page ?? 1;
            var pagedList = products.ToPagedList(pageNumber, pageSize); // Sử dụng thư viện PagedList để phân trang

            return View(pagedList);
        }

        public IActionResult SanPhamTheoLoai(String maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(X=>X.MaLoai==maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }
        public IActionResult ChiTietSanPham(string maSp)
        {
            var sanPham=db.TDanhMucSps.SingleOrDefault(x=> x.MaSp==maSp);
            var anhSanPham=db.TAnhSps.Where(x=>x.MaSp==maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
                return View(sanPham);
        }

        public IActionResult ProductDetail(string maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList(); var homeProductDetailViewModel=new HomeProductDetailViewModel { 
                danhMucSp = sanPham, anhSps=anhSanPham
            }
            ;
            return View(homeProductDetailViewModel);
        }

            public IActionResult Privacy()
        {
            return View();
        }

        public const string CARTKEY = "cart";

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        [Route("addcart/{MaSp}")]
        public IActionResult AddToCart(string MaSp)
        {
            var product = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == MaSp);
            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }

            // Xử lý đưa vào Cart
            List<CartItem> cart = GetCartItems();
            var cartItem = cart.Find(p => p.Product.MaSp == MaSp);
            if (cartItem != null)
            {
                // Sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên 1
                cartItem.Quantity++;
            }
            else
            {
                // Sản phẩm chưa tồn tại trong giỏ hàng, thêm mới
                cart.Add(new CartItem { Quantity = 1, Product = product });
            }

            // Lưu giỏ hàng vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }
        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> cartItems)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(cartItems);
            session.SetString(CARTKEY, jsoncart);
        }

        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] string MaSp)
        {
            var product = ShoppingCart.FirstOrDefault(p => p.MaSp == MaSp);
            if (product != null)
                ShoppingCart.Remove(product); // Xóa sản phẩm khỏi giỏ hàng

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Cart()
        {
            
            
            return View();
        }

        public IActionResult Checkout()
        {
            // Implement your checkout process using the shopping cart service
            // Redirect to a thank you page or order summary page

            return View();
        }


    }
}