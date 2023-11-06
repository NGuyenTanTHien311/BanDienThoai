using FluentAssertions.Common;

namespace BanDienThoai
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // ...

            services.AddDistributedMemoryCache(); // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg => { // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "xuanthulab"; // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 30, 0); // Thời gian tồn tại của Session
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ...
            
            app.UseSession();

            // ...
        }

    }
}
