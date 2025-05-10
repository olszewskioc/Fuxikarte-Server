using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.Models
{
    [Table("customers")]
    public class Customer
    {
        [Column("customer_id")]
        [Key]
        [Required]
        public int CustomerId { get; set; }

        [Column("customer_name")]
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; } = string.Empty;

        [Column("phone")]
        [Required]
        public string Phone { get; set; } = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Sale> Sales { get; set; } = [];
    }
}