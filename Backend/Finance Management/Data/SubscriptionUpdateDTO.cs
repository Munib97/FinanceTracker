using Finance_Management.Models;

namespace Finance_Management.Data
{
    public record SubscriptionUpdateDTO
     (
        int? SubscriptionId,
        string? Name,
        decimal? Amount,
        SubscriptionFrequency? Frequency,
        DateTime? StartDate,
        DateTime? EndDate,
        DateTime? DueDate,
        int? CategoryId
     );
}