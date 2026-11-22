
using System.ComponentModel.DataAnnotations;

namespace ENN_AAR_Sanitaryware.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
