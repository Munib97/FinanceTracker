using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ExpenseRepository> _logger;

        public ExpenseRepository(DataContext context, ILogger<ExpenseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool DeleteExpense(Expense expense)
        {
            try
            {
                _context.Remove(expense);
                return Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting expense {ExpenseId}", expense.ExpenseId);
                return false;
            }
        }

        public bool ExpenseExists(int id, string userId)
        {
            try
            {
                return _context.expenses.Any(e => e.ExpenseId == id && e.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if expense {ExpenseId} exists for user {UserId}", id, userId);
                return false;
            }
        }

        public ICollection<Expense> GetExpenseByDate(DateTime date, string userId)
        {
            try
            {
                return _context.expenses.Where(e => e.DateSpent == date && e.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving expenses for date {Date} and user {UserId}", date, userId);
                return new List<Expense>();
            }
        }

        public Expense GetExpenseById(int id, string userId)
        {
            try
            {
                return _context.expenses.FirstOrDefault(e => e.ExpenseId == id && e.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving expense {ExpenseId} for user {UserId}", id, userId);
                return null;
            }
        }

        public ICollection<Expense> GetExpenseByUserId(string userId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving expenses for user {UserId}", userId);
                return new List<Expense>();
            }
        }

        public ICollection<Expense> GetExpensesByCategory(int categoryId, string userId)
        {
            try
            {
                return _context.expenses.Where(e => e.CategoryId == categoryId && e.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving expenses for category {CategoryId} and user {UserId}", categoryId, userId);
                return new List<Expense>();
            }
        }

        public bool PostExpense(Expense expense, string userId)
        {
            try
            {
                expense.UserId = userId;
                _context.Add(expense);
                return Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting a new expense for user {UserId}", userId);
                return false;
            }
        }

        public bool PutExpense(int id, ExpenseUpdateDTO expenseDTO)
        {
            try
            {
                var expense = _context.expenses.Find(id);

                if (expense == null)
                {
                    _logger.LogWarning("Expense {ExpenseId} not found for update", id);
                    return false;
                }

                if (expenseDTO.Name != null)
                {
                    expense.Name = expenseDTO.Name;
                }
                if (expenseDTO.Amount.HasValue)
                {
                    expense.Amount = expenseDTO.Amount.Value;
                }
                if (expenseDTO.CategoryId.HasValue)
                {
                    expense.CategoryId = expenseDTO.CategoryId.Value;
                }
                if (expenseDTO.DateSpent.HasValue)
                {
                    expense.DateSpent = expenseDTO.DateSpent.Value;
                }
                _context.Update(expense);
                return Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating expense {ExpenseId}", id);
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database");
                return false;
            }
        }
    }
}
