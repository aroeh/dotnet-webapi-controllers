namespace WebApiHttpClient.HttpClientHelpers;

public class HttpFactoryHelper(ILogger<HttpFactoryHelper> log, IHttpClientFactory factory) : HttpBase(log, factory.CreateClient("restuarantClient"))
{
}
