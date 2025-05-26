using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Fuxikarte.Backend.Data;
using Fuxikarte.Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace Fuxikarte.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public AuthController(AppDbContext context, ILogger<AuthController> logger, UserService userService, AuthService authService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model)
        {
            try
            {
                await _userService.CreateUser(model);
                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "ERROR DB");
                return StatusCode(400, $"ERROR DB: {ex.Message}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                return StatusCode(500, $"ERROR: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Username)) return BadRequest(new { message = "Usuário não foi preenchido" });
                if (string.IsNullOrWhiteSpace(model.Password)) return BadRequest(new { message = "Senha não foi preenchida" });

                var user = await _authService.ValidateUserCredentials(model.Username, model.Password);
                if (user == null) return Unauthorized(new { message = "Credenciais inválidas" });
                var token = _authService.GenerateToken(user);
                return Ok(new { user.UserId, user.Username, token });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "ERROR DB");
                return StatusCode(400, $"ERROR DB: {ex.Message}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR");
                return StatusCode(500, $"ERROR: {ex.Message}");
            }
        }
    }
}