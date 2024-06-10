using System.ComponentModel.DataAnnotations;

namespace tickets.Models
{
    public class RegistrarUsuarioViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreUsuario { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = null!;
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; } = null!;

        public string? Empresa { get; set; }

        public int? IdRol { get; set; }
    }
}
