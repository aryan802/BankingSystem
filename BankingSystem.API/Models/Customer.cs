using System.ComponentModel.DataAnnotations;

namespace BankingSystem.API.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }  // ✅ Required for login/view

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}

