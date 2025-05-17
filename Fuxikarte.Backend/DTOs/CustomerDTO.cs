using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.DTOs
{
    public class NewCustomerDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string CustomerName { get; set; }
        [Required]
        [StringLength(13)]
        public required string Phone { get; set; }
    }
    public class CustomerDTO
    {
        public required int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<SaleDTO> Sales { get; set; } = new List<SaleDTO>();
    }
    public class CustomerSaleDTO
    {
        public required int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateCustomerDTO
    {
        [StringLength(100, MinimumLength = 2)]
        public string? CustomerName { get; set; }

        [StringLength(13)]
        public string? Phone { get; set; }
    }
}