using BankingSystem.API.Data;
using BankingSystem.API.Models;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionExportController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public TransactionExportController(BankingDbContext context)
        {
            _context = context;
        }

        // PDF Export
        [HttpGet("export/pdf")]
        [Authorize]
        public async Task<IActionResult> ExportPdf([FromQuery] string accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound("Account not found");

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph($"Transactions for Account: {account.AccountNumber}"));
            doc.Add(new Paragraph(" "));

            foreach (var tx in account.Transactions)
            {
                doc.Add(new Paragraph($"{tx.Date.ToShortDateString()} - {tx.Type} - {tx.Amount} - {tx.Description}"));
            }

            doc.Close();
            return File(ms.ToArray(), "application/pdf", $"Transactions_{account.AccountNumber}.pdf");
        }

        // Excel Export
        [HttpGet("export/excel")]
        [Authorize]
        public async Task<IActionResult> ExportExcel([FromQuery] string accountNumber)
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return NotFound("Account not found");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Transactions");

            worksheet.Cell(1, 1).Value = "Date";
            worksheet.Cell(1, 2).Value = "Type";
            worksheet.Cell(1, 3).Value = "Amount";
            worksheet.Cell(1, 4).Value = "Description";

            var row = 2;
            foreach (var tx in account.Transactions)
            {
                worksheet.Cell(row, 1).Value = tx.Date;
                worksheet.Cell(row, 2).Value = tx.Type;
                worksheet.Cell(row, 3).Value = tx.Amount;
                worksheet.Cell(row, 4).Value = tx.Description;
                row++;
            }

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Transactions_{account.AccountNumber}.xlsx");
        }
    }
}
