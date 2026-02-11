using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using ProjektInzynierski.Domain.Models;
using ProjektIznynierski.Domain.Abstractions;
using System.Diagnostics;
using System.Globalization;

namespace ProjektIznynierski.Application.Services.Sources
{
    public class StrefaInwestorowClientService : IStrefaInwestorowClientService
    {
        public async Task<List<FinancialReportSnapshot>> GetFinancialReportsAsync(string isin, CancellationToken ct)
        {
            var url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/wyniki-finansowe";
            var htmlDoc = new HtmlWeb().Load(url);

            var headerCells = htmlDoc.QuerySelectorAll("table thead th").Skip(1).ToList();

            var snapshots = headerCells
                .Select(h => new FinancialReportSnapshot
                {
                    Period = h.InnerText.Trim(),
                })
                .ToList();

            var rowMap = new Dictionary<string, Action<FinancialReportSnapshot, decimal?>>
            {
                ["Przychody ze sprzedaży"] = (s, v) => s.Revenue = v,
                ["Zysk netto"] = (s, v) => s.NetIncome = v,
                ["Aktywa ogółem"] = (s, v) => s.Assets = v,
                ["Zobowiązania ogółem"] = (s, v) => s.Liabilities = v,
                ["A. Przepływy pieniężne z działalności operacyjnej"] = (s, v) => s.OperatingCashFlow = v,
                ["Przepływy pieniężne razem"] = (s, v) => s.FreeCashFlow = v
            };


            var rows = htmlDoc.QuerySelectorAll("table tbody tr");

            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToList();
                if (cells.Count < 2)
                    continue;

                var rowName = cells[0].InnerText.Trim();

                if (!rowMap.TryGetValue(rowName, out var setter))
                    continue;

                for (int i = 1; i < cells.Count && i <= snapshots.Count; i++)
                {
                    var value = ParseToDecimal(cells[i].InnerText);
                    if (value.HasValue)
                        value *= 1000;   
                    setter(snapshots[i - 1], value);
                }
            }

            url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/akcjonariat";
            htmlDoc = new HtmlWeb().Load(url);

            var totalSharesNode = htmlDoc
                .QuerySelectorAll(".shareholders-extra-data dt")
                .FirstOrDefault(dt => dt.InnerText.Trim() == "Liczba wszystkich akcji:");

            HtmlNode? valueNode = totalSharesNode;
            while (valueNode != null && valueNode.Name != "dd")
                valueNode = valueNode.NextSibling;

            var totalShares = ParseToLong(valueNode?.InnerText);


            if (totalShares.HasValue && totalShares.Value > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    if (snapshot.NetIncome.HasValue)
                    {
                        snapshot.EPS = snapshot.NetIncome.Value / totalShares.Value;
                    }
                }
            }


            return snapshots;
        }

        private static decimal? ParseToDecimal(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var cleaned = text
                .Replace("&nbsp;", "")
                .Replace(" ", "")
                .Replace(",", ".")
                .Trim();

            return decimal.TryParse(
                cleaned,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var value)
                ? value
                : null;
        }

        private static long? ParseToLong(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var cleaned = text
                .Replace("&nbsp;", "")
                .Replace(" ", "")
                .Trim();

            return long.TryParse(
                cleaned,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var value)
                ? value 
                : null;
        }

       

        public async Task<FinancialIndicatorsSnapshot> GetFinancialIndicatorsAsync(string isin,CancellationToken ct)
        {
            var url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/wskazniki-finansowe";
            var htmlDoc = new HtmlWeb().Load(url);

            var rows = htmlDoc
                .QuerySelectorAll("table.table-indicators tbody tr");

            var indicators = new FinancialIndicatorsSnapshot();

            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td").ToList();
                if (cells.Count != 2)
                    continue;

                var name = cells[0].InnerText.Trim();
                var valueText = cells[1].InnerText.Trim();

                var value = ParseIndicatorValue(valueText);

                switch (name)
                {
                    case "C/Z":
                        indicators.PE = value;
                        break;

                    case "C/WK":
                        indicators.PB = value;
                        break;

                    case "ROE":
                        indicators.ROE = value;
                        break;

                    case "C/ZO":
                        indicators.DebtToEquity = value;
                        break;
                }
            }
            var latestDividendYield = await GetLatestDividendYieldAsync(isin);

            if (latestDividendYield.HasValue)
            {
                indicators.DividendYield = latestDividendYield;
            }
            return indicators;
        }

        private async Task<decimal?> GetLatestDividendYieldAsync(string isin)
        {
            var url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/dywidendy";
            var htmlDoc = new HtmlWeb().Load(url);

            var firstRow = htmlDoc
                .QuerySelector("table.table-dividends-mobile tbody tr");

            if (firstRow == null)
                return null;

            var dividendDiv = firstRow
                .QuerySelectorAll("div.mb-1")
                .FirstOrDefault(d =>
                    d.InnerText.Contains("Stopa dywidendy:", StringComparison.OrdinalIgnoreCase));

            if (dividendDiv == null)
                return null;

            var text = HtmlEntity.DeEntitize(dividendDiv.InnerText)
                .Replace("Stopa dywidendy:", "")
                .Trim();

            return ParseIndicatorValue(text);
        }

        private static decimal? ParseIndicatorValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text == "-")
                return null;

            var cleaned = text
                .Replace("%", "")
                .Replace("&nbsp;", "")
                .Replace(" ", "")
                .Replace(",", ".")
                .Trim();

            return decimal.TryParse(
                cleaned,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var value)
                ? value
                : null;
        }
    }
}