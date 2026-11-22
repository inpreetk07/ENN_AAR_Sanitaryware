
using System.ComponentModel.DataAnnotations;

namespace ENN_AAR_Sanitaryware.Models
{
    public class Enquiry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
