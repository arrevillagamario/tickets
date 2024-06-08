using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class Comentario
{
    public int IdComentario { get; set; }

    public string Comentario1 { get; set; } = null!;

    public int IdUsuarioR { get; set; }

    public int IdUsuarioD { get; set; }

    public Guid IdTicket { get; set; }

    public virtual Usuario IdUsuarioRNavigation { get; set; } = null!;
}
