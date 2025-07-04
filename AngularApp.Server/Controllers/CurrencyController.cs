using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using AngularApp.Server.Data;
using System.Linq;

namespace AngularApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private const string ApiKey = "7QxhPO97vl7BdauAygpWm1RKv6079NLO";
        private const string BaseUrl = "https://api.apilayer.com/exchangerates_data";

        public CurrencyController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", ApiKey);
            _context = context;
        }

        // Получение актуальных курсов от внешнего API
        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/latest?base=USD");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // Получение курса одной валюты к другой
        [HttpGet("rate")]
        public async Task<IActionResult> GetRate([FromQuery] string from, [FromQuery] string to)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/convert?from={from}&to={to}&amount=1");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // Конвертация суммы из одной валюты в другую
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertAmount([FromQuery] decimal amount, [FromQuery] string from, [FromQuery] string to)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/convert?from={from}&to={to}&amount={amount}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // история курсов по валюте и периоду
        [HttpGet("history")]
        public IActionResult GetHistory([FromQuery] string currency)
        {
            var rates = _context.CurrencyRateHistory
                .Where(r => r.CurrencyName == currency)
                .OrderBy(r => r.RecordedAt)
                .ToList();

            return Ok(rates);
        }
    }
}
