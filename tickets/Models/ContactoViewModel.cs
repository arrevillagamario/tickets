using System.ComponentModel.DataAnnotations;

namespace tickets.Models
{
    public class ContactoViewModel
    {
        public Guid idTicket { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? comentario { get; set; }
    }
}
