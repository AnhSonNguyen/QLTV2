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
    public class BorrowsController : Controller
    {
        private readonly LibraryDbContext _context;

        public BorrowsController(LibraryDbContext context)
        {
            _context = context;
        }

        // Danh sách phiếu mượn
        public async Task<IActionResult> Index()
        {
            var borrows = await _context.TbBorrows
                .Include(b => b.Account)
                .Include(b => b.TbBorrowDetails)
                    .ThenInclude(d => d.Book)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return View(borrows);
        }

        // Cập nhật trạng thái
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var borrow = await _context.TbBorrows.FindAsync(id);
            if (borrow == null) return NotFound();

            borrow.Status = status;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật trạng thái thành công.";
            return RedirectToAction("Index");
        }
    }
}
