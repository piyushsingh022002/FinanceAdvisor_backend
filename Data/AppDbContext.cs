using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinanceAdvisorApi.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceAdvisorApi.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<FinancialGoal> FinancialGoals { get; set; } = null!;
        
        public DbSet<Budget> Budgets { get; set; } = null!;
    }
}