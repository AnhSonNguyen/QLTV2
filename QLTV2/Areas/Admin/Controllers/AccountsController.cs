using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using QLTV2.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // Đăng nhập
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.TbAccounts
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.RoleId == 1);

                if (user != null)
                {
                    HttpContext.Session.SetString("AdminUsername", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            return View(model);
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUsername");
            return RedirectToAction("Login");
        }

        // Danh sách tài khoản
        public async Task<IActionResult> Index()
        {
            var accounts = _context.TbAccounts.Include(a => a.Role);
            return View(await accounts.ToListAsync());
        }

        // Chi tiết tài khoản
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var account = _context.TbAccounts
                .Include(a => a.Role)
                .FirstOrDefault(m => m.AccountId == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // Thêm tài khoản (GET)
        public IActionResult Create()
        {
            ViewBag.RoleId = new SelectList(_context.TbRoles, "RoleId", "RoleName");
            return View();
        }

        // Thêm tài khoản (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbAccount account)
        {
            if (ModelState.IsValid)
            {
                _context.TbAccounts.Add(account);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RoleId = new SelectList(_context.TbRoles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // Sửa tài khoản (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var account = _context.TbAccounts.Find(id);
            if (account == null) return NotFound();

            ViewBag.RoleId = new SelectList(_context.TbRoles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // Sửa tài khoản (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TbAccount account)
        {
            if (id != account.AccountId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TbAccounts.Any(e => e.AccountId == id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.RoleId = new SelectList(_context.TbRoles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // Xóa tài khoản (GET)
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var account = _context.TbAccounts
                .Include(a => a.Role)
                .FirstOrDefault(m => m.AccountId == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // Xóa tài khoản (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var account = _context.TbAccounts.Find(id);
            if (account != null)
            {
                _context.TbAccounts.Remove(account);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
