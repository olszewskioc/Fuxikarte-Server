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
    public class SaleProductDTO
    {
        public required int SaleProductId { get; set; }
        public required SaleDTO SaleDTO { get; set; }
        public required ProductDTO ProductDTO { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ProductsInSaleDTO
    {
        public required ProductDTO ProductDTO { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class SalesForProductDTO
    {
        public required SaleDTO SaleDTO { get; set; }
        public required int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}