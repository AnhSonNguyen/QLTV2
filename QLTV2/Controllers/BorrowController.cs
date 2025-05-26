using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV2.Models;
using System;
using System.Threading.Tasks;

namespace QLTV2.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibraryDbContext _context;

        public BorrowController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int BookId, int Quantity, DateTime ReturnDate)
        {
            // Kiểm tra đăng nhập
            var accountId = HttpContext.Session.GetInt32("UserId");
            if (accountId == null)
                return RedirectToAction("Login", "Account");

            // Lấy thông tin sách
            var book = await _context.TbBooks.FindAsync(BookId);
            if (book == null || !book.IsActive || book.Quantity < Quantity)
            {
                TempData["Error"] = "Sách không tồn tại hoặc không đủ số lượng.";
                return RedirectToAction("Details", "Books", new { id = BookId });
            }

            // Tạo phiếu mượn
            var borrow = new TbBorrow
            {
                AccountId = accountId.Value,
                BorrowDate = DateTime.Now,
                ReturnDate = ReturnDate
            };
            _context.TbBorrows.Add(borrow);
            await _context.SaveChangesAsync();

            // Tạo chi tiết mượn
            var borrowDetail = new TbBorrowDetail
            {
                BorrowId = borrow.BorrowId,
                BookId = BookId,
                Quantity = Quantity
            };
            _context.TbBorrowDetails.Add(borrowDetail);

            // Cập nhật số lượng sách
            book.Quantity -= Quantity;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Mượn sách thành công!";
            return RedirectToAction("Details", "Books", new { id = BookId });
        }
    }
}
