using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace tickets.Servicios
{
    public interface IAutenticacionUsuarios
    {
        int GetClienteId();
    }
    public class AutenticacionUsuarios : IAutenticacionUsuarios
    {
        private readonly HttpContext _contextAccessor;

        public AutenticacionUsuarios(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor.HttpContext;
        }

        public int GetClienteId()
        {
            if (_contextAccessor.User.Identity.IsAuthenticated)
            {
                var idClaim = _contextAccessor.User
                        .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no está autenticado");
            }
        }
    }
}
