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
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryService _categoryService;

        public CategoryController(AppDbContext context, ILogger<CategoryController> logger, CategoryService service)
        {
            _context = context;
            _logger = logger;
            _categoryService = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] NewCategoryDTO model)
        {
            try
            {
                await _categoryService.CreateCategory(model);
                return Ok(new { message = "Categoria criada com sucesso" });
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
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CategoryNavDTO>>> GetAllCategoriesNav()
        {
            try
            {
                var categoies = await _categoryService.GetAllCategoriesNav();
                if (categoies == null || !categoies.Any()) return NotFound(new { message = "Nenhuma categoria encontrada!" });
                return Ok(new { categoies });
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
        public async Task<ActionResult<CategoryDTO>> GetCategoryByIdNav(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdNav(id);
                if (category == null) return NotFound(new { message = $"Categria de ID {id} não encontrada!" });
                return Ok(new { category });
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
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO model, int id)
        {
            try
            {
                var isUpdated = await _categoryService.UpdateCategory(model, id);
                if (!isUpdated) return NotFound(new { message = $"Categoria de ID {id} não encontrada!" });
                return Ok(new { message = $"Categoria de ID {id} atualizada com sucesso!" });
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var isDeleted = await _categoryService.DeleteCategory(id);
                if (!isDeleted) return NotFound(new { message = $"Categoria de ID {id} não encontrada!" });
                return Ok(new { message = $"Categoria de ID {id} removida com sucesso!" });
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