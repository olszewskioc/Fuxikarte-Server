using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class SaleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;
        private readonly LocalService _localService;

        public SaleService(AppDbContext context, IMapper mapper, CustomerService customerService, LocalService localService)
        {
            _context = context;
            _mapper = mapper;
            _customerService = customerService;
            _localService = localService;
        }

        public async Task CreateSale(NewSaleDTO model)
        {
            _ = await _customerService.GetCustomerById(model.CustomerId) ?? throw new Exception("Cliente não existe!");
            _ = await _localService.GetLocalById(model.LocalId) ?? throw new Exception("Local não existe!");

            Sale sale = _mapper.Map<Sale>(model);
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<SaleDTO>> GetAllSales()
        {
            var sales = await _context.Sales.ToListAsync();
            return _mapper.Map<IEnumerable<SaleDTO>>(sales);
        }
        public async Task<SaleDTO> GetSaleById(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            return _mapper.Map<SaleDTO>(sale);
        }
        public async Task<IEnumerable<SaleNavDTO>> GetAllSalesNav()
        {
            var sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Local)
                .Include(s => s.SaleProducts)
                    .ThenInclude(sp => sp.Product)
                .ToListAsync();
            return _mapper.Map<IEnumerable<SaleNavDTO>>(sales);
        }
        public async Task<SaleNavDTO> GetSaleByIdNav(int id)
        {
            var sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Local)
                .Include(s => s.SaleProducts)
                    .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.SaleId == id);
            return _mapper.Map<SaleNavDTO>(sales);
        }
        public async Task<bool> UpdateSale(UpdateSaleDTO model, int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null) return false;

            if (model.CustomerId != null)
            {
                _ = _customerService.GetCustomerById((int)model.CustomerId) ?? throw new Exception($"Cliente {model.CustomerId} não existe!");
            }
            if (model.LocalId != null)
            {
                _ = _localService.GetLocalById((int)model.LocalId) ?? throw new Exception($"Local {model.LocalId} não existe!");
            }

            sale.LocalId = model.LocalId ?? sale.LocalId;
            sale.CustomerId = model.CustomerId ?? sale.CustomerId;
            sale.Payment = model.Payment ?? sale.Payment;
            sale.Subtotal = model.Subtotal ?? sale.Subtotal;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null) return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}