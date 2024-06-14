using AutoMapper;
using Finance_Management.Data;
using Finance_Management.Models;
using Finance_Management.Repositories;
using Finance_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;

        public ExpensesController(DataContext context, UserManager<IdentityUser> userManager, BalanceService balanceService, SpendingsService currentMonthSpendingService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IExpenseRepository expenseRepository)
        {
            _context = context;
            _userManager = userManager;
            _balanceService = balanceService;
            _currentMonthSpendingService = currentMonthSpendingService;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        // Moved to a new Controller

        //[HttpGet("currentMonthSpending/{userId}")]
        //public async Task<decimal> GetCurrentMonthExpenses(string userId)
        //{
        //    var currentMonthSpending = await _currentMonthSpendingService.CalculateCurrentMonthSpending(userId);
        //    return currentMonthSpending;
        //}

        // GET: api/expenses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);

        //}
        [HttpGet("Category{categoryId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesByCategory(int categoryId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var expenses = await _context.expenses
                .Where(i => i.UserId == userId)
                .Where(c => c.CategoryId == categoryId)
                .Select(e => new Expense
                {
                    ExpenseId = e.ExpenseId,
                    Name = e.Name,
                    DateSpent = e.DateSpent,
                    Amount = e.Amount,
                    CategoryId = e.CategoryId
                }).ToListAsync();
            return expenses;
        }

        [HttpGet("balance/user")]
        public decimal GetBalance()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return _balanceService.CalculateTotalBalance(userId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(int id)
        {
            var expense = await _context.expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpGet("user/")]
        public async Task<ActionResult<Expense>> GetExpenseByUserId()
        {
            var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var expenses = _mapper.Map<List<ExpenseGetDTO>>(_expenseRepository.GetExpenseByUserId(userId));
            return Ok(expenses);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenseByDate(DateTime date)
        {
            var expense = await _context.expenses.Where(e => e.DateSpent < date).ToListAsync();
            if (expense == null)
            {
                return NotFound();
            }
            return expense;
        }

        // PUT: api/expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, ExpenseUpdateDTO expenseDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId == null)
            {
                return BadRequest("User not found");
            }

            var expense = await _context.expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            if (expense.UserId != userId)
            {
                return Forbid();
            }

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
        public async Task<ActionResult<ExpenseCreate>> PostExpense([FromBody] ExpenseCreate expenseDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var expense = _mapper.Map<Expense>(expenseDTO);

            _context.expenses.Add(expense);
            await _context.SaveChangesAsync();

            var expenseDtoResult = new ExpenseCreate
            {
                Amount = expense.Amount,
                Name = expense.Name,
                CategoryId = expense.CategoryId,
                DateSpent = expense.DateSpent,
            };
            return CreatedAtAction("GetExpense", new { id = expense.ExpenseId }, expenseDtoResult);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {

            var expenseToDelete = _expenseRepository.GetExpenseById(id);
            if (!_expenseRepository.DeleteExpense(expenseToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return _context.expenses.Any(e => e.ExpenseId == id);
        }
    }
}
