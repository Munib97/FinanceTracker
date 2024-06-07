using Finance_Management.Models;

namespace Finance_Management.Data
{
    public class SubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public string Name { get; set; }
        public SubscriptionFrequency Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount {  get; set; }
        public int CategoryId { get; set; }
    }
}