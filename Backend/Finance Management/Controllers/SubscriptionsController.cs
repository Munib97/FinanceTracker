using AutoMapper;
using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public SubscriptionsController(DataContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptionsByUserId()
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var subscriptions = await _context.subscriptions
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.StartDate)
                    .ToListAsync();

                if (subscriptions == null || !subscriptions.Any())
                {
                    return NotFound(new { Message = "No subscriptions found for the current user." });
                }

                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving subscriptions.", Details = ex.Message });
            }
        }

        [HttpGet("{categoryName}")]
        public async Task<ActionResult<Subscription>> GetSubscriptionsByCategory(string categoryName)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var subs = await _context.subscriptions.Where(s => s.Category.Name == categoryName && s.UserId == userId).ToListAsync();
            return Ok(subs);
        }

        [HttpGet("GetSubscriptionByDate")]
        public async Task<ActionResult<Subscription>> GetSubscriptionsByDate(DateTime startingDate, DateTime endingDate)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var subs = await _context.subscriptions.Where(s => s.DueDate > startingDate && s.DueDate < endingDate).ToListAsync();
            return Ok(subs);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, SubscriptionUpdateDTO subscriptionsDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            try
            {
                var existingSubscription = await _context.subscriptions.FindAsync(id);
                if (existingSubscription == null || existingSubscription.UserId != userId)
                {
                    return NotFound(new { Message = "Subscription not found." });
                }

                if (subscriptionsDTO.Frequency.HasValue)
                {
                    existingSubscription.Frequency = subscriptionsDTO.Frequency.Value;
                }
                if (subscriptionsDTO.StartDate.HasValue)
                {
                    existingSubscription.StartDate = subscriptionsDTO.StartDate.Value;
                }
                if (subscriptionsDTO.EndDate.HasValue)
                {
                    existingSubscription.EndDate = subscriptionsDTO.EndDate.Value;
                }
                if (subscriptionsDTO.Amount.HasValue)
                {
                    existingSubscription.Amount = subscriptionsDTO.Amount.Value;
                }
                if (!string.IsNullOrWhiteSpace(subscriptionsDTO.Name))
                {
                    existingSubscription.Name = subscriptionsDTO.Name;
                }

                _context.Entry(existingSubscription).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
                {
                    return NotFound(new { Message = "Subscription not found." });
                }
                else
                {
                    return StatusCode(500, new { Message = "An error occurred while updating the subscription." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionCreateDTO>> CreateSubscription([FromBody] SubscriptionCreateDTO subscriptionsDTO)
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (userId == null)
                {
                    return BadRequest(new { Message = "User not found." });
                }

                var subscription = _mapper.Map<Subscription>(subscriptionsDTO);
                subscription.UserId = userId;

                await _context.subscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();

                var subscriptionDTOResult = _mapper.Map<SubscriptionCreateDTO>(subscription);

                return CreatedAtAction(nameof(CreateSubscription), new { id = subscription.SubscriptionId }, subscriptionDTOResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the subscription.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var subscription = await _context.subscriptions
                    .Where(s => s.SubscriptionId == id && s.UserId == userId)
                    .FirstOrDefaultAsync();

                if (subscription == null)
                {
                    return NotFound(new { Message = "Subscription not found." });
                }

                _context.subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the subscription.", Details = ex.Message });
            }
        }

        private bool SubscriptionExists(int id)
        {
            return _context.subscriptions.Any(e => e.SubscriptionId == id);
        }
    }
}
