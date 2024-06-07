using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            decimal totalBalance = CalculateTotalIncome(userId) - CalculateTotalExpenses(userId);
            return totalBalance;
        }

        public decimal CalculateTotalIncome(string userId) 
        {
            decimal totalIncome = _context.incomes.Where(i => i.UserId == userId).Sum(i => i.Amount);
            return totalIncome;
        }

        public decimal CalculateTotalExpenses(string userId) 
        {
            decimal totalExpenses = _context.expenses.Where(i => i.UserId == userId).Sum(i => i.Amount);
            return totalExpenses;
        }
    }
}