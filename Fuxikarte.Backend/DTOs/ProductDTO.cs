using System.ComponentModel.DataAnnotations;
using Fuxikarte.Backend.Models;

namespace Fuxikarte.Backend.DTOs
{
    public class NewProductDTO
    {
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public required string ProductName { get; set; }

        [Required]
        public required int CategoryId { get; set; }

        public string? Description { get; set; }

        [Required]
        public required int Stock { get; set; }

        [Required]
        public required decimal Cost { get; set; }

        [Required]
        public required decimal Price { get; set; }
    }
    public class ProductNavDTO
    {
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public string Description { get; set; } = string.Empty;
        public required int Stock { get; set; }
        public required decimal Cost { get; set; }
        public required decimal Price { get; set; }
        public CategoryDTO? Category { get; set; }
        public ICollection<SalesForProductDTO> SalesProduct { get; set; } = new List<SalesForProductDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ProductDTO
    {
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public string Description { get; set; } = string.Empty;
        public required int Stock { get; set; }
        public required decimal Cost { get; set; }
        public required decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateProductDTO
    {
        [StringLength(150, MinimumLength = 3)]
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? Stock { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
    }
}