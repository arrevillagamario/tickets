namespace tickets.Models
{
    public class ProgresoViewModel
    {
        public Guid idTicket { get; set; }
        public IEnumerable<Usuario>? Usuarios { get; set; }

        public Comentario? Comentario { get; set; }
    }
}
