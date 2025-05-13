using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using QLTV2.Areas.Admin.Models;

namespace QLTV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly LibraryDbContext _context;

        public AccountsController(LibraryDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.TbAccounts
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.RoleId == 1); // 1 = Admin

                if (user != null)
                {
                    HttpContext.Session.SetString("AdminUsername", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUsername");
            return RedirectToAction("Login");
        }
    }
}
