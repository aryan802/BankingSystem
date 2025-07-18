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
    [Authorize(Roles = "Admin,Customer")]
    public class TransferController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public TransferController(BankingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferDTO dto)
        {
            if (dto.Amount <= 0)
                return BadRequest("Invalid amount");

            var fromAccount = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == dto.FromAccountNumber);

            var toAccount = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == dto.ToAccountNumber);

            if (fromAccount == null || toAccount == null)
                return NotFound("Account not found");

            if (fromAccount.Balance < dto.Amount)
                return BadRequest("Insufficient balance");

            // Debit from sender
            fromAccount.Balance -= dto.Amount;
            fromAccount.Transactions.Add(new Transaction
            {
                Type = "Debit",
                Amount = dto.Amount,
                Date = DateTime.UtcNow,
                Description = $"Transfer to {toAccount.AccountNumber}: {dto.Description}",
                AccountId = fromAccount.Id
            });

            // Credit to receiver
            toAccount.Balance += dto.Amount;
            toAccount.Transactions.Add(new Transaction
            {
                Type = "Credit",
                Amount = dto.Amount,
                Date = DateTime.UtcNow,
                Description = $"Transfer from {fromAccount.AccountNumber}: {dto.Description}",
                AccountId = toAccount.Id
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Transfer successful",
                from = fromAccount.AccountNumber,
                to = toAccount.AccountNumber,
                amount = dto.Amount
            });
        }
    }
}
