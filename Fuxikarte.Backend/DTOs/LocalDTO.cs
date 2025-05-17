using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.DTOs
{
    public class NewLocalDTO
    {
        [Required]
        [StringLength(100)]
        public required string LocalName { get; set; }
        public string Description { get; set; } = string.Empty;
    }
    public class LocalDTO
    {
        public required int LocalId { get; set; }
        public required string LocalName { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<SaleDTO> Sales { get; set; } = new List<SaleDTO>();
    }
    public class LocalSaleDTO
    {
        public required int LocalId { get; set; }
        public required string LocalName { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateLocalDTO
    {
        [StringLength(100)] 
        public string? LocalName { get; set; }
        public string? Description { get; set; }
    }
}