namespace tickets.Models
{
    public class ProgresoViewModel
    {
        public IEnumerable<Usuario>? Usuarios { get; set; }

        public Comentario? Comentario { get; set; }
    }
}
