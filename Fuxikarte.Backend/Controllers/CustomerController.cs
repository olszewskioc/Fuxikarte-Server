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
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerService _customerService;

        public CustomerController(AppDbContext context, ILogger<CustomerController> logger, CustomerService service)
        {
            _context = context;
            _logger = logger;
            _customerService = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer([FromBody] NewCustomerDTO model)
        {
            try
            {
                await _customerService.CreateCustomer(model);
                return Ok(new { message = "Cliente criado com sucesso" });
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
        public async Task<ActionResult<IEnumerable<CustomerNavDTO>>> GetAllCustomersNav()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersNav();
                if (customers == null || !customers.Any()) return NotFound(new { message = "Nenhum cliente encontrado!" });
                return Ok(new { customers });
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
        public async Task<ActionResult<CustomerDTO>> GetCustomerByIdNav(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdNav(id);
                if (customer == null) return NotFound(new { message = $"Cliente de ID {id} não encontrado!" });
                return Ok(new { customer });
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
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO model, int id)
        {
            try
            {
                var isUpdated = await _customerService.UpdateCustomer(model, id);
                if (!isUpdated) return NotFound(new { message = $"Cliente de ID {id} não encontrado!" });
                return Ok(new { message = $"Cliente de ID {id} atualizado com sucesso!" });
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
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var isDeleted = await _customerService.DeleteCustomer(id);
                if (!isDeleted) return NotFound(new { message = $"Cliente de ID {id} não encontrado!" });
                return Ok(new { message = $"Cliente de ID {id} removido com sucesso!" });
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