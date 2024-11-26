namespace WebApiHttpClient.HttpClientHelpers;

public class HttpClientHelper(ILogger<HttpClientHelper> log, HttpClient http) : HttpBase(log, http)
{
}
