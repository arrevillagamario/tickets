using System.ComponentModel.DataAnnotations;

namespace tickets.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Es necesario ingresar un email válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Tiene que crear una {0}")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = null!;

        [Display(Name = "Recuérdame")]
        public bool Recuerdame { get; set; }
    }
}
