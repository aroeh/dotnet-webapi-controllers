using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.API.Models.V2;

public record Restuarant
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string CuisineType { get; set; } = string.Empty;

    public Uri? Website { get; set; }

    [Phone]
    public string Phone { get; set; } = string.Empty;

    public Location Address { get; set; } = new Location();
}
