using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;

namespace QLTV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly LibraryDbContext _context;

        public CommentsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Comments
        public async Task<IActionResult> Index()
        {
            var comments = await _context.TbBookReviews
                .Include(c => c.Account)
                .Include(c => c.Book)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View(comments);
        }

        // GET: Admin/Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _context.TbBookReviews
                .Include(c => c.Account)
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.BookReviewId == id);

            if (comment == null)
                return NotFound();

            return View(comment);
        }

        // POST: Admin/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.TbBookReviews.FindAsync(id);
            if (comment != null)
            {
                _context.TbBookReviews.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
