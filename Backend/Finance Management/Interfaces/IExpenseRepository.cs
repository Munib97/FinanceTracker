using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Repositories
{
    public interface IExpenseRepository
    {
        ICollection<Expense> GetExpensesByCategory(int categoryId);
        Expense GetExpenseById(int id);
        ICollection<Expense> GetExpenseByUserId(string userId);
        public ICollection<Expense> GetExpenseByDate(DateTime date);
        bool PutExpense(int id, ExpenseUpdateDTO expenseDTO);
        bool PostExpense(ExpenseCreate expenseDTO);
        bool DeleteExpense(Expense expense);
        bool ExpenseExists(int id);
        bool Save();
    }
}

