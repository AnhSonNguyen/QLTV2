using Microsoft.AspNetCore.Mvc;

namespace QLTV2.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
