using BanDienThoai.Models;

namespace BanDienThoai.Models
{
    public class CartItem
    {
        public int Quantity { get; set; } // Số lượng sản phẩm trong giỏ hàng
        public TDanhMucSp Product { get; set; } // Thông tin chi tiết về sản phẩm (tên, giá, hình ảnh, mô tả, v.v.)
    }
}
