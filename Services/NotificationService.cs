using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinanceAdvisorApi.Data;
using FinanceAdvisorApi.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceAdvisorApi.Services
{
    public class NotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                // Check for overspending
                var budgets = await context.Budgets.ToListAsync(stoppingToken);
                foreach (var budget in budgets)
                {
                    var monthlySpending = await context.Transactions
                        .Where(t => t.UserId == budget.UserId &&
                                    t.Date.Month == DateTime.Now.Month &&
                                    t.Date.Year == DateTime.Now.Year)
                        .SumAsync(t => t.Amount, stoppingToken);

                    if (monthlySpending > budget.MonthlyLimit)
                    {
                        var user = await context.Users.FindAsync(new object[] { budget.UserId }, stoppingToken);
                        var message = new EmailMessage
                        {
                            To = user.Email,
                            Subject = "Overspending Alert",
                            Body = $"You've spent ${monthlySpending}, exceeding your ${budget.MonthlyLimit} monthly budget!"
                        };
                        await emailService.SendAsync(message);
                    }
                }

                // Check for bill due dates (example: assume transactions with "Bill" category)
                var dueBills = await context.Transactions
                    .Where(t => t.Category == "Bill" &&
                                t.Date.Date == DateTime.Now.AddDays(1).Date)
                    .ToListAsync(stoppingToken);
                foreach (var bill in dueBills)
                {
                    var user = await context.Users.FindAsync(new object[] { bill.UserId }, stoppingToken);
                    var message = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Bill Due Reminder",
                        Body = $"Your bill of ${bill.Amount} ({bill.Description}) is due tomorrow!"
                    };
                    await emailService.SendAsync(message);
                }

                // Run every hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}