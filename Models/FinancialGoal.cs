namespace FinanceAdvisorApi.Models
{
    public class FinancialGoal
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; } // e.g., "Vacation Fund"
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }
        public IdentityUser User { get; set; }
    }
}