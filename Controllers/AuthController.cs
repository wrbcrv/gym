using Api.Dtos;
using Api.DTOs;
using Api.Services;
using Api.Services.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest("Usuário e senha são obrigatórios.");
                }

                var user = await _userService.GetByUsernameAsync(dto.UserName, dto.Password);
                if (user == null)
                {
                    return Unauthorized("Usuário ou senha inválidos.");
                }

                var token = _jwtService.GenerateToken(user.UserName!, user.Id);

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(8)
                });

                return Ok(new { message = "Login realizado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logout realizado com sucesso." });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = UserUtils.GetCurrentUserId(HttpContext);

                var user = await _userService.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao buscar usuário: {ex.Message}" });
            }
        }
    }
}
