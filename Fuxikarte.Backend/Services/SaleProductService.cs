using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class SaleProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ProductService _productService;
        private readonly SaleService _saleService;

        public SaleProductService(AppDbContext context, IMapper mapper, SaleService saleService, ProductService productService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
            _saleService = saleService;
        }

        public async Task CreateSaleProduct(NewSaleProductDTO model)
        {
            var sale = await _saleService.GetSaleById(model.SaleId) ?? throw new Exception($"Venda {model.SaleId} não existe");
            var product = await _productService.GetProductById(model.ProductId) ?? throw new Exception($"Produto {model.ProductId} não existe");

            var existingSaleProduct = await _context.SaleProducts
                .FirstOrDefaultAsync(sp => sp.SaleId == model.SaleId && sp.ProductId == model.ProductId);

            if (existingSaleProduct != null)
            {
                UpdateSaleProductDTO updateSaleProduct = new()
                {
                    Quantity = existingSaleProduct.Quantity + model.Quantity,
                };
                await UpdateSaleProduct(updateSaleProduct, existingSaleProduct.SaleProductId);
                return;
            }

            UpdateSaleDTO updateSaleDTO = new()
            {
                Subtotal = sale.Subtotal + (product.Price * model.Quantity)
            };

            UpdateProductDTO updateProductDTO = new()
            {
                Stock = product.Stock - model.Quantity
            };

            Console.WriteLine($"{updateProductDTO.Stock} = {product.Stock} - {model.Quantity}");


            var succes = await _saleService.UpdateSale(updateSaleDTO, sale.SaleId) && await _productService.UpdateProduct(updateProductDTO, product.ProductId);

            if (!succes) throw new Exception("Estoque/Subtotal não conseguiu ser atualizado");


            SaleProduct saleProduct = _mapper.Map<SaleProduct>(model);

            _context.SaleProducts.Add(saleProduct);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<SaleProductDTO>> GetAllSaleProducts()
        {
            var saleProducts = await _context.SaleProducts
            .Include(sp => sp.Sale)
            .Include(sp => sp.Product)
            .ToListAsync();
            return _mapper.Map<IEnumerable<SaleProductDTO>>(saleProducts);
        }
        public async Task<SaleProductDTO> GetSaleProductById(int id)
        {
            var saleProduct = await _context.SaleProducts
            .Include(sp => sp.Sale)
            .Include(sp => sp.Product)
            .FirstOrDefaultAsync(sp => sp.SaleProductId == id);
            return _mapper.Map<SaleProductDTO>(saleProduct);
        }
        public async Task<IEnumerable<ProductsInSaleDTO>> GetAllProductsInSale(int id)
        {
            var products = await _context.SaleProducts
                .Include(sp => sp.Product)
                .Where(sp => sp.SaleId == id)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProductsInSaleDTO>>(products);
        }
        public async Task<IEnumerable<SalesForProductDTO>> GetAllSalesForProduct(int id)
        {
            var sales = await _context.SaleProducts
                .Include(sp => sp.Sale)
                .Where(sp => sp.ProductId == id)
                .ToListAsync();
            return _mapper.Map<IEnumerable<SalesForProductDTO>>(sales);
        }
        public async Task<bool> UpdateSaleProduct(UpdateSaleProductDTO model, int id)
        {
            var saleProduct = await _context.SaleProducts.FindAsync(id);
            if (saleProduct == null) return false;
            if (model.SaleId != null)
            {
                _ = await _saleService.GetSaleById((int)model.SaleId) ?? throw new Exception($"Venda {model.SaleId} não existe!");
            }
            if (model.ProductId != null)
            {
                _ = await _productService.GetProductById((int)model.ProductId) ?? throw new Exception($"Produto {model.ProductId} não existe!");
            }

            var sale = await _saleService.GetSaleById(saleProduct.SaleId) ?? throw new Exception($"Venda {saleProduct.SaleId} não existe");
            var product = await _productService.GetProductById(saleProduct.ProductId) ?? throw new Exception($"Produto {saleProduct.ProductId} não existe");

            UpdateSaleDTO updateSaleDTO = new()
            {
                Subtotal = sale.Subtotal + (product.Price * (model.Quantity > saleProduct.Quantity ?
                    model.Quantity - saleProduct.Quantity :
                    saleProduct.Quantity - model.Quantity))
            };

            UpdateProductDTO updateProductDTO = new()
            {
                Stock = product.Stock + saleProduct.Quantity
            };

            var succes = await _saleService.UpdateSale(updateSaleDTO, sale.SaleId) && await _productService.UpdateProduct(updateProductDTO, product.ProductId);

            if (!succes) throw new Exception("Estoque/Subtotal não conseguiu ser atualizado");

            saleProduct.SaleId = model.SaleId ?? saleProduct.SaleId;
            saleProduct.ProductId = model.ProductId ?? saleProduct.ProductId;
            saleProduct.Quantity = model.Quantity ?? saleProduct.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSaleProduct(int id)
        {
            var saleProduct = await _context.SaleProducts.FindAsync(id);
            if (saleProduct == null) return false;

            var sale = await _saleService.GetSaleById(saleProduct.SaleId) ?? throw new Exception($"Venda {saleProduct.SaleId} não existe");
            var product = await _productService.GetProductById(saleProduct.ProductId) ?? throw new Exception($"Produto {saleProduct.ProductId} não existe");

            UpdateSaleDTO updateSaleDTO = new()
            {
                Subtotal = sale.Subtotal - (product.Price * saleProduct.Quantity)
            };

            UpdateProductDTO updateProductDTO = new()
            {
                Stock = product.Stock + saleProduct.Quantity
            };

            var succes = await _saleService.UpdateSale(updateSaleDTO, sale.SaleId) && await _productService.UpdateProduct(updateProductDTO, product.ProductId);

            if (!succes) throw new Exception("Estoque/Subtotal não conseguiu ser atualizado");

            _context.SaleProducts.Remove(saleProduct);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}