namespace tickets.Models
{
    public class RegistrarUsuarioViewModel
    {
        public string NombreUsuario { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string? Empresa { get; set; }

        public int? IdRol { get; set; }
    }
}
