using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;    
        public SubscriptionsController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Subscriptions
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptionByUserId(IHttpContextAccessor httpContextAccessor)
        {
            var userId = _userManager.GetUserId(httpContextAccessor.HttpContext.User);
            return await _context.subscriptions.Where(i => i.UserId == userId).OrderByDescending(i => i.StartDate).ToListAsync();
        }


        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptions(int id, Subscription subscriptions)
        {
            if (id != subscriptions.SubscriptionId)
            {
                return BadRequest();
            }

            _context.Entry(subscriptions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionsExists(id))
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

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscriptionDto>> PostSubscriptions([FromBody] SubscriptionDto subscriptionsDto)
        {
            var UserId = _userManager.GetUserId(HttpContext.User);
            if (UserId == null)
            {
                return BadRequest("User Not Found");
            }
                var subscription = new Subscription
                {
                    Name = subscriptionsDto.Name,
                    Frequency = subscriptionsDto.Frequency,
                    StartDate = subscriptionsDto.StartDate,
                    EndDate = subscriptionsDto.EndDate,
                    Amount = subscriptionsDto.Amount,

                    UserId = UserId,
                    CategoryId = subscriptionsDto.CategoryId,

                };
                _context.subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                var subscriptionDtoResult = new SubscriptionDto
                {
                    SubscriptionId = subscription.SubscriptionId,
                    Amount = subscription.Amount,
                    Name = subscription.Name,
                    Frequency = subscription.Frequency,
                    StartDate = subscription.StartDate,
                    EndDate= subscription.EndDate,
                };


                return CreatedAtAction("PostSubscriptions", new { id = subscription.SubscriptionId }, subscriptionDtoResult);
            
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptions(int id)
        {
            var subscriptions = await _context.subscriptions.FindAsync(id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            _context.subscriptions.Remove(subscriptions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionsExists(int id)
        {
            return _context.subscriptions.Any(e => e.SubscriptionId == id);
        }
    }
}