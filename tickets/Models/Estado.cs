using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Estado1 { get; set; } = null!;

    public virtual ICollection<RegistroActualizacione> RegistroActualizaciones { get; set; } = new List<RegistroActualizacione>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
