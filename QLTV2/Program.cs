using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;

var builder = WebApplication.CreateBuilder(args);

// C?u hình DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Kích ho?t Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// C?u hình Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Account/Login";   // ???ng d?n ??n trang ??ng nh?p
        options.AccessDeniedPath = "/Admin/Account/AccessDenied"; // n?u b? ch?n truy c?p
    });

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // c?n dòng này ?? x? lý file t?nh nh? css/js/img

app.UseRouting();

app.UseAuthentication(); // c?n ??t tr??c Authorization
app.UseAuthorization();

app.UseSession(); // dùng sau UseRouting

// C?u hình route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// C?u hình route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
