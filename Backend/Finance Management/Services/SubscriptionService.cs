using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Services
{
    public class SubscriptionService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(IServiceScopeFactory scopeFactory, ILogger<SubscriptionService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task ProcessDueSubscriptions(string? userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                try
                {
                    var today = DateTime.Now.Date;

                    var query = context.subscriptions.Where(s => s.DueDate == today);

                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        query = query.Where(s => s.UserId == userId);
                    }

                    var dueSubscriptions = await query.ToListAsync();

                    foreach (var subscription in dueSubscriptions)
                    {
                        subscription.DueDate = subscription.Frequency switch
                        {
                            SubscriptionFrequency.Weekly => subscription.DueDate.AddDays(7),
                            SubscriptionFrequency.Monthly => subscription.DueDate.AddMonths(1),
                            SubscriptionFrequency.Quarterly => subscription.DueDate.AddMonths(3),
                            SubscriptionFrequency.Biannually => subscription.DueDate.AddMonths(6),
                            SubscriptionFrequency.Yearly => subscription.DueDate.AddYears(1),
                            _ => subscription.DueDate
                        };

                        _logger.LogInformation($"Subscription {subscription.Name} for user {subscription.UserId} renewed. Next due date: {subscription.DueDate}");
                    }

                    await context.SaveChangesAsync();
                    _logger.LogInformation("Processed due subscriptions successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing due subscriptions.");
                }
            }
        }
    }
}



//public async Task<List<Subscription>> GetDueSubscriptions(string userId)
//{
//    try
//    {
//        var today = DateTime.Today;
//        var subscriptions = await _context.subscriptions
//            .Where(s => s.UserId == userId)
//            .ToListAsync();

//        return subscriptions
//            .Where(subscription => IsSubscriptionDue(subscription, today))
//            .ToList();
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "An error occurred while fetching due subscriptions for user {UserId}", userId);
//        throw new ApplicationException("Unable to fetch due subscriptions at this time. Please try again later.");
//    }
//}

//private bool IsSubscriptionDue(Subscription subscription, DateTime referenceDate)
//{
//    try
//    {
//        DateTime nextDueDate = subscription.StartDate;

//        while (nextDueDate <= referenceDate)
//        {
//            if (nextDueDate == referenceDate || nextDueDate.AddDays(-1) == referenceDate)
//            {
//                return true;
//            }

//            switch (subscription.Frequency)
//            {
//                case SubscriptionFrequency.Weekly:
//                    nextDueDate = nextDueDate.AddDays(7);
//                    break;
//                case SubscriptionFrequency.Monthly:
//                    nextDueDate = nextDueDate.AddMonths(1);
//                    break;
//                case SubscriptionFrequency.Biannually:
//                    nextDueDate = nextDueDate.AddMonths(6);
//                    break;
//                case SubscriptionFrequency.Yearly:
//                    nextDueDate = nextDueDate.AddYears(1);
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException($"Unknown frequency: {subscription.Frequency}");
//            }
//        }
//        return false;
//    }
//    catch (ArgumentOutOfRangeException ex)
//    {
//        _logger.LogError(ex, "Invalid frequency value for subscription {SubscriptionId}", subscription.SubscriptionId);
//        throw new ApplicationException($"Subscription {subscription.Name} has an invalid frequency setting.");
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "An error occurred while determining if subscription {SubscriptionId} is due", subscription.SubscriptionId);
//        throw new ApplicationException("Unable to determine if the subscription is due at this time.");
//    }
//}
//    }
//}
