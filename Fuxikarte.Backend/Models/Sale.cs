using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.Models
{
    public enum Payment
    {
        Dinheiro, Pix, Débito, Crédito
    }

    [Table("sales")]
    public class Sale
    {
        [Column("sale_id")]
        [Key]
        [Required]
        public int SaleId { get; set; }

        [Column("local_id")]
        [Required]
        public int LocalId { get; set; }

        [Column("customer_id")]
        [Required]
        public int CustomerId { get; set; }

        [Column("payment")]
        [Required]
        public Payment Payment { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(LocalId))]
        public Local? Local { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
    }
}