#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto_WEB.Areas.Identity.Pages.Account
{
    // Esta clase reemplaza la página de Login que trae la librería de Identity.
    // Aquí vive la lógica (validar usuario y contraseña); la vista Login.cshtml solo dibuja el formulario.
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Datos que escribe el usuario en el formulario.
        [BindProperty]
        public InputModel Input { get; set; }

        // Página a la que se vuelve después de iniciar sesión (la que intentó abrir sin sesión).
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ingrese su correo electrónico.")]
            [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Ingrese su contraseña.")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Recordarme")]
            public bool RememberMe { get; set; }
        }

        // GET: muestra el formulario vacío.
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Borra cualquier cookie externa para empezar el inicio de sesión desde cero.
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        // POST: se ejecuta al presionar el botón "Iniciar sesión".
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Identity compara la contraseña escrita con el hash guardado en la base de datos.
                // lockoutOnFailure: true bloquea la cuenta unos minutos tras varios intentos fallidos.
                var resultado = await _signInManager.PasswordSignInAsync(
                    Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation("El usuario inició sesión.");
                    return LocalRedirect(returnUrl);
                }

                if (resultado.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "La cuenta está bloqueada temporalmente por demasiados intentos. Intente más tarde.");
                    return Page();
                }

                // Mensaje genérico: no revela si el correo existe o si falló la contraseña.
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return Page();
            }

            // Si algo no es válido, vuelve a mostrar el formulario con los errores.
            return Page();
        }
    }
}
