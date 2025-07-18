using System;

namespace BankingSystem.API.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; } = 0;

        public string AccountType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}


