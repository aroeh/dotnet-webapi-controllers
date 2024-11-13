using System.Text.Json;

namespace WebApiControllers.HttpClientHelpers;

public class HttpClientHelper(ILogger<HttpClientHelper> log, HttpClient http)
{
    private readonly ILogger<HttpClientHelper> logger = log;
    private readonly HttpClient httpClient = http;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

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
        
        // deserialize using System.Text.Json
        var deserializedContent = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);

        // if Newtonsoft is preferred, use the JsonConvert class
        //var deserializedContent = JsonConvert.DeserializeObject<T>(content);

        return deserializedContent;
    }
}
