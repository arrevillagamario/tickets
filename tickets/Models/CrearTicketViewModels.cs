namespace tickets.Models
{
    public record CrearTicketViewModels
    {
        public string? Servicio { get; set; } 

        public string? DescripcionProblema { get; set; } 

        public DateOnly Fecha { get; set; }

        public string? Prioridad { get; set; }

        public DateOnly FechaFinalizacion { get; set; }

    }
}
