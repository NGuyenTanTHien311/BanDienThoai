using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanDienThoai.Models;

public partial class TUser
{
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = " Mật khẩu là bắt buộc.")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null;

    public byte? LoaiUser { get; set; }


    public virtual ICollection<TKhachHang> TKhachHangs { get; set; } = new List<TKhachHang>();

    public virtual ICollection<TNhanVien> TNhanViens { get; set; } = new List<TNhanVien>();
}
