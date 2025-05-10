using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.Models
{
    [Table("locals")]
    public class Local
    {
        [Column("local_id")]
        [Key]
        [Required]
        public int LocalId { get; set; }

        [Column("local_name")]
        [Required]
        [StringLength(50)]
        public string LocalName { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}