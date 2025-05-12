using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;

namespace QLTV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.TbBooks.Include(t => t.Category);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBook = await _context.TbBooks
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (tbBook == null)
            {
                return NotFound();
            }

            return View(tbBook);
        }

        // GET: Admin/Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Author,Description,Image,Quantity,CategoryId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] TbBook tbBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbBook.CategoryId);
            return View(tbBook);
        }

        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBook = await _context.TbBooks.FindAsync(id);
            if (tbBook == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbBook.CategoryId);
            return View(tbBook);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,Description,Image,Quantity,CategoryId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] TbBook tbBook)
        {
            if (id != tbBook.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBookExists(tbBook.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "CategoryId", "CategoryId", tbBook.CategoryId);
            return View(tbBook);
        }

        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBook = await _context.TbBooks
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (tbBook == null)
            {
                return NotFound();
            }

            return View(tbBook);
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBook = await _context.TbBooks.FindAsync(id);
            if (tbBook != null)
            {
                _context.TbBooks.Remove(tbBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbBookExists(int id)
        {
            return _context.TbBooks.Any(e => e.BookId == id);
        }
    }
}
