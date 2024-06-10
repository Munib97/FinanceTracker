﻿using Finance_Management.Models;

namespace Finance_Management.Data
{
    public record SubscriptionUpdate
     (
        int? SubscriptionId,
        string? Name,
        decimal? Amount,
        SubscriptionFrequency? Frequency,
        DateTime? StartDate,
        DateTime? EndDate,
        int? CategoryId
     );
}
