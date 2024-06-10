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
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptionByUserId()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return await _context.subscriptions.Where(i => i.UserId == userId).OrderByDescending(i => i.StartDate).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptions(int id, SubscriptionUpdate subscriptionsDTO)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var existingSubscription = await _context.subscriptions.FindAsync(id);
            if (existingSubscription == null)
            {
                return NotFound();
            }

            if (subscriptionsDTO.Frequency != null)
            {
                existingSubscription.Frequency = (SubscriptionFrequency)subscriptionsDTO.Frequency;
            }
            if (subscriptionsDTO.StartDate != null)
            {
                existingSubscription.StartDate = (DateTime)subscriptionsDTO.StartDate;
            }
            if (subscriptionsDTO.EndDate != null)
            {
                existingSubscription.EndDate = (DateTime)subscriptionsDTO.EndDate;
            }
            if (subscriptionsDTO.Amount != null)
            {
                existingSubscription.Amount = (decimal)subscriptionsDTO.Amount;
            }
            if (subscriptionsDTO.Name != null)
            {
                existingSubscription.Name = subscriptionsDTO.Name;
            }
            _context.Entry(existingSubscription).State = EntityState.Modified;

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

        [HttpPost]
        public async Task<ActionResult<SubscriptionCreate>> PostSubscriptions([FromBody] SubscriptionCreate subscriptionsDTO)
        {
            var UserId = _userManager.GetUserId(HttpContext.User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserId == null)
            {
                return BadRequest("User not found");
            }

            var subscription = _mapper.Map<Subscription>(subscriptionsDTO);
            subscription.UserId = UserId;
            _context.subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            var subscriptionDTOResult = _mapper.Map<SubscriptionCreate>(subscription);


            return CreatedAtAction("PostSubscriptions", new { id = subscription.SubscriptionId }, subscriptionDTOResult);

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