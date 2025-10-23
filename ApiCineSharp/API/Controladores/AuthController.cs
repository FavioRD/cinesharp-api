using ApiCineSharp.API.DTOs;
using ApiCineSharp.API.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Register(RegistrarCredencialesDTO dto)
        {
            var token = await _authService.Registrar(dto);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CredencialesLoginDTO dto)
        {
            var token = await _authService.IniciarSesion(dto);
            return Ok(new { token });
        }
    }
}
