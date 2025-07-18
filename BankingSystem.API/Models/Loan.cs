namespace BankingSystem.API.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public double InterestRate { get; set; }
        public int TermInMonths { get; set; }
        public DateTime StartDate { get; set; }

        public string Status { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser Applicant { get; set; }
    }
}
