using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using System.Linq;
using Microsoft.AspNetCore.Http; // Đảm bảo có dòng này

public class AccountController : Controller
{
    private readonly LibraryDbContext _context;

    public AccountController(LibraryDbContext context)
    {
        _context = context;
    }

    // GET: /Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var account = _context.TbAccounts
            .FirstOrDefault(a => a.Username == username && a.Password == password && a.RoleId == 2);

        if (account != null)
        {
            // Lưu thông tin vào session
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetInt32("UserId", account.AccountId);
            HttpContext.Session.SetString("FullName", account.FullName ?? "");

            // Gửi thông báo thành công
            TempData["Success"] = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Tài khoản không hợp lệ hoặc không có quyền đăng nhập.";
        return View();
    }

    // GET: /Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
