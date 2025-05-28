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
    public class SaleProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SaleProductController> _logger;
        private readonly SaleProductService _saleProductService;

        public SaleProductController(AppDbContext context, ILogger<SaleProductController> logger, SaleProductService saleProductService)
        {
            _context = context;
            _logger = logger;
            _saleProductService = saleProductService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SaleProductDTO>> CreateSaleProduct([FromBody] NewSaleProductDTO model)
        {
            try
            {
                await _saleProductService.CreateSaleProduct(model);
                return Ok(new { message = $"Produto {model.ProductId} adiconado a venda {model.SaleId} com sucesso" });
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
        [HttpGet("sale/{saleId:int}")]
        public async Task<ActionResult<IEnumerable<ProductsInSaleDTO>>> GetAllProductsInSale(int saleId)
        {
            try
            {
                var saleProducts = await _saleProductService.GetAllProductsInSale(saleId);
                if (saleProducts == null || !saleProducts.Any()) return NotFound(new { message = $"Nenhum produto encontrado na venda {saleId}!" });
                return Ok(new { saleProducts });
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
        [HttpGet("product/{productId:int}")]
        public async Task<ActionResult<IEnumerable<SalesForProductDTO>>> GetAllSalesForProduct(int productId)
        {
            try
            {
                var saleProducts = await _saleProductService.GetAllSalesForProduct(productId);
                if (saleProducts == null || !saleProducts.Any()) return NotFound(new { message = $"Nenhuma venda encontrada para o produto {productId}!" });
                return Ok(new { saleProducts });
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
        public async Task<ActionResult<IEnumerable<SaleProductDTO>>> GetAllSaleProducts()
        {
            try
            {
                var saleProducts = await _saleProductService.GetAllSaleProducts();
                if (saleProducts == null || !saleProducts.Any()) return NotFound(new { message = "Nenhuma venda com produto encontrado!" });
                return Ok(new { saleProducts });
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
        public async Task<ActionResult<SaleProductDTO>> GetSaleProductById(int id)
        {
            try
            {
                var product = await _saleProductService.GetSaleProductById(id);
                if (product == null) return NotFound(new { message = $"Venda com produto de ID {id} não encontrado!" });
                return Ok(new { product });
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
        public async Task<IActionResult> UpdateSaleProduct(UpdateSaleProductDTO model, int id)
        {
            try
            {
                var isUpdated = await _saleProductService.UpdateSaleProduct(model, id);
                if (!isUpdated) return NotFound(new { message = $"Venda com produto de ID {id} não encontrado!" });
                return Ok(new { message = $"Venda com produto de ID {id} atualizado com sucesso!" });
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
        public async Task<IActionResult> DeleteSaleProduct(int id)
        {
            try
            {
                var isDeleted = await _saleProductService.DeleteSaleProduct(id);
                if (!isDeleted) return NotFound(new { message = $"Venda com produto de ID {id} não encontrado!" });
                return Ok(new { message = $"Venda com produto de ID {id} removido com sucesso!" });
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