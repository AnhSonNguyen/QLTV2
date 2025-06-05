using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace QLTV2.Controllers
{
    public class BookReviewController : Controller
    {
        private readonly LibraryDbContext _context;

        public BookReviewController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TbBookReview review)
        {
            if (string.IsNullOrWhiteSpace(review.Comment))
            {
                TempData["Error"] = "Nội dung bình luận không được để trống.";
                return RedirectToAction("Details", "Books", new { id = review.BookId });
            }

            // Lấy người dùng đang đăng nhập từ session
            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (int.TryParse(accountIdStr, out int accountId))
            {
                review.AccountId = accountId;
            }

            review.CreatedDate = DateTime.Now;

            _context.TbBookReviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Books", new { id = review.BookId });
        }
    }
}
