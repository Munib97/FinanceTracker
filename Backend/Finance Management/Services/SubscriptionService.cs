using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Services
{
    public class SubscriptionService
    {
        private readonly DataContext _context;

        public SubscriptionService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Subscription>> GetDueSubscriptions(string userId)
        {
            var subscriptions = await _context.subscriptions.Where(i => i.UserId == userId).ToListAsync();
            var dueSubs = new List<Subscription>();
            foreach (var subscription in subscriptions)
            {
                if (IsSubscriptionDue(subscription))
                {
                    dueSubs.Add(subscription);
                }
            }
            return dueSubs;
        }

        private bool IsSubscriptionDue(Subscription subscription)
        {
            DateTime dueDate;
            switch (subscription.Frequency)
            {
                case SubscriptionFrequency.Weekly:
                    dueDate = subscription.StartDate.AddDays(7);
                    break;
                case SubscriptionFrequency.Monthly:
                    dueDate = subscription.StartDate.AddMonths(1);
                    break;
                case SubscriptionFrequency.Biannually:
                    dueDate = subscription.StartDate.AddMonths(6);
                    break;
                case SubscriptionFrequency.Yearly:
                    dueDate = subscription.StartDate.AddYears(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return dueDate <= DateTime.Today;
        }
    }
}
