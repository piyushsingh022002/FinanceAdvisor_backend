using Microsoft.AspNetCore.Identity;

namespace FinanceAdvisorApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } // e.g., Food, Rent
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public IdentityUser User { get; set; }
    }
}