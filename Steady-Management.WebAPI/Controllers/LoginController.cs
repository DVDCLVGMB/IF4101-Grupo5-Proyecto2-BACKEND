using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.WebAPI.DTOs;
using Steady_Management.WebAPI.Services;

namespace Steady_Management.WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthBusiness _auth;
        private readonly TokenService _tokenSvc;

        public LoginController(IConfiguration cfg)
        {
            string cs = cfg.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("ConnString missing");
            _auth = new AuthBusiness(cs);
            _tokenSvc = new TokenService(cfg);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequestDTO req)
        {
            if (req is null || string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username y Password requeridos.");

            var user = _auth.ValidateCredentials(req.Username, req.Password);
            if (user is null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var token = _tokenSvc.BuildToken(user);

            var resp = new LoginResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                Username = user.UserLogin,
                RoleId = user.RoleId
            };

            return Ok(resp);
        }
    }
}
