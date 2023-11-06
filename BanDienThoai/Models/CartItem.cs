using BanDienThoai.Models;

namespace BanDienThoai.Models
{
    public class CartItem
    {
        private TDanhMucSp product;

        public int Quantity { get; set; } // Số lượng sản phẩm trong giỏ hàng
        public TDanhMucSp Product { get => product; set => product = value; } // Thông tin chi tiết về sản phẩm (tên, giá, mô tả, v.v.)
        public List<TDanhMucSp> AnhDaiDien { get; set; } // Danh sách ảnh sản phẩm
    }
}
