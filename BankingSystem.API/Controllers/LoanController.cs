using BankingSystem.API.Data;
using BankingSystem.API.DTOs;
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
    public class LoanController : ControllerBase
    {
        private readonly BankingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanController(BankingDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/Loan (Customer applies for loan)
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ApplyLoan([FromBody] LoanDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return Unauthorized();

            var loan = new Loan
            {
                Amount = dto.Amount,
                InterestRate = dto.InterestRate,
                TermInMonths = dto.TermInMonths,
                StartDate = DateTime.UtcNow,
                Status = "Pending",
                ApplicationUserId = user.Id
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Loan application submitted.", loanId = loan.Id });
        }

        // GET: api/Loan (Admin gets all loan applications)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _context.Loans
                .Include(l => l.Applicant)
                .ToListAsync();

            return Ok(loans);
        }

        // PUT: api/Loan/approve/5
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            var loan = await _context.Loans.Include(l => l.Applicant).FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null) return NotFound();

            if (loan.Status != "Pending") return BadRequest("Loan already processed");

            loan.Status = "Approved";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Loan approved" });
        }

        // PUT: api/Loan/reject/5
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            if (loan.Status != "Pending") return BadRequest("Loan already processed");

            loan.Status = "Rejected";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Loan rejected" });
        }
    }
}

