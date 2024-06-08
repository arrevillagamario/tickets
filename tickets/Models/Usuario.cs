using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Empresa { get; set; }

    public int IdRol { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Ticket> TicketIdUsuarioAsignadoNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketIdUsuarioSolicitanteNavigations { get; set; } = new List<Ticket>();
}
