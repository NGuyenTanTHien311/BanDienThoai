using Microsoft.AspNetCore.Mvc;
using  BanDienThoai.Models;


namespace BanDienThoai.Controllers
{
    public class AccessController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
            [HttpPost]

        public IActionResult Login(TUser user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Username);

                    // Kiểm tra LoaiUser, nếu là 1 (admin) chuyển hướng đến trang admin, ngược lại chuyển hướng đến trang chính
                    if (u.LoaiUser == 1)
                    {
                        return RedirectToAction("Index", "HomeAdminControllers", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                // Tạo đối tượng TUser để truyền vào biểu mẫu đăng ký
                TUser user = new TUser();
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Register(TUser user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xác nhận mật khẩu
                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
                    return View(user);
                }

                // Kiểm tra xem người dùng đã tồn tại chưa
                var existingUser = db.TUsers.FirstOrDefault(x => x.Username == user.Username);
                if (existingUser == null)
                {
                    // Thêm người dùng mới vào cơ sở dữ liệu
                    db.TUsers.Add(user);
                    db.SaveChanges();

                    // Đăng nhập người dùng sau khi đăng ký thành công
                    HttpContext.Session.SetString("UserName", user.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username already exists. Please choose a different one.");
                }
            }

            // Nếu có lỗi, hiển thị lại form đăng ký với các thông báo lỗi
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }

    }
}
