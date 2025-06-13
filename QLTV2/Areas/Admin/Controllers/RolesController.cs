using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;
using System.Threading.Tasks;
using System.Linq;

namespace QLTV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly LibraryDbContext _context;

        public RolesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _context.TbRoles.ToListAsync();
            return View(roles);
        }

        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbRole role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm quyền thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var role = await _context.TbRoles.FindAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbRole role)
        {
            if (id != role.RoleId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật quyền thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var role = await _context.TbRoles.FindAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.TbRoles.FindAsync(id);
            if (role != null && role.RoleId > 2) // Không cho xóa admin/user mặc định
            {
                _context.TbRoles.Remove(role);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa quyền thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể xóa quyền mặc định!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
