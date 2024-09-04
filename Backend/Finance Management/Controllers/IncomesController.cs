using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly DataContext _context;

        public IncomesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Incomes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes()
        {
            try
            {
                var userId = User.Identity.Name;
                var incomes = await _context.incomes
                    .Where(i => i.UserId == userId)
                    .ToListAsync();

                if (incomes == null || !incomes.Any())
                {
                    return NotFound(new { Message = "No incomes found for the current user." });
                }

                return Ok(incomes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving incomes.", Details = ex.Message });
            }
        }

        // GET: api/Incomes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(int id)
        {
            try
            {
                var userId = User.Identity.Name;
                var income = await _context.incomes
                    .Where(i => i.UserId == userId && i.Id == id)
                    .FirstOrDefaultAsync();

                if (income == null)
                {
                    return NotFound(new { Message = "Income not found." });
                }

                return Ok(income);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the income.", Details = ex.Message });
            }
        }

        // PUT: api/Incomes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, Income income)
        {
            if (id != income.Id)
            {
                return BadRequest(new { Message = "Income ID mismatch." });
            }

            _context.Entry(income).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!IncomeExists(id))
                {
                    return NotFound(new { Message = "Income not found." });
                }
                else
                {
                    return StatusCode(500, new { Message = "An error occurred while updating the income.", Details = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // POST: api/Incomes
        [HttpPost]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            try
            {
                income.UserId = User.Identity.Name; // Set the UserId to the current logged-in user

                _context.incomes.Add(income);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the income.", Details = ex.Message });
            }
        }

        // DELETE: api/Incomes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            try
            {
                var userId = User.Identity.Name;
                var income = await _context.incomes
                    .Where(i => i.UserId == userId && i.Id == id)
                    .FirstOrDefaultAsync();

                if (income == null)
                {
                    return NotFound(new { Message = "Income not found." });
                }

                _context.incomes.Remove(income);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the income.", Details = ex.Message });
            }
        }

        private bool IncomeExists(int id)
        {
            return _context.incomes.Any(e => e.Id == id);
        }
    }
}
