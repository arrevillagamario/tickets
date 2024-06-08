using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tickets.Models;
using tickets.Servicios;

namespace tickets.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IRepositorioTickets _repositorioTickets;

        public TicketsController(IRepositorioTickets repositorioTickets)
        {
            _repositorioTickets = repositorioTickets;
        }

        // GET: TicketsController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrearTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearTicket(CrearTicketViewModels nuevoTicket)
        {
            await _repositorioTickets.CrearTicket(nuevoTicket);
            return RedirectToAction("TicketsCreados");
        }

        public async Task<IActionResult> TicketsCreados()
        {
            var ticketsCreados = await _repositorioTickets.ListarTicketsCreados();
            return View(ticketsCreados);
        }

        public async Task<IActionResult> DetalleTicket(Guid id)
        {
            var detalle = await _repositorioTickets.DetalleTicket(id);

            return PartialView("_ComentariosTicket", detalle);
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
