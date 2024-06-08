using System;
using System.Collections.Generic;

namespace tickets.Models;

public partial class Ticket
{
    public Guid IdTicket { get; set; }

    public string Servicio { get; set; } = null!;

    public string DescripcionProblema { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public string Prioridad { get; set; } = null!;

    public DateOnly FechaFinalizacion { get; set; }

    public int IdUsuarioSolicitante { get; set; }

    public int? IdUsuarioAsignado { get; set; }

    public int Estado { get; set; }

    public virtual ICollection<Archivo> Archivos { get; set; } = new List<Archivo>();

    public virtual Estado EstadoNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAsignadoNavigation { get; set; }

    public virtual Usuario IdUsuarioSolicitanteNavigation { get; set; } = null!;

    public virtual ICollection<RegistroActualizacione> RegistroActualizaciones { get; set; } = new List<RegistroActualizacione>();
}
