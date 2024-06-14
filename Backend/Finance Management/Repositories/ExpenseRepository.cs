using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContext _context;

        public ExpenseRepository(DataContext context)
        {
            _context = context;
        }

        public bool DeleteExpense(Expense expense)
        {
            _context.Remove(expense);
            return Save();
        }

        public bool ExpenseExists(int id)
        {
            return _context.expenses.Where(e => e.ExpenseId == id).Any();
        }

        public ICollection<Expense> GetExpenseByDate(DateTime date)
        {
            return _context.expenses.Where(e => e.DateSpent == date).ToList();
        }

        public Expense GetExpenseById(int id)
        {
            return _context.expenses.Where(e => e.ExpenseId == id).FirstOrDefault();
        }

        public ICollection<Expense> GetExpenseByUserId(string userId)
        {
            return _context.expenses
                .Where(i => i.UserId == userId)
                .Select(e => new Expense
                {
                    ExpenseId = e.ExpenseId,
                    Name = e.Name,
                    Amount = e.Amount,
                    DateSpent = e.DateSpent,
                    CategoryId = e.CategoryId
                })
                .ToList();
        }


        public ICollection<Expense> GetExpensesByCategory(int categoryId)
        {
            return _context.expenses.Where(e => categoryId == e.CategoryId).ToList();
        }

        public bool PostExpense(ExpenseCreate expenseDTO)
        {
            _context.Add(expenseDTO);
            return Save();
        }

        public bool PutExpense(int id, ExpenseUpdateDTO expenseDTO)
        {
            _context.Update(expenseDTO);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
