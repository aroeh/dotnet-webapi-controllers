using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Models;

public record FilterQueryParameters : PaginationQueryParameters
{
    [FromQuery(Name = "name")]
    public string[]? Names { get; init; } = default!;

    [FromQuery(Name = "cuisine")]
    public string? CuisineType { get; init; } = default!;
}
