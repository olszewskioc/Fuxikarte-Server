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
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;

        public ProductController(AppDbContext context, ILogger<ProductController> logger, ProductService productService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] NewProductDTO model)
        {
            try
            {
                await _productService.CreateProduct(model);
                return Ok(new { message = "Produto criado com sucesso" });
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
        public async Task<ActionResult<IEnumerable<ProductNavDTO>>> GetAllProductsNav()
        {
            try
            {
                var products = await _productService.GetAllProductsNav();
                if (products == null || !products.Any()) return NotFound(new { message = "Nenhum produto encontrado!" });
                return Ok(new { products });
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
        public async Task<ActionResult<ProductDTO>> GetProductByIdNav(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdNav(id);
                if (product == null) return NotFound(new { message = $"Produto de ID {id} não encontrado!" });
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
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model, int id)
        {
            try
            {
                var isUpdated = await _productService.UpdateProduct(model, id);
                if (!isUpdated) return NotFound(new { message = $"Produto de ID {id} não encontrado!" });
                return Ok(new { message = $"Produto de ID {id} atualizado com sucesso!" });
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var isDeleted = await _productService.DeleteProduct(id);
                if (!isDeleted) return NotFound(new { message = $"Produto de ID {id} não encontrado!" });
                return Ok(new { message = $"Produto de ID {id} removido com sucesso!" });
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