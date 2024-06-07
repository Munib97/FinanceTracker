using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
            var subscriptions = await _context.subscriptions.Where(s => s.UserId == userId).ToListAsync();
            var dueSubscriptions = subscriptions.Where(s => s.DueDate <= DateTime.Today).ToList();
            decimal total = 0;
            foreach(var subscription in dueSubscriptions)
            {
                total += subscription.Amount;
            }
            return total;
        }

        public async Task<decimal> CalculateCurrentMonthExpenses(string userId)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            decimal currentMonthExpenses = await _context.expenses.Where(i => i.UserId == userId && i.DateSpent >= startOfMonth && i.DateSpent <= endOfMonth).SumAsync(i => i.Amount);
            return currentMonthExpenses;
        }

        public async Task<decimal> CalculateCurrentMonthSpending(string userId)
        {
            return await CalculateCurrentMonthExpenses(userId) + await CalculateCurrentMonthSubscriptions(userId);
        }
    }
}
