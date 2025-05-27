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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Fuxikarte.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(AppDbContext context, ILogger<UserController> logger, UserService userService, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users (Need auth)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                if (users == null || !users.Any()) return NotFound(new { message = "Nenhum usuário encontrado" });
                return Ok(users);
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

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null) return NotFound(new { message = $"Nenhum usuário encontrado com o ID: {id}" });
                return Ok(user);
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

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO model, int id)
        {
            try
            {
                var isUpdated = await _userService.UpdateUser(id, model);
                if (!isUpdated) return NotFound(new { message = $"Nenhum usuário encontrado com o ID: {id}" });
                return Ok(new { message = "Usuário atualizado com sucesso" });
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

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.DeleteUser(id);
                if (!user) return NotFound(new { message = $"Nenhum usuário encontrado com o ID: {id}" });
                return Ok(new { message = "Usuário excluído com sucesso" });
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