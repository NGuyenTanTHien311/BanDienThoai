using BanDienThoai.Models;
using Microsoft.EntityFrameworkCore;

namespace BanDienThoai.Repository
{
    public class LoaiSpRepository : ILoaiSpRepository
    {
        private readonly QlbanVaLiContext _context;
        public LoaiSpRepository(QlbanVaLiContext context)
        {
            _context = context;
        }
        public TLoaiSp Add(TLoaiSp loaiSp)
        {
            _context.TLoaiSps.Add(loaiSp); _context.SaveChanges(); return loaiSp;
        }

        public TLoaiSp Delete(string maloaiSp)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TLoaiSp> GetAllloaiSp()
        {
            return _context.TLoaiSps;
        }

        public object GetAllLoaiSp()
        {
            throw new NotImplementedException();
        }

        public TLoaiSp GetLoaiSp(string maloaiSp)
        {
            return _context.TLoaiSps.Find(maloaiSp);
        }

        public TLoaiSp Update(TLoaiSp loaiSp)
        {
            _context.Update(loaiSp); 
            _context.SaveChanges(); 
            return loaiSp;
        }
    }
}
