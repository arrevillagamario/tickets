using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tickets.Models;
using tickets.Servicios;

namespace tickets.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signIn;
        private readonly IAutenticacionUsuarios autenticacion;
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signIn, IAutenticacionUsuarios autenticacion, IRepositorioUsuarios repositorioUsuarios)
        {
            _userManager = userManager;
            _signIn = signIn;
            this.autenticacion = autenticacion;
            this.repositorioUsuarios = repositorioUsuarios;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Interno()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Interno(RegistrarUsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = new  Usuario()
            {
                 NombreUsuario = viewModel.NombreUsuario,
                 Email = viewModel.Email,
                 Telefono = viewModel.Telefono,
                 IdRol = viewModel.IdRol,
            };

            var resultado = await _userManager.CreateAsync(usuario, password: viewModel.Contrasena);

            if (resultado.Succeeded)
            {

                await _signIn.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "TicketsCreados");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }

        public ActionResult Externo()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var resultado = await _signIn.PasswordSignInAsync(viewModel.Email,
                viewModel.Contrasena, viewModel.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                int idUsuario = autenticacion.GetClienteId();
                
                int rol = await repositorioUsuarios.ObtenerRol(idUsuario);

                TempData["rol"] = rol;

                return RedirectToAction("TicketsCreados", "Tickets");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login");
        }

    }
}
