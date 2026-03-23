using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Demo.Restuarants.API.Models;

public record FilterQueryParameters : PaginationQueryParameters
{
    /*
     * By default Refit serializes using the property name
     * Using the AliasAs attribute instructs Refit to use the specified name for serialization
     */
    [AliasAs("name")]
    [FromQuery(Name = "name")]
    public string[]? Names { get; init; } = default!;

    [AliasAs("cuisine")]
    [FromQuery(Name = "cuisine")]
    public string? CuisineType { get; init; } = default!;
}
