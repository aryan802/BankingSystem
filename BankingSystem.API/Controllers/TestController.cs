using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // Anyone can access this
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("This is a public endpoint.");
        }

        // Only authenticated users can access this
        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var userName = User.Identity?.Name;
            return Ok($"This is a protected endpoint. Hello, {userName}!");
        }
    }
}

