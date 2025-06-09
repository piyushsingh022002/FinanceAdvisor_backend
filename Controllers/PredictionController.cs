using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceAdvisorApi.Data;
using FinanceAdvisorApi.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisorApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PredictionService _predictionService;

        public PredictionController(AppDbContext context, PredictionService predictionService)
        {
            _context = context;
            _predictionService = predictionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrediction()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();
            var prediction = await _predictionService.GetSpendingPrediction(userId, transactions);
            return Ok(new { Prediction = prediction });
        }
    }
}