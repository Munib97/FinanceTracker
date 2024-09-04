using Finance_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Services
{
    public class SpendingsService
    {
        private readonly DataContext _context;
        private readonly SubscriptionService _subscriptionService;

        public SpendingsService(DataContext context, SubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public async Task<decimal> CalculateCurrentMonthSubscriptions(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var today = DateTime.Today;
            decimal total = await _context.subscriptions
                .Where(s => s.UserId == userId && s.DueDate <= today)
                .SumAsync(s => s.Amount);

            return total;
        }

        public async Task<decimal> CalculateCurrentMonthExpenses(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            decimal currentMonthExpenses = await _context.expenses
                .Where(e => e.UserId == userId && e.DateSpent >= startOfMonth && e.DateSpent <= endOfMonth)
                .SumAsync(e => e.Amount);

            return currentMonthExpenses;
        }

        public async Task<decimal> CalculateCurrentMonthSpending(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var expenses = await CalculateCurrentMonthExpenses(userId);
            var subscriptions = await CalculateCurrentMonthSubscriptions(userId);

            return expenses + subscriptions;
        }
    }
}
