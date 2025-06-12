using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Controller xử lý các chức năng tài khoản cho người dùng thông thường (ngoài khu vực Admin).
/// </summary>
public class AccountController : Controller
{
    private readonly LibraryDbContext _context;

    /// <summary>
    /// Khởi tạo controller với DbContext.
    /// </summary>
    /// <param name="context">DbContext của thư viện.</param>
    public AccountController(LibraryDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Hiển thị trang đăng nhập cho người dùng.
    /// </summary>
    /// <returns>View đăng nhập.</returns>
    public IActionResult Login()
    {
        return View();
    }

    /// Xử lý đăng nhập cho người dùng với vai trò RoleId = 2.
    /// <param name="username">Tên đăng nhập.</param>
    /// <param name="password">Mật khẩu.</param>
    /// <returns>Chuyển hướng về trang chủ nếu thành công, ngược lại trả về view với thông báo lỗi.</returns>
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

    /// Đăng xuất người dùng, xóa toàn bộ session.
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    // Thêm vào trong class AccountController

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string Username, string Password, string FullName, string Phone, string Email)
    {
        // Kiểm tra trùng tên đăng nhập
        if (_context.TbAccounts.Any(a => a.Username == Username))
        {
            ViewBag.Error = "Tên đăng nhập đã tồn tại.";
            return View();
        }

        // Kiểm tra trùng email (nếu muốn)
        if (!string.IsNullOrEmpty(Email) && _context.TbAccounts.Any(a => a.Email == Email))
        {
            ViewBag.Error = "Email đã được sử dụng.";
            return View();
        }

        // Tạo tài khoản mới với RoleId mặc định là 2
        var account = new TbAccount
        {
            Username = Username,
            Password = Password, // Nên mã hóa mật khẩu trong thực tế!
            FullName = FullName,
            Phone = Phone,
            Email = Email,
            RoleId = 2
        };

        _context.TbAccounts.Add(account);
        _context.SaveChanges();
        // Thông báo đăng ký thành công
        TempData["Success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
        // Đăng ký thành công, chuyển về trang đăng nhập
        return RedirectToAction("Login", "Account");
    }
}
