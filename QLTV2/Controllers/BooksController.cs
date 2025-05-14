using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;

namespace QLTV2.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.TbBooks
                .Where(b => b.IsActive)
                .Include(b => b.Category)
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.TbBooks
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id && b.IsActive);

            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
