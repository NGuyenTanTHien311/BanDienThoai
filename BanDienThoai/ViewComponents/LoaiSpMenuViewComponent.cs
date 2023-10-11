using BanDienThoai.Models;
using BanDienThoai.Repository;
using Microsoft.AspNetCore.Mvc;
namespace BanDienThoai.ViewComponents
{
    public class LoaiSpMenuViewComponent: ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSpRepository;
        public LoaiSpMenuViewComponent(ILoaiSpRepository loaiSpRepository)
        {
            _loaiSpRepository = loaiSpRepository;
        }
        public IViewComponentResult Invoke()
        {
        var loaiSp = _loaiSpRepository.GetAllloaiSp().OrderBy(x => x.Loai);
            return View(loaiSp);    
        }
    }
}
