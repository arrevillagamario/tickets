namespace tickets.Models
{
    public class DetalleViewModel
    {
        public Guid idTicket { get; set; }
        public string? comentarios { get; set; }    
        public string? Descripcion { get; set; }
        public IEnumerable<Usuario>? usuarios { get; set;}

    }
}
