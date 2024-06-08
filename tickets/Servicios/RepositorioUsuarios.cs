using Microsoft.EntityFrameworkCore;
using tickets.Models;

namespace tickets.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario?> BuscarUsuarioPorEmail(string email);
        Task<int> CrearUsuario(Usuario usuario);
        Task<int> ObtenerRol(int id);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly TicketsContext context;

        public RepositorioUsuarios(TicketsContext context)
        {
            this.context = context;
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
             
            var usuarioNuevo = new Usuario()
            {
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Contrasena = usuario.Contrasena,
                Telefono = usuario.Telefono,
                Empresa = usuario.Empresa is null? null: usuario.Empresa,
                IdRol = usuario.IdRol,
   
            };

            context.Add(usuario);
            await context.SaveChangesAsync();

            return usuario.UsuarioId;
        }

        public async Task<Usuario?> BuscarUsuarioPorEmail(string email)
        {
            if(email == null)
            {
                throw new ArgumentException("Error al enviar el email");
            }

            var usuario = await context.Usuarios.Where(x  => x.Email == email).FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<int> ObtenerRol(int id)
        {
            Usuario usuario = await context.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefaultAsync();

            var rol = await context.Rols.Where(x=> x.IdRol == usuario.IdRol).FirstOrDefaultAsync();

            return rol.IdRol;

        }
    }
}
