
using Microsoft.AspNetCore.Mvc;
using tickets.Models;
using tickets.Servicios;

namespace tickets.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IRepositorioTickets _repositorioTickets;
        private readonly IAutenticacionUsuarios _autenticacion;
        private readonly IRepositorioUsuarios _repositorioUsuarios;

        public TicketsController(IRepositorioTickets repositorioTickets, IAutenticacionUsuarios autenticacion, IRepositorioUsuarios repositorioUsuarios)
        {
            _repositorioTickets = repositorioTickets;
            _autenticacion = autenticacion;
            _repositorioUsuarios = repositorioUsuarios;
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
            //int usuarioEnSession = autenticacion.GetClienteId();
            //int rolEnSession =  await repositorioUsuarios.ObtenerRol(usuarioEnSession);

            //IEnumerable<Ticket> ticketsCreados = new List<Ticket>();

            //if(rolEnSession == 1)
            //{
              var  ticketsCreados = await _repositorioTickets.ListarTicketsCreados();
            //}


            
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

        public async Task<IActionResult> TicketsEnProgreso()
        {
            var tickets = await _repositorioTickets.ListarTicketsAsignados();

            return View(tickets);
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

        [HttpPost]
        public async Task<IActionResult> AsignarTicket(Guid id, int usuarioSeleccionado, string comment)
        {
            

            await _repositorioTickets.AsignarTicket(id, usuarioSeleccionado, comment);

            return  this.RedirectToAction("TicketsEnProgreso");
        }



    }
}
