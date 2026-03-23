using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.API.Models;

public record UpdateLocationRequest
{
    [StringLength(150)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(2)]
    public string? State { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Zip Code must be in one of the following formats: xxxxx, xxxxx xxxx, xxxxx-xxxx")]
    public string? ZipCode { get; set; }
}
