using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tickets.Controllers
{
    public class TicketsController : Controller
    {
        // GET: TicketsController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrearTicket()
        {
            return View();
        }

        public ActionResult TicketsCreados()
        {
            return View();
        }
        public ActionResult TicketsEnProgreso()
        {
            return View();
        }
        public ActionResult TicketsEnPausa()
        {
            return View();
        }
        public ActionResult TicketsResueltos()
        {
            return View();
        }



    }
}
