namespace tickets.Models
{
    public class DetalleViewModel
    {
        public Comentario? comentarios { get; set; }    
        public string? Descripcion { get; set; }
        public IEnumerable<Usuario>? usuarios { get; set;}

    }
}
