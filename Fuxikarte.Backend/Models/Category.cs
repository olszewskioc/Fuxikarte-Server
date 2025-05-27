using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuxikarte.Backend.Models
{
    [Table("categories")]
    public class Category
    {
        [Column("category_id")]
        [Key]
        [Required]
        public int CategoryId { get; set; }

        [Column("category_name")]
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public List<Product> Products { get; set; } = new List<Product>();
    }
}