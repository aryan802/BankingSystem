namespace BankingSystem.API.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public int CustomerId { get; set; }
    }

    public class CreateAccountDTO
    {
        public string AccountType { get; set; }
        public int CustomerId { get; set; }
    }

    public class UpdateAccountDTO
    {
        public string AccountType { get; set; }
    }
}
