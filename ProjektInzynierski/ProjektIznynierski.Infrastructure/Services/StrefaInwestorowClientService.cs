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
            //LOAD FINANCIAL TABLE
            var url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/wyniki-finansowe";
            var htmlDoc = new HtmlWeb().Load(url);

            var headerCells = htmlDoc.QuerySelectorAll("table thead th").Skip(1).ToList();

            var snapshots = headerCells
                .Select(h => new FinancialReportSnapshot
                {
                    Period = h.InnerText.Trim(),
                })
                .ToList();

            //ROW → PROPERTY MAP
            var rowMap = new Dictionary<string, Action<FinancialReportSnapshot, decimal?>>
            {
                ["Przychody ze sprzedaży"] = (s, v) => s.Revenue = v,
                ["Zysk netto"] = (s, v) => s.NetIncome = v,
                ["Aktywa ogółem"] = (s, v) => s.Assets = v,
                ["Zobowiązania ogółem"] = (s, v) => s.Liabilities = v,
                ["A. Przepływy pieniężne z działalności operacyjnej"] = (s, v) => s.OperatingCashFlow = v,
                ["Przepływy pieniężne razem"] = (s, v) => s.FreeCashFlow = v
            };


            //PARSE TABLE BODY
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
                    setter(snapshots[i - 1], value);
                }
            }

            //LOAD TOTAL SHARES
            url = $"https://strefainwestorow.pl/notowania/spolki/{isin}/akcjonariat";
            htmlDoc = new HtmlWeb().Load(url);

            var totalSharesNode = htmlDoc
                .QuerySelectorAll(".shareholders-extra-data dt")
                .FirstOrDefault(dt => dt.InnerText.Trim() == "Liczba wszystkich akcji:");

            HtmlNode? valueNode = totalSharesNode;
            while (valueNode != null && valueNode.Name != "dd")
                valueNode = valueNode.NextSibling;

            var totalShares = ParseToLong(valueNode?.InnerText);

            Debug.WriteLine($"TOTAL SHARES: {totalShares}");

            //CALCULATE EPS

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

            ShowAllSnapshotsData(snapshots);

            return snapshots;
        }

        //HELPERS
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
                ? value *1000
                : null;
        }

        private static void ShowAllSnapshotsData(IEnumerable<FinancialReportSnapshot> snapshots)
        {
            Debug.WriteLine("========== FINANCIAL REPORT SNAPSHOTS ==========");

            foreach (var s in snapshots)
            {
                Debug.WriteLine("-----------------------------------------------");
                Debug.WriteLine($"Period: {s.Period}");

                Debug.WriteLine($"Revenue: {s.Revenue}");
                Debug.WriteLine($"NetIncome: {s.NetIncome}");
                Debug.WriteLine($"EPS: {s.EPS}");

                Debug.WriteLine($"Assets: {s.Assets}");
                Debug.WriteLine($"Liabilities: {s.Liabilities}");

                Debug.WriteLine($"OperatingCashFlow: {s.OperatingCashFlow}");
                Debug.WriteLine($"FreeCashFlow: {s.FreeCashFlow}");
            }

            Debug.WriteLine("===============================================");
        }
    }
}