using System.ComponentModel.DataAnnotations;
using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Client.MVC.Models;

public class RestuarantFormViewModel
{
    public string? Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    [Display(Name = "Cuisine")]
    public string CuisineType { get; set; } = string.Empty;

    [Url]
    [Display(Name = "Website")]
    public string? Website { get; set; }

    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Street { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string State { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Country { get; set; } = "United States";

    [Required]
    [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Zip Code must be in one of the following formats: xxxxx, xxxxx xxxx, xxxxx-xxxx")]
    [Display(Name = "Zip Code")]
    public string ZipCode { get; set; } = string.Empty;

    public static RestuarantFormViewModel FromRestuarant(RestuarantBO restuarant) => new()
    {
        Id = restuarant.Id,
        Name = restuarant.Name,
        CuisineType = restuarant.CuisineType,
        Website = restuarant.Website?.ToString(),
        Phone = restuarant.Phone,
        Street = restuarant.Address.Street,
        City = restuarant.Address.City,
        State = restuarant.Address.State,
        Country = restuarant.Address.Country,
        ZipCode = restuarant.Address.ZipCode
    };

    public CreateRestuarantRequest ToCreateRequest() => new()
    {
        Name = Name,
        CuisineType = CuisineType,
        Website = string.IsNullOrWhiteSpace(Website) ? null : new Uri(Website),
        Phone = Phone,
        Address = new CreateLocationRequest
        {
            Street = Street,
            City = City,
            State = State,
            Country = Country,
            ZipCode = ZipCode
        }
    };

    public UpdateRestuarantRequest ToUpdateRequest() => new()
    {
        Name = Name,
        CuisineType = CuisineType,
        Website = string.IsNullOrWhiteSpace(Website) ? null : new Uri(Website),
        Phone = Phone,
        Address = new UpdateLocationRequest
        {
            Street = Street,
            City = City,
            State = State,
            Country = Country,
            ZipCode = ZipCode
        }
    };
}
