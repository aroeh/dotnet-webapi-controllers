using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.API.Models;

public record UpdateRestuarantRequest
{
    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(30)]
    public string? CuisineType { get; set; }

    public Uri? Website { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public UpdateLocationRequest? Address { get; set; }
}
