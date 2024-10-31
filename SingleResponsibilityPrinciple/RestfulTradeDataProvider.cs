using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        private readonly string _url;
        private readonly ILogger _logger;

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<string> GetTradeData()
        {
            try
            {
                return GetTradeDataAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message); // Log the exception message
                return Enumerable.Empty<string>(); // Return an empty list on error
            }
        }

        private async Task<IEnumerable<string>> GetTradeDataAsync()
        {
            using var httpClient = new HttpClient();

            // Fetch the data from the API
            var response = await httpClient.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            // Read the content as a string
            var jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON data into a list of strings
            var tradeData = JsonSerializer.Deserialize<List<string>>(jsonData);

            return tradeData ?? Enumerable.Empty<string>();
        }
    }
}
