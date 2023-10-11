using BanDienThoai.Models;
namespace BanDienThoai.Repository
{
    public interface ILoaiSpRepository
    {
        TLoaiSp Add(TLoaiSp loaiSp);
        TLoaiSp Update(TLoaiSp loaiSp);
        TLoaiSp Delete(String maloaiSp); 
        TLoaiSp GetLoaiSp(String maloaiSp);
        IEnumerable<TLoaiSp> GetAllloaiSp();
        object GetAllLoaiSp();
    }
}
