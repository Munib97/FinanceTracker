using Finance_Management.Data;
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
    public class CombinedSpendingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SpendingsService _spendingsService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CombinedSpendingsController(DataContext context, UserManager<IdentityUser> userManager, SpendingsService spendingsService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _spendingsService = spendingsService;
            _contextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetCombinedExpensesList()
        {
            try
            {
                var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
                if (userId == null)
                {
                    return Unauthorized(new { Message = "User not found." });
                }

                var expenses = await _context.expenses
                    .Where(e => e.UserId == userId)
                    .Select(e => new CombinedSpendingsDto
                    {
                        Id = e.ExpenseId,
                        Amount = e.Amount,
                        UserId = e.UserId,
                        Date = e.DateSpent,
                        Name = e.Name,
                        CategoryName = e.Category.Name,
                        Type = "Expense"
                    }).ToListAsync();

                var subscriptions = await _context.subscriptions
                    .Where(s => s.UserId == userId)
                    .Select(s => new CombinedSpendingsDto
                    {
                        Id = s.SubscriptionId,
                        Amount = s.Amount,
                        UserId = s.UserId,
                        Date = s.DueDate,
                        Name = s.Name,
                        CategoryName = s.Category.Name,
                        Type = "Subscription"
                    }).ToListAsync();

                var combinedData = expenses.Concat(subscriptions).OrderBy(item => item.Date);
                return Ok(combinedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching combined expenses.", Details = ex.Message });
            }
        }

        [HttpGet("currentMonthSpending/user/")]
        public async Task<IActionResult> GetCurrentMonthExpenses()
        {
            try
            {
                var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);
                if (userId == null)
                {
                    return Unauthorized(new { Message = "User not found." });
                }

                var currentMonthSpending = await _spendingsService.CalculateCurrentMonthSpending(userId);
                return Ok(currentMonthSpending);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while calculating current month's spending.", Details = ex.Message });
            }
        }
    }
}
