
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

        public async Task<IActionResult> TicketProgreso(Guid id)
        {
            var tecnicos =  await _repositorioTickets.ProgresoTicket(id);

            return PartialView("_ProgresoTicket", tecnicos);
        }

        public async Task<IActionResult> ResolverTicket(Guid id)
        {
            await _repositorioTickets.FinalizarTicket(id);

            return RedirectToAction("TicketsResueltos");
        }

        public ActionResult TicketsEnProgreso()
        {
            return View();
        }
        public ActionResult TicketsEnPausa()
        {
            return View();
        }
        public async Task<IActionResult> TicketsResueltos()
        {
            var tickets = await _repositorioTickets.ListarTicketsResueltos();
            return View(tickets);
        }



    }
}
