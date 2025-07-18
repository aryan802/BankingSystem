namespace BankingSystem.API.DTOs
{
    public class LoanDTO
    {
        public decimal Amount { get; set; }
        public double InterestRate { get; set; }
        public int TermInMonths { get; set; }
    }
}
