using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tickets.Models;

namespace tickets.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signIn;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signIn)
        {
            _userManager = userManager;
            _signIn = signIn;
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


    }
}
