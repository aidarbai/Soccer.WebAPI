using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Soccer.COMMON.Constants;
using Microsoft.Extensions.Logging;
using Soccer.BLL.Helpers;
using Soccer.BLL.Services.Interfaces;

namespace Soccer.BLL.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpClientService> _logger;
        public HttpClientService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T?> GetDataAsync<T>(string url)
        {
            var httpRequestMessage = GetHttpRequestMessage(url);

            try
            {
                var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return default;
                }

                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions { MaxDepth = 10, PropertyNameCaseInsensitive = true };
                options.Converters.Add(new Int32NullConverter());

                var responseImportDTO = await JsonSerializer.DeserializeAsync
                    <T>(contentStream, options);

                return responseImportDTO;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default;
            }
        }

        private HttpRequestMessage GetHttpRequestMessage(string url)
        {
            return new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                                {
                                    { AppConstants.FootballAPI.HEADERKEY, _configuration["Football-API:ApiKey"] },
                                    { AppConstants.FootballAPI.HEADERHOST, _configuration["Football-API:Host"] },
                                }
            };
        }
    }
}
