using BankingSystem.API.Data;
using BankingSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly BankingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(BankingDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ 1. Get all transactions for a specific account
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccount(int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return Ok(transactions);
        }

        // ✅ 2. Get all transactions for currently logged-in user
        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the Customer entity for this ApplicationUser
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null) return Unauthorized("Customer not found");

            // Get all account IDs owned by this customer
            var accountIds = await _context.Accounts
                .Where(a => a.CustomerId == customer.Id)
                .Select(a => a.Id)
                .ToListAsync();

            // Get all transactions from those accounts
            var transactions = await _context.Transactions
                .Where(t => accountIds.Contains(t.AccountId))
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return Ok(transactions);
        }
    }
}



