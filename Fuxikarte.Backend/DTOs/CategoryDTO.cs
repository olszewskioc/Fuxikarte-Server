using System.ComponentModel.DataAnnotations;
using Fuxikarte.Backend.Models;

namespace Fuxikarte.Backend.DTOs
{
    public class NewCategoryDTO
    {
        [Required]
        [StringLength(100)]
        public required string CategoryName { get; set; }
    }
    public class CategoryDTO
    {
        public required string CategoryName { get; set; }
    }
    public class CategoryNavDTO
    {
        public required string CategoryName { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
    public class UpdateCategoryDTO
    {
        public string? CategoryName { get; set; }
    }
}