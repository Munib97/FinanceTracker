using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Finance_Management.Services;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Http;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BalanceService _balanceService;
        private readonly SpendingsService _currentMonthSpendingService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ExpensesController(DataContext context, UserManager<IdentityUser> userManager, BalanceService balanceService, SpendingsService currentMonthSpendingService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _balanceService = balanceService;
            _currentMonthSpendingService = currentMonthSpendingService;
            _contextAccessor = httpContextAccessor;
        }

        // Moved to a new Controller

        //[HttpGet("currentMonthSpending/{userId}")]
        //public async Task<decimal> GetCurrentMonthExpenses(string userId)
        //{
        //    var currentMonthSpending = await _currentMonthSpendingService.CalculateCurrentMonthSpending(userId);
        //    return currentMonthSpending;
        //}

        // GET: api/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> Getexpenses()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var expenses = await _context.expenses
                .Where(i => i.UserId == userId)
                .Select(e => new ExpenseDto
                {
                    
                    ExpenseId = e.ExpenseId,
                    Name = e.Name,
                    Amount = e.Amount,
                })
                .ToListAsync();
            return expenses;
        }

        [HttpGet("balance/user")]
        public decimal GetBalance()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return _balanceService.CalculateTotalBalance(userId);
        }

        // GET: api/expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> Getexpense(int id)
        {
            var expense = await _context.expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpGet("user/")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenseByUserId()
        {
            var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var expense = await _context.expenses.Where(e => e.UserId == userId).ToListAsync();

            if(expense == null)
            {
                return NotFound();
            }
            return expense;
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenseByDate(DateTime date)
        {
            var expense = await _context.expenses.Where(e => e.DateSpent < date).ToListAsync();
            if(expense == null)
            {
                return NotFound();
            }
            return expense;
        }

        // PUT: api/expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // POST: api/expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> PostExpense([FromBody]ExpenseDto expenseDto)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var expense = new Expense
            {
                Amount = expenseDto.Amount,
                Name = expenseDto.Name,
                DateSpent = expenseDto.DateSpent,
                UserId = userId,
                CategoryId = expenseDto.CategoryId,
            };
            _context.expenses.Add(expense);
            await _context.SaveChangesAsync();

            var expenseDtoResult = new ExpenseDto
            {
                ExpenseId = expense.ExpenseId,
                Amount = expense.Amount,
                Name = expense.Name,
                DateSpent = expense.DateSpent,
            };
            return CreatedAtAction("Getexpense", new { id = expense.ExpenseId }, expenseDtoResult);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expenses = await _context.expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }

            _context.expenses.Remove(expenses);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return _context.expenses.Any(e => e.ExpenseId == id);
        }
    }
}