using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace FinanceAdvisorApi.Services
{
    public class PredictionService
    {
        private readonly HttpClient _httpClient;

        public PredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetSpendingPrediction(string userId, List<Models.Transaction> transactions)
        {
            // var requestData = new { UserId = userId, Transactions = transactions };
            // var content = new StringContent(
            //     JsonSerializer.Serialize(requestData),
            //     Encoding.UTF8,
            //     "application/json");

            // var response = await _httpClient.PostAsync("http://localhost:5000/predict", content);
            // response.EnsureSuccessStatusCode();

            // var result = await response.Content.ReadFromJsonAsync<PredictionResult>();
            // return result.Prediction;

            var requestData = new
            {
                Transactions = transactions.Select(t => new
                {
                    t.Amount,
                    Date = t.Date.ToString("O"), // ISO 8601 format
                    t.Category,
                    t.Description
                }).ToList()
            };
            var content = new StringContent(
                JsonSerializer.Serialize(requestData),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5001/predict", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PredictionResult>();
            return result.Prediction;
        }
        }
    }

    public class PredictionResult
    {
        public decimal Prediction { get; set; }
    }
