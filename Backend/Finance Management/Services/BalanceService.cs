using Finance_Management.Data;

namespace Finance_Management.Services
{
    public class BalanceService
    {
        private readonly DataContext _context;

        public BalanceService(DataContext context)
        {
            _context = context;
        }

        public decimal CalculateTotalBalance(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var totalIncome = CalculateTotalIncome(userId);
            var totalExpenses = CalculateTotalExpenses(userId);

            return totalIncome - totalExpenses;
        }

        public decimal CalculateTotalIncome(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            return _context.incomes
                           .Where(i => i.UserId == userId)
                           .Sum(i => i.Amount);
        }

        public decimal CalculateTotalExpenses(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            return _context.expenses
                           .Where(e => e.UserId == userId)
                           .Sum(e => e.Amount);
        }
    }
}
