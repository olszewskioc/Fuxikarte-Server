using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuxikarte.Backend.Models
{
    [Table("products")]
    public class Product
    {
        [Column("product_id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("product_name")]
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;
        
        [Column("description")]
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Column("stock")]
        [Required]
        public int Stock { get; set; }

        [Column("cost")]
        [Required]
        public decimal Cost { get; set; }
        
        [Column("price")]
        [Required]
        public decimal Price { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public List<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
    }
}