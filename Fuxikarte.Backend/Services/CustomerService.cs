using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CustomerService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCustomer(NewCustomerDTO model)
        {
            Customer customer = _mapper.Map<Customer>(model);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<IEnumerable<CustomerNavDTO>> GetAllCustomersNav()
        {
            var customers = await _context.Customers
                .Include(c => c.Sales)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CustomerNavDTO>>(customers);
        }

        public async Task<CustomerNavDTO> GetCustomerByIdNav(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Sales)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            return _mapper.Map<CustomerNavDTO>(customer);
        }
        
        public async Task<bool> UpdateCustomer(UpdateCustomerDTO model, int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            customer.CustomerName = model.CustomerName ?? customer.CustomerName;
            customer.Phone = model.Phone ?? customer.Phone;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}