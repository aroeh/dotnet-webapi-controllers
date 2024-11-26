namespace WebApiHttpClient.Models;

public record Location
{
    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = "United States";

    public string ZipCode { get; set; } = string.Empty;
}
