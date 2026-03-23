using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.API.Models.V2;

public record Location
{
    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string State { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = "United States";

    [Required]
    [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Zip Code must be in one of the following formats: xxxxx, xxxxx xxxx, xxxxx-xxxx")]
    public string ZipCode { get; set; } = string.Empty;
}
