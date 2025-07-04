using System.Text.Json;
using AngularApp.Server.Data;
using AngularApp.Server.Models;

namespace AngularApp.Server.Jobs
{
    public class CurrencyRateJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiKey = "7QxhPO97vl7BdauAygpWm1RKv6079NLO";

        public CurrencyRateJob(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task FetchAndSaveRates()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("apikey", ApiKey);

            var response = await httpClient.GetAsync("https://api.apilayer.com/exchangerates_data/latest?base=USD");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            var rates = jsonDoc.RootElement.GetProperty("rates");

            foreach (var rate in rates.EnumerateObject())
            {
                _context.CurrencyRateHistory.Add(new CurrencyRateHistory
                {
                    CurrencyName = rate.Name,
                    Rate = rate.Value.GetDouble(),
                    RecordedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}