using Microsoft.AspNetCore.Mvc;
using QLTV2.Models;
using System;
using System.Threading.Tasks;

namespace QLTV2.Controllers
{
    public class ContactController : Controller
    {
        private readonly LibraryDbContext _context;

        public ContactController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbContact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedDate = DateTime.Now;
                contact.IsRead = false;
                _context.TbContacts.Add(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Gửi liên hệ thành công!";
                return RedirectToAction("Create");
            }
            TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
            return View(contact);
        }
    }
}
