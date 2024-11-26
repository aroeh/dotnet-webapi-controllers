namespace WebApiHttpClient.Models;

public record Restuarant
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string CuisineType { get; set; } = string.Empty;

    public Uri? Website { get; set; }

    public string Phone { get; set; } = string.Empty;

    public Location Address { get; set; } = new Location();
}
