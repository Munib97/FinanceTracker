using Finance_Management.Data;
using Finance_Management.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, JwtTokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid login data.", Errors = ModelState.Values });
            }

            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user == null)
                    {
                        return Unauthorized(new { Message = "User not found." });
                    }

                    await _signInManager.SignInAsync(user, model.RememberMe);

                    var token = _tokenService.GenerateToken(user.Id);
                    return Ok(new { Message = "Login successful.", Token = token });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid login attempt." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login.", Details = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid registration data.", Errors = ModelState.Values });
            }

            try
            {
                var newUser = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Registration successful." });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(new { Message = "Registration failed.", Errors = ModelState.Values });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration.", Details = ex.Message });
            }
        }

        [HttpPost("validateToken")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "Token is required." });
            }

            try
            {
                var result = _tokenService.ValidateToken(token);
                if (result != null)
                {
                    return Ok(new { Message = "Token is valid." });
                }
                else
                {
                    return BadRequest(new { Message = "Token is invalid." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during token validation.", Details = ex.Message });
            }
        }
    }
}
