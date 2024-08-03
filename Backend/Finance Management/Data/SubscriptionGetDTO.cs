using Finance_Management.Models;

namespace Finance_Management.Data
{
    public class SubscriptionGetDTO
    {
        public string? Name { get; set; }
        public SubscriptionFrequency? Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Amount { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? Date { get; set; }
        public string CategoryName { get; set; }
        public string Type { get; set; }
    }
}
