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
    public class SaleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SaleController> _logger;
        private readonly SaleService _saleService;

        public SaleController(AppDbContext context, ILogger<SaleController> logger, SaleService saleService)
        {
            _context = context;
            _logger = logger;
            _saleService = saleService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SaleDTO>> CreateSale([FromBody] NewSaleDTO model)
        {
            try
            {
                await _saleService.CreateSale(model);
                return Ok(new { message = "Venda criada com sucesso" });
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
        public async Task<ActionResult<IEnumerable<SaleNavDTO>>> GetAllSalesNav()
        {
            try
            {
                var sales = await _saleService.GetAllSalesNav();
                if (sales == null || !sales.Any()) return NotFound(new { message = "Nenhuma venda encontrada!" });
                return Ok(new { sales });
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
        public async Task<ActionResult<SaleDTO>> GetSaleByIdNav(int id)
        {
            try
            {
                var sale = await _saleService.GetSaleByIdNav(id);
                if (sale == null) return NotFound(new { message = $"Venda de ID {id} não encontrada!" });
                return Ok(new { sale });
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
        public async Task<IActionResult> UpdateSale(UpdateSaleDTO model, int id)
        {
            try
            {
                var isUpdated = await _saleService.UpdateSale(model, id);
                if (!isUpdated) return NotFound(new { message = $"Venda de ID {id} não encontrada!" });
                return Ok(new { message = $"Venda de ID {id} atualizada com sucesso!" });
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
        public async Task<IActionResult> DeleteSale(int id)
        {
            try
            {
                var isDeleted = await _saleService.DeleteSale(id);
                if (!isDeleted) return NotFound(new { message = $"Venda de ID {id} não encontrada!" });
                return Ok(new { message = $"Venda de ID {id} removida com sucesso!" });
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