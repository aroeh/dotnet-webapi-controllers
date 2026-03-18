namespace Demo.Restuarants.API.Shared.Models;

public record FilterQueryParametersBO
(
    string[]? Names,
    string? CuisineType
) : PaginationQueryParametersBO;
