using System.ComponentModel.DataAnnotations;

namespace tickets.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = null!;

        [Display(Name = "Recuérdame")]
        public bool Recuerdame { get; set; }
    }
}
