using Microsoft.AspNetCore.Identity;

namespace BankingSystem.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields here
        public string FullName { get; set; }
    }
}

