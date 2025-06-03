using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Fuxikarte.Backend.Models
{
    public enum Payment
    {
        [EnumMember(Value = "dinheiro")]
        Dinheiro,
        [EnumMember(Value = "pix")]
        Pix,
        [EnumMember(Value = "debito")]
        Débito,
        [EnumMember(Value = "credito")]
        Crédito
    }

    [Table("sales")]
    public class Sale
    {
        [Column("sale_id")]
        [Key]
        [Required]
        public int SaleId { get; set; }

        [Column("local_id")]
        public int? LocalId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("payment")]
        [Required]
        public Payment Payment { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("paid")]
        public bool Paid { get; set; } 

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