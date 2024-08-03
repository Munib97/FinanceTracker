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

        public bool ExpenseExists(int id, string userId)
        {
            return _context.expenses.Where(e => e.ExpenseId == id).Any();
        }

        public ICollection<Expense> GetExpenseByDate(DateTime date, string userId)
        {
            return _context.expenses.Where(e => e.DateSpent == date && e.UserId == userId).ToList();
        }

        public Expense GetExpenseById(int id, string userId)
        {
            var expense = _context.expenses.Find(id);
            return expense;
            //var query = _context.expenses
            //                    .Where(i => i.UserId == userId && i.ExpenseId == id)
            //                    .Select(e => new Expense
            //                    {
            //                        Name = e.Name,
            //                        Amount = e.Amount,
            //                        DateSpent = e.DateSpent,
            //                    }).FirstOrDefault();
            //return query;
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
                    CategoryId = e.CategoryId,

                })
                .ToList();
        }


        public ICollection<Expense> GetExpensesByCategory(int categoryId, string userId)
        {
            return _context.expenses.Where(e => categoryId == e.CategoryId && e.UserId == userId).ToList();
        }

        public bool PostExpense(Expense expense, string userId)
        {
            expense.UserId = userId;
            _context.Add(expense);
            return Save();
        }

        public bool PutExpense(int id, ExpenseUpdateDTO expenseDTO)
        {
            var expense = _context.expenses.Find(id);

            if (expenseDTO.Name != null)
            {
                expense.Name = expenseDTO.Name;
            }
            if (expenseDTO.Amount != null)
            {
                expense.Amount = (decimal)expenseDTO.Amount;
            }
            if (expenseDTO.CategoryId != null)
            {
                expense.CategoryId = (int)expenseDTO.CategoryId;
            }
            if (expenseDTO.DateSpent != null)
            {
                expense.DateSpent = (DateTime)expenseDTO.DateSpent;
            }
            _context.Update(expense);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
