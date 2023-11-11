using BanDienThoai.Models;

namespace BanDienThoai.ViewModels
{
    public class CheckoutViewModel
    {
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<TUser> Username { get; set; }
        public List<THoaDonBan> MaHoaDon { get; set; }

    }
}