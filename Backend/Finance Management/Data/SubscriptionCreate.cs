using Finance_Management.Models;
using System.ComponentModel.DataAnnotations;

namespace Finance_Management.Data
{
    public class SubscriptionCreate
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public SubscriptionFrequency? Frequency { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}