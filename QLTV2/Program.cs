using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;

var builder = WebApplication.CreateBuilder(args);

// C?u h�nh DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// K�ch ho?t Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// C?u h�nh Cookie Authentication
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
app.UseStaticFiles(); // c?n d�ng n�y ?? x? l� file t?nh nh? css/js/img

app.UseRouting();

app.UseAuthentication(); // c?n ??t tr??c Authorization
app.UseAuthorization();

app.UseSession(); // d�ng sau UseRouting

// C?u h�nh route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// C?u h�nh route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
