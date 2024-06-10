using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using tickets.Models;

namespace tickets.Servicios
{
    public interface IRepositorioTickets
    {
        Task AsignarTicket(Guid idTicket, int idAsignado, string comentario);
        Task<Ticket> CrearTicket(CrearTicketViewModels ticketNuevo);
        Task<DetalleViewModel> DetalleTicket(Guid id);
        Task<Guid> FinalizarTicket(Guid id);
        Task<IEnumerable<Ticket>> ListarTicketsAsignados();
        Task<IEnumerable<Ticket>> ListarTicketsCreados();
        Task<IEnumerable<Ticket>> ListarTicketsPausados();
        Task<IEnumerable<Ticket>> ListarTicketsResueltos();
        Task PausarTicket(Guid idTicket);
        Task<ProgresoViewModel> ProgresoTicket(Guid id);
        Task<IEnumerable<Usuario>> Tecnicos();
    }
    public class RepositorioTickets : IRepositorioTickets
    {
        private readonly TicketsContext _context;
        private readonly IAutenticacionUsuarios _autenticacion;
        private readonly IServicioEmail _servicioEmail;
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public RepositorioTickets(TicketsContext context, IAutenticacionUsuarios autenticacion, IServicioEmail servicioEmail, IRepositorioUsuarios repositorioUsuarios)
        {
            _context = context;
            _autenticacion = autenticacion;
            _servicioEmail = servicioEmail;
            this.repositorioUsuarios = repositorioUsuarios;
        }

