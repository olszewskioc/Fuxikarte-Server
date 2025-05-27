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
    public class LocalController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LocalController> _logger;
        private readonly LocalService _localService;

        public LocalController(AppDbContext context, ILogger<LocalController> logger, LocalService service)
        {
            _context = context;
            _logger = logger;
            _localService = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LocalDTO>> CreateLocal([FromBody] NewLocalDTO model)
        {
            try
            {
                await _localService.CreateLocal(model);
                return Ok(new { message = "Local criado com sucesso" });
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalNavDTO>>> GetAllLocalsNav()
        {
            try
            {
                var locals = await _localService.GetAllLocalsNav();
                if (locals == null || !locals.Any()) return NotFound(new { message = "Nenhum local encontrado!" });
                return Ok(new { locals });
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
        public async Task<ActionResult<LocalDTO>> GetLocalByIdNav(int id)
        {
            try
            {
                var local = await _localService.GetLocalByIdNav(id);
                if (local == null) return NotFound(new { message = $"Local de ID {id} não encontrado!" });
                return Ok(new { local });
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
        public async Task<IActionResult> UpdateLocal(UpdateLocalDTO model, int id)
        {
            try
            {
                var isUpdated = await _localService.UpdateLocal(model, id);
                if (!isUpdated) return NotFound(new { message = $"Local de ID {id} não encontrado!" });
                return Ok(new { message = $"Local de ID {id} atualizado com sucesso!" });
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
        public async Task<IActionResult> DeleteLocal(int id)
        {
            try
            {
                var isDeleted = await _localService.DeleteLocal(id);
                if (!isDeleted) return NotFound(new { message = $"Local de ID {id} não encontrado!" });
                return Ok(new { message = $"Local de ID {id} removido com sucesso!" });
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