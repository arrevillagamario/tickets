using Microsoft.AspNetCore.Mvc;

namespace tickets.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
