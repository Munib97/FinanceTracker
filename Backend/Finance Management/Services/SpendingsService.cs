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

        public async Task<decimal> CalculateCurrentMonthSubscriptions(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var query = _context.subscriptions.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(s => s.UserId == userId && s.DueDate >= startDate && s.DueDate <= endDate);
            }
            else
            {
                var today = DateTime.Today;
                query = query.Where(s => s.UserId == userId && s.DueDate <= today);
            }

            return await query.SumAsync(s => s.Amount);
        }


        public async Task<decimal> CalculateCurrentMonthExpenses(string userId, DateTime? startDate = null, DateTime? endDate = null, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var query = _context.expenses.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.UserId == userId && e.DateSpent >= startDate && e.DateSpent <= endDate && e.Category.Name == category);
            }
            else
            {
                var now = DateTime.Now;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                query = query.Where(e => e.UserId == userId && e.DateSpent >= startOfMonth && e.DateSpent <= endOfMonth);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category.Name == category);
            }

            return await query.SumAsync(e => e.Amount);
        }

        public async Task<decimal> CalculateCurrentMonthSpending(string userId, DateTime? startDate = null, DateTime? endDate = null, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var expenses = await CalculateCurrentMonthExpenses(userId, startDate, endDate, category);
            var subscriptions = await CalculateCurrentMonthSubscriptions(userId, startDate, endDate);

            return expenses + subscriptions;
        }

        public async Task<List<CategorySpending>> GetSpendingByCategory(string userId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            return await _context.expenses
                .Where(e => e.UserId == userId && e.DateSpent >= startDate && e.DateSpent <= endDate)
                .GroupBy(e => e.Category.Name)
                .Select(g => new CategorySpending
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToListAsync();
        }
    }


    public class CategorySpending
    {
        public string? Category { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
