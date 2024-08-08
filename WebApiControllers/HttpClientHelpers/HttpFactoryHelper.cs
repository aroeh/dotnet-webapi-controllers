using Newtonsoft.Json;

namespace WebApiControllers.HttpClientHelpers
{
    public class HttpFactoryHelper
    {
        private readonly ILogger<HttpFactoryHelper> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;

        public HttpFactoryHelper(ILogger<HttpFactoryHelper> log, IHttpClientFactory factory)
        {
            logger = log;
            httpClientFactory = factory;

            logger.LogInformation("Create http client instance from factory");
            httpClient = httpClientFactory.CreateClient("restuarantClient");
        }

        public async Task<T> GetAsync<T>(string url)
        {
            // there are many things that can be added here, like setting headers or adding query parameters
            
            logger.LogInformation("Initiating Get Request to {url}", url);
            // initiate the http request
            HttpResponseMessage response = await httpClient.GetAsync(url);

            return await ValidateAndDeserialize<T>(response);
        }

        public async Task<Y> PostAsync<T, Y>(string url, T request)
        {
            // there are many things that can be added here, like setting headers or adding query parameters
            
            logger.LogInformation("Initiating Get Request to {url}", url);
            // initiate the http request
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, request);

            return await ValidateAndDeserialize<Y>(response);
        }

        public async Task<Y> PutAsync<T, Y>(string url, T request)
        {
            // there are many things that can be added here, like setting headers or adding query parameters
            
            logger.LogInformation("Initiating Get Request to {url}", url);
            // initiate the http request
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, request);

            return await ValidateAndDeserialize<Y>(response);
        }

        private async Task<T> ValidateAndDeserialize<T>(HttpResponseMessage response)
        {
            // ensure the response was successful - if not successful, then return a default version of the object
            if (!response.IsSuccessStatusCode)
            {
                logger.LogInformation("Request Completed with Failed Status Code of {code}", response.StatusCode);
                return default;
            }

            logger.LogInformation("Request Completed with Success Status Code of {code}", response.StatusCode);

            // read the response body
            string content = await response.Content.ReadAsStringAsync();

            logger.LogInformation("Deserializing response body");
            var deserializedContent = JsonConvert.DeserializeObject<T>(content);
            return deserializedContent;
        }
    }
}
