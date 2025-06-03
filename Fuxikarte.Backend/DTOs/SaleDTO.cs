using System.ComponentModel.DataAnnotations;
using Fuxikarte.Backend.Models;

namespace Fuxikarte.Backend.DTOs
{
    public class NewSaleDTO
    {
        [Required]
        public required int LocalId { get; set; }

        [Required]
        public required int CustomerId { get; set; }

        public required Payment Payment { get; set; }
        public required bool Paid { get; set; }

    }

    public class SaleDTO
    {
        public required int SaleId { get; set; }
        public required int LocalId { get; set; }
        public required int CustomerId { get; set; }
        public required decimal Subtotal { get; set; }
        public required Payment Payment { get; set; }
        public required bool Paid { get; set; }
    }

    public class SaleNavDTO
    {
        public required int SaleId { get; set; }
        public required LocalDTO Local { get; set; }
        public required CustomerDTO Customer { get; set; }
        public required Payment Payment { get; set; }
        public required decimal Subtotal { get; set; }
        public required bool Paid { get; set; }
        public ICollection<ProductsInSaleDTO> SaleProducts { get; set; } = new List<ProductsInSaleDTO>(); 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateSaleDTO
    {
        public int? LocalId { get; set; }
        public int? CustomerId { get; set; }
        public Payment? Payment { get; set; }
        public decimal? Subtotal { get; set; }
        public bool? Paid { get; set; }
    }
}