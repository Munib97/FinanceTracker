using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Repositories
{
    public interface IExpenseRepository
    {
        ICollection<Expense> GetExpensesByCategory(int categoryId, string userId);
        Expense GetExpenseById(int id, string userId);
        ICollection<Expense> GetExpenseByUserId(string userId);
        public ICollection<Expense> GetExpenseByDate(DateTime date, string userId);
        bool PutExpense(int id, ExpenseUpdateDTO expense);
        bool PostExpense(Expense expense, string userId);
        bool DeleteExpense(Expense expense);
        bool ExpenseExists(int id, string userId);
        bool Save();
    }
}
