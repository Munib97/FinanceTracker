using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_Management.Models
{
    public enum SubscriptionFrequency
    {
        Weekly,
        Monthly,
        Quarterly,
        Biannually,
        Yearly
    }
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        [Required]
        public SubscriptionFrequency Frequency { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public DateTime DueDate
        {
            get
            {
                switch (Frequency)
                {
                    case SubscriptionFrequency.Weekly:
                        return StartDate.AddDays(7);
                    case SubscriptionFrequency.Monthly:
                        return StartDate.AddMonths(1);
                    case SubscriptionFrequency.Quarterly:
                        return StartDate.AddMonths(3);
                    case SubscriptionFrequency.Biannually:
                        return StartDate.AddMonths(6);
                    case SubscriptionFrequency.Yearly:
                        return StartDate.AddYears(1);
                    default:
                        throw new ArgumentOutOfRangeException("Frequency");

                }
            }
        }
    }
}