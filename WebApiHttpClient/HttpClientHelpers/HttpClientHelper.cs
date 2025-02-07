namespace WebApiHttpClient.HttpClientHelpers;

public class HttpClientHelper(ILogger<HttpClientHelper> log, HttpClient http) : HttpBase(log, http)
{
    private readonly HttpClient httpClient = http;

    public void SetHeader()
    {
        httpClient.DefaultRequestHeaders.Add("test-header", "test-value");
    }
}
