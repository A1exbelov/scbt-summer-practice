using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using AngularApp.Server.Data;
using AngularApp.Server.Models;

namespace AngularApp.Server.Services
{
    public class CurrencyRatesUpdater : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiKey = "7QxhPO97vl7BdauAygpWm1RKv6079NLO";

        public CurrencyRatesUpdater(IServiceProvider services, IHttpClientFactory httpClientFactory)
        {
            _services = services;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.DefaultRequestHeaders.Add("apikey", ApiKey);

                    var response = await httpClient.GetAsync("https://api.apilayer.com/exchangerates_data/latest?base=USD");
                    var content = await response.Content.ReadAsStringAsync();

                    var data = JsonDocument.Parse(content);
                    var rates = data.RootElement.GetProperty("rates");

                    foreach (var rate in rates.EnumerateObject())
                    {
                        context.CurrencyRateHistory.Add(new CurrencyRateHistory
                        {
                            CurrencyName = rate.Name,
                            Rate = rate.Value.GetDouble(),
                            RecordedAt = DateTime.UtcNow
                        });
                    }

                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
