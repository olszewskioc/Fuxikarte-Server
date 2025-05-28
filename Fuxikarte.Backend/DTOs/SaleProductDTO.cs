using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.DTOs
{
    public class NewSaleProductDTO
    {
        [Required]
        public required int SaleId { get; set; }

        [Required]
        public required int ProductId { get; set; }

        public required int Quantity { get; set; }
    }
    public class UpdateSaleProductDTO
    {
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class SaleProductDTO
    {
        public required int SaleProductId { get; set; }
        public required SaleDTO Sale { get; set; }
        public required ProductDTO Product { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ProductsInSaleDTO
    {
        public required ProductDTO Product { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class SalesForProductDTO
    {
        public required SaleDTO Sale { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}