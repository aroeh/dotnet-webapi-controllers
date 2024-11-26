using System.Text.Json;

namespace WebApiHttpClient.HttpClientHelpers;

/// <summary>
/// This base class contains the common code for initiating http client methods for HTTP requests.
/// Both the factory and http client helpers derive from this base class to demonstrate how multiple 
/// clients can share common code.
/// 
/// Methods here are set as protected internal so derived and other classes in the assembly can directly
/// call to these methods.  But there are other ways this class could be structured.
/// 
/// Methods could be made virtual so derived classes can override them and alter/extend functionality
/// Methods could be made abstract so derived classes must implement them and define unique functionality
/// </summary>
/// <param name="log">Logger class instantiated from the derived class</param>
/// <param name="http">HTTP client instantiated from the derived class</param>
public abstract class HttpBase(ILogger log, HttpClient http)
{
    private readonly ILogger logger = log;
    private readonly HttpClient httpClient = http;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected internal async Task<T> GetAsync<T>(string url)
    {
        // there are many things that can be added here, like setting headers or adding query parameters

        logger.LogInformation("Initiating Get Request to {url}", url);
        // initiate the http request
        HttpResponseMessage response = await httpClient.GetAsync(url);

        return await ValidateAndDeserialize<T>(response);
    }

    protected internal async Task<Y> PostAsync<T, Y>(string url, T request)
    {
        // there are many things that can be added here, like setting headers or adding query parameters

        logger.LogInformation("Initiating Get Request to {url}", url);
        // initiate the http request
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, request);

        return await ValidateAndDeserialize<Y>(response);
    }

    protected internal async Task<Y> PutAsync<T, Y>(string url, T request)
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