        public async Task<Ticket> CrearTicket(CrearTicketViewModels ticketNuevo)
        {
            DateOnly hoy = DateOnly.FromDateTime(dateTime: DateTime.Now);

            var usuarioEnSession = _autenticacion.GetClienteId();


            var ticket = new Ticket()
            {
                Servicio = ticketNuevo.Servicio,
                DescripcionProblema = ticketNuevo.DescripcionProblema,
                Fecha = hoy,
                Prioridad = ticketNuevo.Prioridad,
                IdUsuarioSolicitante = usuarioEnSession,
                FechaFinalizacion = ticketNuevo.FechaFinalizacion,
                IdUsuarioAsignado = null,
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
            int cliente = _autenticacion.GetClienteId();

            int rol = await repositorioUsuarios.ObtenerRol(cliente);

            var tickets = new List<Ticket>();   

            if(rol == 1)
            {
                 tickets = await _context.Tickets.Where(x => x.Estado == 1 && x.IdUsuarioAsignado == null).OrderByDescending(x => x.Fecha)
                                .ToListAsync();
            }
            else
            {
                tickets = await _context.Tickets.Where( x => x.Estado == 1 && x.IdUsuarioAsignado == cliente || x.IdUsuarioSolicitante == cliente).ToListAsync();
            }

            return tickets;
        }


        public async Task<IEnumerable<Ticket>> ListarTicketsAsignados()
        {
            int cliente = _autenticacion.GetClienteId();

            int rol = await repositorioUsuarios.ObtenerRol(cliente);

            var tickets = new List<Ticket>();

            if (rol == 1)
            {
                tickets = await _context.Tickets.Include(x => x.IdUsuarioAsignadoNavigation).Where(x => x.Estado == 3).OrderByDescending(x => x.Fecha)
                               .ToListAsync();
            }
            else if(rol == 2)
            {
                tickets = await _context.Tickets.Include(x => x.IdUsuarioAsignadoNavigation).Where(x => x.Estado == 3 && x.IdUsuarioAsignado == cliente).OrderByDescending(x => x.Fecha)
                               .ToListAsync();
            }
            else
            {
                tickets = await _context.Tickets.Where(x => x.Estado == 3 && x.IdUsuarioSolicitante == cliente).OrderByDescending(x => x.Fecha)
                    .ToListAsync();
            }

            return tickets;
        }

        public async Task<IEnumerable<Ticket>> ListarHistorialTickets()
        {
            var Tickets = await _context.Tickets.OrderByDescending(x => x.Fecha)
                                .ToListAsync();

            return Tickets;
        }


        public async Task<DetalleViewModel> DetalleTicket( Guid id)
        {
            var ticket = await _context.Tickets
                                                .Where(x => x.IdTicket == id)
                                                .FirstAsync();

            var detalleTicket = await _context.Comentarios.Where(x => x.IdTicket == id && x.IdUsuarioD == ticket.IdUsuarioAsignado).OrderByDescending(x => x.IdComentario).FirstOrDefaultAsync();
            //.OrderByDescending(x => x.IdUsuarioD == usaurioSession)


            var usuarios = await Tecnicos();
            string comment;
            if (detalleTicket != null)
            {
                 comment = detalleTicket.Comentario1;
            }
            else
            {
                comment = "--No tiene actividad asignada--";

            }

            var descripcion = new DetalleViewModel()
            {
                idTicket = id,
                comentarios = comment,
                Descripcion = ticket.DescripcionProblema,
                usuarios = usuarios
            };

            return descripcion;
        }
        public async Task<string> CorreoUsuario()
        {
            int UsuarioEnSession = _autenticacion.GetClienteId();
            var usuario = await _context.Usuarios.Where(x => x.UsuarioId == UsuarioEnSession).FirstAsync();

            return usuario.Email;
        }

        public async Task<IEnumerable<Usuario>> Tecnicos()
        {
            var tecnicos = await _context.Usuarios.Where(x => x.IdRol == 2).ToListAsync();

            return tecnicos;
        }

        public async Task<ProgresoViewModel> ProgresoTicket(Guid id)
        {

            var usuarios = await Tecnicos();
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
            DateOnly hoy = DateOnly.FromDateTime(dateTime: DateTime.Now);
            var ticket = await _context.Tickets.Where(x => x.IdTicket == id).FirstOrDefaultAsync();

            ticket.Estado = 4;
            ticket.FechaFinalizacion = hoy;

            string email = await ObtenerEmailTicket(id); 

            _context.Update(ticket);
            await _context.SaveChangesAsync();
            await _servicioEmail.EnviarFinalizaciónTicket(ticket.IdTicket, email);

            return ticket.IdTicket;
        }

        public async Task<IEnumerable<Ticket>> ListarTicketsResueltos()
        {
            var cliente = _autenticacion.GetClienteId();

            var usuario = await _context.Usuarios.Where(x => x.UsuarioId == cliente).FirstOrDefaultAsync();

            List<Ticket> result = new List<Ticket>();

            if(usuario.IdRol == 1)
            {
                 result = await _context.Tickets.Where(x => x.Estado == 4).ToListAsync();
            }
            else
            {
                 result = await _context.Tickets.Where(x => x.Estado == 4).Where(x => x.IdUsuarioAsignado == cliente || x.IdUsuarioSolicitante == cliente).ToListAsync();
            }

            return result;
        }

        public async Task<string> ObtenerEmailTicket(Guid id)
        {
            var ticket = await _context.Tickets.Where(x => x.IdTicket == id).FirstOrDefaultAsync();
            var idCliente = ticket.IdUsuarioSolicitante;

            var cliente = await _context.Usuarios.Where(x => x.UsuarioId ==idCliente).FirstOrDefaultAsync();

            string correo = cliente.Email;

            return correo;

        }

        public async Task<string> ObtenerMailAsignadoTicket(int id)
        {
            var tecnico = await _context.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefaultAsync();


            string correo = tecnico.Email;

            return correo;

        }



        public async Task AsignarTicket(Guid idTicket, int idAsignado, string comentario)
        {
            int asignante = _autenticacion.GetClienteId();
            var ticket = await _context.Tickets.Where(x => x.IdTicket == idTicket).FirstOrDefaultAsync();

            ticket.Estado = 3;
            ticket.IdUsuarioAsignado = idAsignado;

            var comentarioHecho = new Comentario()
            {
                IdTicket = idTicket,
                Comentario1 = comentario,
                IdUsuarioR = asignante,
                IdUsuarioD = idAsignado
            };

            _context.Update(ticket);
            _context.Add(comentarioHecho);
            await _context.SaveChangesAsync();

            string correo = await ObtenerMailAsignadoTicket(idAsignado);

            await _servicioEmail.EnviarAsignacionTicket(ticket.IdTicket, correo);
        }

        public async Task<IEnumerable<Ticket>> ListarTicketsPausados()
        {
            var cliente = _autenticacion.GetClienteId();

            var usuario = await _context.Usuarios.Where(x => x.UsuarioId == cliente).FirstOrDefaultAsync();

            List<Ticket> result = new List<Ticket>();

            if (usuario.IdRol == 1)
            {
                result = await _context.Tickets.Where(x => x.Estado == 2).ToListAsync();
            }
            else
            {
                result = await _context.Tickets.Where(x => x.Estado == 2).Where(x => x.IdUsuarioAsignado == cliente || x.IdUsuarioSolicitante == cliente).ToListAsync();
            }

            return result;
        }

        public async Task PausarTicket(Guid idTicket)
        {
            int asignante = _autenticacion.GetClienteId();
            var ticket = await _context.Tickets.Where(x => x.IdTicket == idTicket).FirstOrDefaultAsync();

            ticket.Estado = 2;

           

            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }


    }


    


}
