using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class Archivo
{
    public int IdArchivo { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string TipoArchivo { get; set; } = null!;

    public Guid IdTicket { get; set; }

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
