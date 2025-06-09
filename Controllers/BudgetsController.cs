using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceAdvisorApi.Data;
using FinanceAdvisorApi.Models;
using System.Security.Claims;

namespace FinanceAdvisorApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetDto dto)
        {
            var budget = new Budget
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                MonthlyLimit = dto.MonthlyLimit
            };
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return Ok(budget);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId);
            return Ok(budget);
        }
    }

    public class BudgetDto
    {
        public decimal MonthlyLimit { get; set; }
    }
}