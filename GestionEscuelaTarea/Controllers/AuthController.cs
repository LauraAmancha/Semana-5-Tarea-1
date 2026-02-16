using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
// IMPORTANTE: Este using debe coincidir con la ubicación de tu clase personalizada
using GestionEscuelaTarea.Areas.Identity.Data;

namespace GestionEscuelaTarea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Cambiamos IdentityUser por GestionEscuelaTareaUser
        private readonly UserManager<GestionEscuelaTareaUser> _userManager;

        public AuthController(UserManager<GestionEscuelaTareaUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Buscamos por EMAIL específicamente (campo en aspnetusers)
            // Usamos .Trim() para limpiar espacios accidentales
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user != null)
            {
                // 2. Verificamos el PasswordHash usando las herramientas de Identity
                var result = await _userManager.CheckPasswordAsync(user, request.Password);

                if (result)
                {
                    return Ok(new
                    {
                        Authenticated = true,
                        User = user.UserName, // El nombre que se mostrará en el saludo
                        Token = "token-real-uniandes-2026",
                        ExpiresIn = 20
                    });
                }
            }

            // 3. Respuesta de error si no coinciden los datos
            return Unauthorized(new { Message = "El correo o la contraseña no coinciden en nuestra base de datos." });
        }
    }

    // Modelo de datos para recibir la petición de Angular
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}