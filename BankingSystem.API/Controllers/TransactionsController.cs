using BankingSystem.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Customer")]
    public class TransactionsController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public TransactionsController(BankingDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Get Transactions by Account ID (Admin use)
        // GET: /api/Transactions/account/1
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return Ok(transactions);
        }

        // ✅ 2. Get Transactions by Account Number (Customer/ Admin)
        // GET: /api/Transactions/number/AC2025071901
        [HttpGet("number/{accountNumber}")]
        public async Task<IActionResult> GetByAccountNumber(string accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound("Account not found");

            var transactions = account.Transactions
                .OrderByDescending(t => t.Date)
                .Select(t => new
                {
                    t.Id,
                    t.Date,
                    t.Type,
                    t.Amount,
                    t.Description
                });

            return Ok(transactions);
        }

        // ✅ 3. Get All Transactions of Current Logged-in Customer (My History)
        // GET: /api/Transactions/my
        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get Customer ID associated with the logged-in user
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
            if (customer == null)
                return NotFound("Customer profile not found");

            // Get all accounts of this customer
            var accounts = await _context.Accounts
                .Where(a => a.CustomerId == customer.Id)
                .Include(a => a.Transactions)
                .ToListAsync();

            var transactions = accounts
                .SelectMany(a => a.Transactions.Select(t => new
                {
                    t.Id,
                    t.Date,
                    t.Type,
                    t.Amount,
                    t.Description,
                    AccountNumber = a.AccountNumber
                }))
                .OrderByDescending(t => t.Date)
                .ToList();

            return Ok(transactions);
        }
    }
}

