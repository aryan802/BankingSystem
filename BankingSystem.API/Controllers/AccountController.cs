using BankingSystem.API.Data;
using BankingSystem.API.DTOs;
using BankingSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AccountController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public AccountController(BankingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _context.Accounts
                .Select(a => new AccountDTO
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance,
                    AccountType = a.AccountType,
                    CustomerId = a.CustomerId
                })
                .ToListAsync();

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _context.Accounts.FindAsync(id);
            if (a == null) return NotFound();

            return Ok(new AccountDTO
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                Balance = a.Balance,
                AccountType = a.AccountType,
                CustomerId = a.CustomerId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDTO model)
        {
            var customer = await _context.Customers.FindAsync(model.CustomerId);
            if (customer == null) return NotFound("Customer not found");

            var account = new Account
            {
                AccountType = model.AccountType,
                CustomerId = model.CustomerId,
                AccountNumber = GenerateAccountNumber()
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account created", account.AccountNumber });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAccountDTO model)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            account.AccountType = model.AccountType;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account deleted" });
        }

        private string GenerateAccountNumber()
        {
            return "AC" + DateTime.UtcNow.Ticks.ToString().Substring(6, 10);
        }
    }
}
