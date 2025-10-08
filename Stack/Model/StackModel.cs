using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StackApp.Model
{
    public class Stock
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public override string ToString() => $"{Date:yyyy-MM-dd} -> {Price}";
    }

    public class StackModel
    {
        private readonly Stack<Stock> stockStack = new();
        private readonly Stack<Stock> redoStack = new();

        public List<Stock> LoadStocks(string filePath, int maxRows = int.MaxValue)
        {
            var result = new List<Stock>();
            if (!File.Exists(filePath)) return result;

            using var sr = new StreamReader(filePath);
            string? headerLine = sr.ReadLine();
            if (headerLine == null) return result;

            var headers = SplitCsvLine(headerLine).Select(h => h.Trim().ToLowerInvariant()).ToArray();
            int dateIdx = Array.FindIndex(headers, h => h.Contains("date"));
            int priceIdx = -1;
            string[] priceCandidates = { "adj close", "adj_close", "adjclose", "close", "closeprice", "close price" };
            foreach (var cand in priceCandidates)
            {
                priceIdx = Array.FindIndex(headers, h => h.Contains(cand));
                if (priceIdx >= 0) break;
            }
            if (priceIdx < 0) priceIdx = headers.Length - 1;

            int count = 0;
            while (!sr.EndOfStream && count < maxRows)
            {
                var line = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = SplitCsvLine(line);
                if (parts.Length <= Math.Max(dateIdx, priceIdx)) continue;

                string dateStr = dateIdx >= 0 ? parts[dateIdx].Trim() : parts[0].Trim();
                string priceStr = parts[priceIdx].Trim();

                if (!DateTime.TryParse(dateStr, out var date)) continue;

                priceStr = priceStr.Replace(",", "");
                if (!double.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                    continue;

                result.Add(new Stock { Date = date.Date, Price = price });
                count++;
            }

            return result;
        }

        public void InitializeStack(IEnumerable<Stock> stocks, int pushCount)
        {
            stockStack.Clear();
            redoStack.Clear();
            foreach (var s in stocks.Take(pushCount))
                stockStack.Push(s);
        }

        public void PushStock(Stock s)
        {
            stockStack.Push(s);
            redoStack.Clear();
        }

        public Stock? Undo()
        {
            if (stockStack.Count == 0) return null;
            var popped = stockStack.Pop();
            redoStack.Push(popped);
            return popped;
        }

        public Stock? Redo()
        {
            if (redoStack.Count == 0) return null;
            var restored = redoStack.Pop();
            stockStack.Push(restored);
            return restored;
        }

        public List<Stock> GetStackSnapshot()
        {
            return stockStack.ToArray().ToList();
        }

        public (double min, double max) GetMinMax(List<Stock> allStocks, DateTime start, DateTime end)
        {
            var filtered = allStocks.Where(s => s.Date >= start && s.Date <= end).Select(s => s.Price).ToList();
            if (filtered.Count == 0) return (0, 0);
            return (filtered.Min(), filtered.Max());
        }

        private static string[] SplitCsvLine(string line)
        {
            var parts = new List<string>();
            var sb = new System.Text.StringBuilder();
            bool inQuotes = false;
            foreach (char c in line)
            {
                if (c == '"') { inQuotes = !inQuotes; continue; }
                if (c == ',' && !inQuotes)
                {
                    parts.Add(sb.ToString());
                    sb.Clear();
                }
                else sb.Append(c);
            }
            parts.Add(sb.ToString());
            return parts.ToArray();
        }
    }
}
