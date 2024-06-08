using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class RegistroActualizacione
{
    public int IdRegistro { get; set; }

    public DateOnly Fecha { get; set; }

    public Guid IdTicket { get; set; }

    public int IdEstado { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
