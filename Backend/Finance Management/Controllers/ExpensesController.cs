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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BalanceService _balanceService;
        private readonly SpendingsService _currentMonthSpendingService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;

        public ExpensesController(
            UserManager<IdentityUser> userManager,
            BalanceService balanceService,
            SpendingsService currentMonthSpendingService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IExpenseRepository expenseRepository)
        {
            _userManager = userManager;
            _balanceService = balanceService;
            _currentMonthSpendingService = currentMonthSpendingService;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        [HttpGet("Category{categoryId}")]
        public async Task<ActionResult<IEnumerable<ExpenseGetDTO>>> GetExpensesByCategory(int categoryId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var expenses = _expenseRepository.GetExpensesByCategory(categoryId, userId);
                var expenseDtos = _mapper.Map<List<ExpenseGetDTO>>(expenses);
                return Ok(expenseDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving expenses by category.", Details = ex.Message });
            }
        }

        [HttpGet("balance/user")]
        public ActionResult<decimal> GetBalance()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var balance = _balanceService.CalculateTotalBalance(userId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the balance.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseGetDTO>> GetExpenseById(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var expense = _expenseRepository.GetExpenseById(id, userId);
                if (expense == null)
                {
                    return NotFound(new { Message = "Expense not found." });
                }

                var expenseDto = _mapper.Map<ExpenseGetDTO>(expense);
                return Ok(expenseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the expense.", Details = ex.Message });
            }
        }

        [HttpGet("user/")]
        public async Task<ActionResult<IEnumerable<ExpenseGetDTO>>> GetExpenseByUserId()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var expenses = _expenseRepository.GetExpenseByUserId(userId);
                var expenseDtos = _mapper.Map<List<ExpenseGetDTO>>(expenses);
                return Ok(expenseDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving expenses by user.", Details = ex.Message });
            }
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<ExpenseGetDTO>>> GetExpenseByDate(DateTime date)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var expenses = _expenseRepository.GetExpenseByDate(date, userId);
                var expenseDtos = _mapper.Map<List<ExpenseGetDTO>>(expenses);
                return Ok(expenseDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving expenses by date.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, ExpenseUpdateDTO expenseDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            return Ok();
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _expenseRepository.PutExpense(id, expenseDTO);
            //try
            //{
            //    var existingExpense = _expenseRepository.GetExpenseById(id, userId);
            //    if (existingExpense == null)
            //    {
            //        return NotFound(new { Message = "Expense not found." });
            //    }

            //    var updatedExpense = _mapper.Map(expenseDTO, existingExpense);
            //    _expenseRepository.PutExpense(id, updatedExpense);

            //    return Ok(new { Message = "Expense updated successfully.", Expense = _mapper.Map<ExpenseGetDTO>(updatedExpense) });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { Message = "An error occurred while updating the expense.", Details = ex.Message });
            //}
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseCreate>> PostExpense(ExpenseCreate expenseDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var expense = _mapper.Map<Expense>(expenseDTO);
                _expenseRepository.PostExpense(expense, userId);

                return CreatedAtAction(nameof(GetExpenseById), new { id = expense.ExpenseId }, _mapper.Map<ExpenseGetDTO>(expense));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the expense.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            try
            {
                var expenseToDelete = _expenseRepository.GetExpenseById(id, userId);
                if (expenseToDelete == null)
                {
                    return NotFound(new { Message = "Expense not found." });
                }

                var success = _expenseRepository.DeleteExpense(expenseToDelete);
                if (!success)
                {
                    return StatusCode(500, new { Message = "An error occurred while deleting the expense." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the expense.", Details = ex.Message });
            }
        }
    }
}
