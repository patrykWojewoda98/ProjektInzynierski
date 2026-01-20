using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Models;
using System.Net;
using System.Text.Json;

namespace ProjektIznynierski.Infrastructure.Services
{
    public class YahooFinanceService : IYahooFinanceService
    {
        private readonly HttpClient _httpClient;

        public YahooFinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/121.0.0.0 Safari/537.36");

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        public async Task<MarketDataSnapshot?> GetMarketDataByTIcker(string ticker, CancellationToken ct)
        {
            var url =
                $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}.WA?interval=1d&range=1d";

            using var response = await _httpClient.GetAsync(url, ct);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                return null;

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync(ct);

            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement
                .GetProperty("chart")
                .GetProperty("result")[0];

            var timestamp = root.GetProperty("timestamp")[0];
            var quote = root.GetProperty("indicators")
                            .GetProperty("quote")[0];

            var date = DateTimeOffset
                .FromUnixTimeSeconds(timestamp.GetInt64())
                .LocalDateTime;

            decimal? open = GetDecimal(quote, "open");
            decimal? high = GetDecimal(quote, "high");
            decimal? low = GetDecimal(quote, "low");
            decimal? close = GetDecimal(quote, "close");
            long? volume = GetLong(quote, "volume");

            decimal? dailyChange = null;
            decimal? dailyChangePercent = null;

            if (open.HasValue && close.HasValue && open.Value != 0)
            {
                dailyChange = close.Value - open.Value;
                dailyChangePercent = Math.Round((dailyChange.Value / open.Value) * 100, 2);
            }

            return new MarketDataSnapshot
            {
                Date = date,
                OpenPrice = open,
                HighPrice = high,
                LowPrice = low,
                ClosePrice = close,
                Volume = volume,
                DailyChange = dailyChange,
                DailyChangePercent = dailyChangePercent
            };
        }

        private static decimal? GetDecimal(JsonElement parent, string name)
        {
            var arr = parent.GetProperty(name);
            return arr[0].ValueKind == JsonValueKind.Null
                ? null
                : arr[0].GetDecimal();
        }

        private static long? GetLong(JsonElement parent, string name)
        {
            var arr = parent.GetProperty(name);
            return arr[0].ValueKind == JsonValueKind.Null
                ? null
                : arr[0].GetInt64();
        }
    }
}
