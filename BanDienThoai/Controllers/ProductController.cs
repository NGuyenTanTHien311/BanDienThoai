using BanDienThoai.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ProductController : Controller
{
    // Key lưu chuỗi json của Cart
    public const string CARTKEY = "cart";

    

    // Lấy cart từ Session (danh sách CartItem)
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
    // Xóa cart khỏi Session
    void ClearCart()
    {
        var session = HttpContext.Session;
        session.Remove(CARTKEY);
    }
    // Lưu Cart (Danh sách CartItem) vào Session
    void SaveCartSession(List<CartItem> cartItems)
    {
        var session = HttpContext.Session;
        string jsonCart = JsonConvert.SerializeObject(cartItems);
        session.SetString(CARTKEY, jsonCart);
    }

}
