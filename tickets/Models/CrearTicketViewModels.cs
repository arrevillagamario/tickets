using System.ComponentModel.DataAnnotations;

namespace tickets.Models
{
    public record CrearTicketViewModels
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Servicio { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? DescripcionProblema { get; set; } 

        public DateOnly Fecha { get; set; }

        public string? Prioridad { get; set; }

        public DateOnly FechaFinalizacion { get; set; }

    }
}
