using AutoMapper;
using Finance_Management.Data;
using Finance_Management.Models;
using Finance_Management.Repositories;
using Finance_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public ExpensesController(UserManager<IdentityUser> userManager, BalanceService balanceService, SpendingsService currentMonthSpendingService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IExpenseRepository expenseRepository)
        {
            _balanceService = balanceService;
            _currentMonthSpendingService = currentMonthSpendingService;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
            _expenseRepository = expenseRepository;
            _userManager = userManager;
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
            var expenses = _expenseRepository.GetExpensesByCategory(categoryId, userId);
            return Ok(expenses);

            //    var userId = _userManager.GetUserId(HttpContext.User);

            //    var expenses = await _context.expenses
            //        .Where(i => i.UserId == userId)
            //        .Where(c => c.CategoryId == categoryId)
            //        .Select(e => new Expense
            //        {
            //            ExpenseId = e.ExpenseId,
            //            Name = e.Name,
            //            DateSpent = e.DateSpent,
            //            Amount = e.Amount,
            //            CategoryId = e.CategoryId
            //        }).ToListAsync();
            //    return expenses;
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
            var userId = _userManager.GetUserId(HttpContext.User);
            var expense = _mapper.Map<ExpenseGetDTO>(_expenseRepository.GetExpenseById(id, userId));

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpGet("user/")]
        public async Task<ActionResult<Expense>> GetExpenseByUserId()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var expenses = _mapper.Map<List<ExpenseGetDTO>>(_expenseRepository.GetExpenseByUserId(userId));
            return Ok(expenses);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenseByDate(DateTime date)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var expenses = _mapper.Map<List<ExpenseGetDTO>>(_expenseRepository.GetExpenseByDate(date, userId));
            return Ok(expenses);
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

            //var expense = _expenseRepository.GetExpenseById(id, userId);
            //var expense = await _context.expenses.FindAsync(id);


            //_context.Entry(expense).State = EntityState.Modified;
            var updatedExpense = _mapper.Map<ExpenseUpdateDTO>(expenseDTO);
            _expenseRepository.PutExpense(id, expenseDTO);
            return Ok(updatedExpense);
        }
        // POST: api/expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpenseCreate>> PostExpense(ExpenseCreate expenseDTO)
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
            _expenseRepository.PostExpense(expense, userId);

            //_context.expenses.Add(expense);
            //await _context.SaveChangesAsync();

            //var expenseDtoResult = new ExpenseCreate
            //{
            //    Amount = expense.Amount,
            //    Name = expense.Name,
            //    CategoryId = expense.CategoryId,
            //    DateSpent = expense.DateSpent,
            //};
            //return CreatedAtAction("GetExpense", new { id = expense.ExpenseId }, expenseDtoResult);
            return Ok();
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var expenseToDelete = _expenseRepository.GetExpenseById(id, userId);
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
