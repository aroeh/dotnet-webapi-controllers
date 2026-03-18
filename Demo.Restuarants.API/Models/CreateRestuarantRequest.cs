using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.API.Models;

public record CreateRestuarantRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string CuisineType { get; set; } = string.Empty;

    public Uri? Website { get; set; }

    [Phone]
    public string Phone { get; set; } = string.Empty;

    public CreateLocationRequest Address { get; set; } = new();
}
