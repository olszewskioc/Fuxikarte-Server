using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public ProductService(AppDbContext context, IMapper mapper, CategoryService categoryService)
        {
            _context = context;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public async Task CreateProduct(NewProductDTO model)
        {
            var _ = await _categoryService.GetCategoryById(model.CategoryId) ?? throw new Exception("Categoria não existe!");
            Product product = _mapper.Map<Product>(model);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }
        public async Task<IEnumerable<ProductNavDTO>> GetAllProductsNav()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SaleProducts)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProductNavDTO>>(products);
        }
        public async Task<ProductNavDTO> GetProductByIdNav(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SaleProducts)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            return _mapper.Map<ProductNavDTO>(product);
        }
        public async Task<bool> UpdateProduct(UpdateProductDTO model, int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            var _ = await _context.Categories.FindAsync(model.CategoryId) ?? throw new Exception("Categoria inexistente");
            
            product.ProductName = model.ProductName ?? product.ProductName;
            product.Description = model.Description ?? product.Description;
            product.CategoryId = model.CategoryId ?? product.CategoryId;
            product.Stock = model.Stock ?? product.Stock;
            product.Cost = model.Cost ?? product.Cost;
            product.Price = model.Price ?? product.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}