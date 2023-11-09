
using Microsoft.EntityFrameworkCore;

namespace BanDienThoai.Models
{
    public class Order : DbContext
    {
        internal string MaKhachHang;

        
         public string customerName { get; set; }

       
        public string shippingAddress { get; set; }

        public string phoneNumber { get; set; }

        // Các thuộc tính liên quan đến đơn hàng khác
    
    // Thêm DbSet cho các đối tượng khác
}
}