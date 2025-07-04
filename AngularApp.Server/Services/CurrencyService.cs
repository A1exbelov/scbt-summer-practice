using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class CurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "CurrencyRates";
    private const string ApiUrl = "https://api.apilayer.com/exchangerates_data/latest?base=USD";
    private const string ApiKey = "7QxhPO97vl7BdauAygpWm1RKv6079NLO";

    public CurrencyService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<CurrencyRatesResponse?> GetRatesAsync()
    {

        var url = "https://api.apilayer.com/exchangerates_data/latest?base=USD";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("apikey", ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var rates = JsonSerializer.Deserialize<CurrencyRatesResponse>(json);

        return rates;
    }

}
public class CurrencyRatesResponse
{
    public string Base { get; set; }
    public DateTime Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}