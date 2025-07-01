using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.WebAPI.DTOs;
using Microsoft.Extensions.Configuration;

namespace Steady_Management.WebAPI.Controllers
{
    /// <summary>
    /// End‑point para inicio de sesión.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthBusiness _auth;

        // Inyección de IConfiguration para leer la cadena de conexión
        public LoginController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException(
                                   "No se encontró ConnectionString 'DefaultConnection'.");
            _auth = new AuthBusiness(connStr);
        }

        /// <summary>
        /// POST: api/Login
        /// Body: { "username": "...", "password": "..." }
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            // 1️⃣  Validación básica del payload
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username y Password son requeridos.");
            }

            // 2️⃣  Autenticación
            var user = _auth.ValidateCredentials(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            // 3️⃣  Éxito → retornar la información necesaria.
            //      Aquí solo devolvemos lo mínimo; si luego generas JWT, hazlo aquí.
            var response = new
            {
                userId = user.UserId,
                username = user.UserLogin,
                roleId = user.RoleId
            };

            return Ok(response);
        }
    }
}
