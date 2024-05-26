using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tickets.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: UsuariosController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Interno()
        {
            return View();
        }
        public ActionResult Externo()
        {
            return View();
        }


    }
}
