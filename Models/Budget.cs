using Microsoft.AspNetCore.Identity;

namespace FinanceAdvisorApi.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string ?UserId { get; set; }
        public decimal MonthlyLimit { get; set; }
        public IdentityUser ?User { get; set; }
    }
}