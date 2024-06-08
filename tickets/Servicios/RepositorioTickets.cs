using Microsoft.EntityFrameworkCore;
using tickets.Models;

namespace tickets.Servicios
{
    public interface IRepositorioTickets
    {
        Task<Ticket> CrearTicket(CrearTicketViewModels ticketNuevo);
        Task<DetalleViewModel> DetalleTicket(Guid id);
        Task<Guid> FinalizarTicket(Guid id);
        Task<IEnumerable<Ticket>> ListarTicketsCreados();
        Task<IEnumerable<Ticket>> ListarTicketsResueltos();
        Task<ProgresoViewModel> ProgresoTicket(Guid id);
        Task<IEnumerable<Usuario>> Tecnicos();
    }
    public class RepositorioTickets : IRepositorioTickets
    {
        private readonly TicketsContext _context;
        private readonly IAutenticacionUsuarios _autenticacion;
        private readonly IServicioEmail _servicioEmail;

        public RepositorioTickets(TicketsContext context, IAutenticacionUsuarios autenticacion, IServicioEmail servicioEmail)
        {
            _context = context;
            _autenticacion = autenticacion;
            _servicioEmail = servicioEmail;
        }

        public async Task<Ticket> CrearTicket(CrearTicketViewModels ticketNuevo)
        {
            DateOnly hoy = DateOnly.FromDateTime(dateTime: DateTime.Now);

            var ticket = new Ticket()
            {
                Servicio = ticketNuevo.Servicio,
                DescripcionProblema = ticketNuevo.DescripcionProblema,
                Fecha = hoy,
                Prioridad = ticketNuevo.Prioridad,
                IdUsuarioSolicitante = 1,
                FechaFinalizacion = ticketNuevo.FechaFinalizacion,
                IdUsuarioAsignado = 2,
                Estado = 1
            };

             _context.Add(ticket);
            await _context.SaveChangesAsync();

            var email = await CorreoUsuario();

            await _servicioEmail.EnviarConfirmacionTicket(ticket.IdTicket, email);

            return ticket;
        }


        public async Task<IEnumerable<Ticket>> ListarTicketsCreados()
        {
            var Tickets = await _context.Tickets.Where(x => x.Estado == 1).OrderByDescending(x => x.Fecha)
                                .ToListAsync();

            return Tickets;
        }

        public async Task<DetalleViewModel> DetalleTicket( Guid id)
        {
            var ticket = await _context.Tickets
                                                .Where(x => x.IdTicket == id)
                                                .FirstAsync();

            var detalleTicket = await _context.Comentarios.Where(x => x.IdTicket == id).OrderByDescending(x => x.IdComentario).FirstOrDefaultAsync();
            //.OrderByDescending(x => x.IdUsuarioD == usaurioSession)


            var usuarios = await _context.Usuarios.Where(x => x.IdRol == 2).ToListAsync();

            var descripcion = new DetalleViewModel()
            {
                idTicket = id,
                comentarios = detalleTicket,
                Descripcion = ticket.DescripcionProblema,
                usuarios = usuarios
            };

            return descripcion;
        }
        public async Task<string> CorreoUsuario()
        {
            var usuario = await _context.Usuarios.Where(x => x.UsuarioId == 8).FirstAsync();

            return usuario.Email;
        }

        public async Task<IEnumerable<Usuario>> Tecnicos()
        {
            var tecnicos = await _context.Usuarios.Where(x => x.IdRol == 2).ToListAsync();

            return tecnicos;
        }

        public async Task<ProgresoViewModel> ProgresoTicket(Guid id)
        {

            var usuarios = await _context.Usuarios.ToListAsync();
            var progreso = new ProgresoViewModel()
            {
                idTicket = id,
                Usuarios = usuarios,
                Comentario = null
            };

            return progreso; 
        }

        public async Task<Guid> FinalizarTicket(Guid id)
        {
            var ticket = await _context.Tickets.Where(x => x.IdTicket == id).FirstOrDefaultAsync();

            ticket.Estado = 4;

            return ticket.IdTicket;
        }

        public async Task<IEnumerable<Ticket>> ListarTicketsResueltos()
        {
            var cliente = _autenticacion.GetClienteId();

            var tickets = await _context.Tickets.Where(x => x.Estado == 4 && x.IdUsuarioSolicitante == cliente || x.IdUsuarioAsignado == cliente).ToListAsync();

            return tickets;
        }
    }

   
}
