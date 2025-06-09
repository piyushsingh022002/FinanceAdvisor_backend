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
    public class GoalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FinancialGoalDto dto)
        {
            var goal = new FinancialGoal
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = dto.Name,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = dto.CurrentAmount,
                Deadline = dto.Deadline
            };
            _context.FinancialGoals.Add(goal);
            await _context.SaveChangesAsync();
            return Ok(goal);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goals = await _context.FinancialGoals
                .Where(g => g.UserId == userId)
                .ToListAsync();
            return Ok(goals);
        }
    }

    public class FinancialGoalDto
    {
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }
    }
}