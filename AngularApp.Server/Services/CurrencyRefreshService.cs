using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class CurrencyRefreshService : BackgroundService
{
    private readonly CurrencyService _currencyService;
    private readonly ILogger<CurrencyRefreshService> _logger;

    public CurrencyRefreshService(CurrencyService currencyService, ILogger<CurrencyRefreshService> logger)
    {
        _currencyService = currencyService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Обновляю курсы валют...");
            await _currencyService.GetRatesAsync();
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}